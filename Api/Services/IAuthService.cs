using ContactApi.Domain.DTOs;

namespace ContactApi.Api.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}
