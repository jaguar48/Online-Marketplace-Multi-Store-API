using Online_Marketplace.DAL.Entities.Models;
using Online_Marketplace.Shared;
using Online_Marketplace.Shared.DTOs;

namespace Online_Marketplace.BLL.Interface.IUserServices
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken();

        public Task<User> VerifyUser(string email, string verificationToken);
        public Task<bool> SendVerificationEmail(string email, string verificationToken);
        public Task<bool> ResetPassword(string email, string token, string newPassword);
        public Task<bool> SendPasswordResetEmail(string email, string resetToken);
    }

}