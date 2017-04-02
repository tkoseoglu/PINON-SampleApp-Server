using System.Threading.Tasks;

namespace PINON.SampleApp.Web.Tokens
{
    public interface IJwtFactory
    {
        Task<string> GetJwtTokenAsync(string userName, bool isAdmin);
    }
}