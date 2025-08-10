using Transference.Dtos;
using Transference.Models;
using Transference.Resources.Bases.Response;

namespace Transference.Interface
{
    public interface IApiService
    {
        Task<bool> LoginAsync(string usuario, string password, string authType);
        Task<string> GetActualStateAsync(int userId);
        Task<SigningUpModel> GetActualStateAsync(SigningUpModel signingUpModel, string TypeSigning);
        Task<NominatimResponse> GetLocationDetailsAsync(double latitude, double longitude);
        Task<ProvComResponseDto> GetProvComIDAsync(ProvComRequestDto provComRequestDto);
        Task<BaseResponse<bool>> RegisterSigning(SigningUpModel requestDto);
        Task<BaseResponse<bool>> GetOverTime(int userId);
        void SetAuthorizationToken(string token);
    }
}
