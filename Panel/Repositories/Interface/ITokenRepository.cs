using Microsoft.AspNetCore.Identity;

namespace Panel.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
