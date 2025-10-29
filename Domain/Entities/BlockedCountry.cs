namespace BDAssignment.Domain.Entities
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; }  
        public string CountryName { get; set; }  
        public string BlockType { get; set; }    
        public DateTime? ExpiryDate { get; set; } // لو الحظر مؤقت - تاريخ الانتهاء
    }
}
