using BDAssignment.Application.Interfaces;
using BDAssignment.Application.Models;
using BDAssignment.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BDAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockedCountriesController : ControllerBase
    {
        private readonly IGeoLookupService _geoLookupService;
        private readonly CountryBlockService _countryBlockService;
        private readonly TestIpApiService _testIpApiService;


        public BlockedCountriesController(
            IGeoLookupService geoLookupService,
            CountryBlockService countryBlockService,
            TestIpApiService testIpApiService)
        {
            _geoLookupService = geoLookupService;
            _countryBlockService = countryBlockService;
            _testIpApiService = testIpApiService;
        }

        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] BlockCountryRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.CountryCode))
                return BadRequest("كود الدولة مطلوب");

            var success = _countryBlockService.BlockCountry(request.CountryCode, request.CountryName);

            if (!success)
                return Conflict("الدولة دي محظورة بالفعل");

            return Ok($"{request.CountryName} ({request.CountryCode}) تم حظرها بنجاح ");
        }

        // حظر مؤقت لدولة
        [HttpPost("temporal-block")]
        public IActionResult TemporalBlock([FromBody] TemporalBlockRequestDto request)
        {
            try
            {
                var success = _countryBlockService.TemporalBlockCountry(request);

                if (!success)
                    return Conflict("الدولة دي محظورة بالفعل مؤقتًا أو دائمًا");

                return Ok($"{request.CountryName} ({request.CountryCode}) تم حظرها لمدة {request.DurationMinutes} دقيقة ⏳");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //  عرض الدول المحظورة
        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries()
        {
            return Ok(_countryBlockService.GetAllBlockedCountries());
        }

        //  ip <-- جلب بيانات الدولة من 
        [HttpGet("ip/lookup")]
        public async Task<IActionResult> LookupIp([FromQuery] string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return BadRequest("Please enter IP Address valid");

            var result = await _geoLookupService.GetCountryByIPAsync(ipAddress);

            if (result == null)
                return NotFound(" We could not get the IP data. ");

            return Ok(result);
        }

        [HttpGet("ip/check-block")]
        public async Task<IActionResult> CheckIfIpBlocked([FromQuery] string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            //  الخطوة 1: بنجيب بيانات الـ IP من موقع ipapi.co
            var ipInfo = await _geoLookupService.GetCountryByIPAsync(ipAddress);

            //  الخطوة 2: بنأكد إن عندنا كود دولة
            if (ipInfo == null || string.IsNullOrEmpty(ipInfo.CountryCode))
                return BadRequest("We could not get the country data from the (IP) OR code. The reference is empty.");

            //  الخطوة 3: بنستخدم الكود علشان نعرف هل الدولة دي محظورة ولا لا
            bool isBlocked = _countryBlockService.IsBlocked(ipInfo.CountryCode);

            if (isBlocked)
                return Ok($"الـ IP {ipAddress} تابع لدولة محظورة: {ipInfo.CountryName} ({ipInfo.CountryCode})");

            return Ok($"الـ IP {ipAddress} مش محظور (الدولة: {ipInfo.CountryName} - {ipInfo.CountryCode})");
        }


        //  حذف دولة محظورة
        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return BadRequest("من فضلك أدخل كود الدولة");

            bool removed = _countryBlockService.UnblockCountry(countryCode);

            if (!removed)
                return NotFound("الدولة دي مش موجودة في قائمة الحظر ");

            return Ok($"{countryCode} تم إزالتها من قائمة الحظر ");
        }
        [HttpGet("ip/test")]
        public async Task<IActionResult> TestIpApi([FromQuery] string ipAddress = "8.8.8.8")
        {
            var result = await _testIpApiService.GetIpInfoAsync(ipAddress);
            return Ok(result);
        }
        //  عرض الدول مع Pagination وSearch
        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = _countryBlockService.GetBlockedCountriesPaged(search, page, pageSize);
            return Ok(result);
        }




    }
}
