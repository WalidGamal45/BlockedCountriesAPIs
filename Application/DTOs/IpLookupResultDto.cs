namespace BDAssignment.Application.Models
{
    public class IpLookupResultDto
    {
        public string Ip { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public string Org { get; set; } // to company 
    }
}
