using System.Threading.Tasks;
using BDAssignment.Application.Models;

namespace BDAssignment.Application.Interfaces
{
    public interface IGeoLookupService
    {
        Task<IpLookupResultDto> GetCountryByIPAsync(string ipAddress);
    }
}
