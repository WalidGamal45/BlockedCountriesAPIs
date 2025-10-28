namespace BDAssignment.Domain.Entities
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; }   // مثال: "EG" أو "US"
        public string CountryName { get; set; }   // اسم الدولة
        public string BlockType { get; set; }     // Permanent أو Temporal
        public DateTime? ExpiryDate { get; set; } // لو الحظر مؤقت - تاريخ الانتهاء
    }
}
