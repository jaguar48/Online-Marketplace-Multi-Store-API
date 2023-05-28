using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Marketplace.BLL.Implementation.UserServices;
using Online_Marketplace.BLL.Interface.IUserServices;
using Online_Marketplace.DAL.Entities.Models;
using Online_Marketplace.Shared.DTOs;
using Online_Marketplace.Shared.Filters;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Online_Marketplace.Presentation.Controllers
{
    [ApiController]
    [Route("/marketplace/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authentication;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(IAuthService authentication, UserManager<User> userManager)
        {
            _authentication = authentication;
            _userManager = userManager;
        }

       
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation(Summary = "Authenticate user and create token", Description = "Authenticate user and create token.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Token created successfully.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid user credentials.")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            var response = await _authentication.ValidateUser(user);

            

            if (!response.Success)
                return BadRequest(response);

            return Ok(new { Token = await _authentication.CreateToken(), Role = response.Role });
          
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyUser([FromQuery] string email, [FromQuery] string verificationToken)
        {
            var isVerified = await _authentication.VerifyUser(email, verificationToken);

          
                return Ok("User successfully verified!");
         
        }



        [HttpPost("forgot-password")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation(Summary = "Initiate password reset", Description = "Initiate password reset by sending a password reset email.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Password reset email sent successfully.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid email or email not found.")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email or email not found.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var emailSent = await _authentication.SendPasswordResetEmail(forgotPasswordDto.Email, resetToken);

            if (emailSent)
            {
                return Ok(new { Token = resetToken, Email = forgotPasswordDto.Email });
            }
            else
            {
                return BadRequest("Failed to send password reset email. Please try again later.");
            }
        }

        [HttpPost("reset-password")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation(Summary = "Reset user password", Description = "Reset user password using the password reset token.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Password reset successfully.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid password reset request.")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string token, [FromBody] ResetPasswordDto resetPasswordDto)
        {
            bool passwordResetResult = await _authentication.ResetPassword(email, token, resetPasswordDto.Password);

            if (passwordResetResult)
            {
                return Ok("Password reset successfully.");
            }
            else
            {
                return BadRequest("Failed to reset password. Please try again later.");
            }
        }


    }
}
