using CartApi.Domain.Entities;
using HotelApi.DTOS.WriteDtos;
using System.Threading.Tasks;

namespace CartApi.Interfaces
{
       public interface IAuthManager
        {
            Task<bool> AuthenticateUserAsync(LoginWriteDto loginInfo);

            Task<string> GenerateTokenAsync(string userName);

            Task<ApiUser> GetUserFromTokenAsync(string token);
        }
}
