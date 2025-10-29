namespace BDAssignment.Application.Models
{
    public class BlockCountryRequestDto
    {
        public string CountryCode { get; set; }   // like => "US"  or  "EG"
        public string CountryName { get; set; }   // like => "United States"  or  "Egypt"
    }
}
