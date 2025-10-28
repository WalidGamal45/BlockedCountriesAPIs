namespace BDAssignment.Application.Models
{
    public class TemporalBlockRequestDto
    {
        public string CountryCode { get; set; }     // مثال: "EG"
        public string CountryName { get; set; }     // مثال: "Egypt"
        public int DurationMinutes { get; set; }    // عدد الدقايق للحظر
    }
}
