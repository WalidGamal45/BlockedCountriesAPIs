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

        //  جلب بيانات الدولة من IP
        [HttpGet("ip/lookup")]
        public async Task<IActionResult> LookupIp([FromQuery] string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return BadRequest("من فضلك أدخل IP Address صالح");

            var result = await _geoLookupService.GetCountryByIPAsync(ipAddress);

            if (result == null)
                return NotFound("ما قدرناش نجيب بيانات الـ IP ");

            return Ok(result);
        }

        //  التحقق هل الـ (IP) محظور
        [HttpGet("ip/check-block")]
        public async Task<IActionResult> CheckIfIpBlocked([FromQuery] string ipAddress)
        {
            // لو المستخدم ما كتبش IP، نجيب IP بتاعه

            if (string.IsNullOrWhiteSpace(ipAddress))
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var ipInfo = await _geoLookupService.GetCountryByIPAsync(ipAddress);

            if (ipInfo == null)
                return BadRequest("ما قدرناش نجيب بيانات الـ IP");

            // نشوف هل الدولة محظورة
            bool isBlocked = _countryBlockService.IsBlocked(ipInfo.CountryCode);

            if (isBlocked)
                return Ok($" الـ IP {ipAddress} تابع لدولة محظورة: {ipInfo.CountryName} ({ipInfo.CountryCode})");

            return Ok($" الـ IP {ipAddress} مش محظور (الدولة: {ipInfo.CountryName} - {ipInfo.CountryCode})");
        }
        //  حذف دولة محظورة
        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return BadRequest("من فضلك أدخل كود الدولة");

            bool removed = _countryBlockService.UnblockCountry(countryCode);

            if (!removed)
                return NotFound("الدولة دي مش موجودة في قائمة الحظر ❌");

            return Ok($"{countryCode} تم إزالتها من قائمة الحظر ");
        }
        [HttpGet("ip/test")]
        public async Task<IActionResult> TestIpApi([FromQuery] string ipAddress = "8.8.8.8")
        {
            var result = await _testIpApiService.GetIpInfoAsync(ipAddress);
            return Ok(result);
        }


    }
}
