using System.Threading.Tasks;

namespace PINON.SampleApp.Auth.Tokens
{
    public interface IJwtFactory
    {
        Task<string> GetJwtTokenAsync(string userName, bool isAdmin);
    }
}