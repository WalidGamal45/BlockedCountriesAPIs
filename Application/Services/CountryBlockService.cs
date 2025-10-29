using BDAssignment.Application.Models;
using BDAssignment.Domain.Entities;
using System.Collections.Concurrent;
using System.Text.Json;

namespace BDAssignment.Application.Services
{
    public class CountryBlockService
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new();
        private readonly string _filePath = "blocked_countries.json";

        // Load file from file on startup
        public CountryBlockService()
        {
            LoadFromFile();
        }

        //  Load data from file
        private void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                var list = JsonSerializer.Deserialize<List<BlockedCountry>>(json);
                if (list != null)
                {
                    foreach (var item in list)
                        _blockedCountries[item.CountryCode] = item;
                }
            }
        }

        //  Save data after any modification
        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(
                _blockedCountries.Values.ToList(),
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(_filePath, json);
        }

        //  Add permanent block
        public bool BlockCountry(string countryCode, string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return false;

            var key = countryCode.Trim().ToUpperInvariant();

            var added = _blockedCountries.TryAdd(key, new BlockedCountry
            {
                CountryCode = key,
                CountryName = countryName?.Trim(),
                BlockType = "Permanent",
                ExpiryDate = null
            });

            if (added) SaveToFile();
            return added;
        }

        //  Add a temporary block 
        public bool TemporalBlockCountry(TemporalBlockRequestDto request)
        {
            // تحقق إن المدة بين 1 و 1440 دقيقة
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                throw new ArgumentException("المدة لازم تكون بين 1 و 1440 دقيقة");

            // لو الدولة محظورة بالفعل
            if (_blockedCountries.ContainsKey(request.CountryCode))
                return false;

            var expiry = DateTime.UtcNow.AddMinutes(request.DurationMinutes);

            var added = _blockedCountries.TryAdd(request.CountryCode, new BlockedCountry
            {
                CountryCode = request.CountryCode,
                CountryName = request.CountryName,
                BlockType = "Temporal",
                ExpiryDate = expiry
            });

            if (added) SaveToFile(); // حفظ بعد الإضافة
            return added;
        }

        //  إزالة الدول اللي انتهى وقتها
        public void RemoveExpiredBlocks()
        {
            var expired = _blockedCountries.Values
                .Where(x => x.BlockType == "Temporal" && x.ExpiryDate <= DateTime.UtcNow)
                .Select(x => x.CountryCode)
                .ToList();

            foreach (var code in expired)
                _blockedCountries.TryRemove(code, out _);

            if (expired.Any()) SaveToFile(); // حفظ بعد التنظيف
        }

        //  عرض كل الدول المحظورة
        public IEnumerable<BlockedCountry> GetAllBlockedCountries()
        {
            return _blockedCountries.Values;
        }

        //  حذف دولة من الحظر
        public bool UnblockCountry(string countryCode)
        {
            var removed = _blockedCountries.TryRemove(countryCode, out _);
            if (removed) SaveToFile(); // حفظ بعد الحذف
            return removed;
        }

        //  التحقق إذا كانت الدولة محظورة
        public bool IsBlocked(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return false;

            var key = countryCode.Trim().ToUpperInvariant();
            return _blockedCountries.ContainsKey(key);
        }

        // عرض الدول المحظورة مع البحث والتجزئة (Pagination + Filter)
        public IEnumerable<BlockedCountry> GetBlockedCountriesPaged(string? search = null, int page = 1, int pageSize = 10)
        {
            var query = _blockedCountries.Values.AsEnumerable();

            //  بحث بالاسم أو الكود
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(c =>
                    c.CountryCode.ToLower().Contains(search) ||
                    c.CountryName.ToLower().Contains(search));
            }

            //  ترتيب أبجدي (اختياري)
            query = query.OrderBy(c => c.CountryName);

            //  تطبيق التجزئة
            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

    }
}
