using Auditory.Domain.Entities;

namespace Auditory.Infrastructure.Auth;

public interface IJwtService
{
    string GenerateAccessToken(User user);
}