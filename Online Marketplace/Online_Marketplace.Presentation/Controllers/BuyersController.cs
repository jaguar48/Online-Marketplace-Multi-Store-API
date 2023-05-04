using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Marketplace.BLL.Implementation.Services;
using Online_Marketplace.BLL.Interface.IProfileServices;
using Online_Marketplace.BLL.Interface.IServices;
using Online_Marketplace.Shared.DTOs;
using Online_Marketplace.Shared.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace Online_Marketplace.Presentation.Controllers
{
    [ApiController]
    [Route("/marketplace/buyer")]
    public class BuyersController : ControllerBase
    {

        private readonly IBuyerServices _buyerServices;
        private readonly IBuyerProfileServices _buyerProfileServices;

        public BuyersController(IBuyerServices buyerServices, IBuyerProfileServices buyerProfileServices)
        {
            _buyerServices = buyerServices;
            _buyerProfileServices = buyerProfileServices;
        }




        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [SwaggerOperation("Registers a new buyer.")]
        [SwaggerResponse(200, "The buyer has been successfully registered.", typeof(BuyerForRegistrationDto))]
        public async Task<IActionResult> RegisterBuyer([FromBody] BuyerForRegistrationDto buyerForRegistration)
        {
            var result = await _buyerServices.RegisterBuyer(buyerForRegistration);
            return Ok(result);
        }




        [HttpPost("createProfile")]
        [Authorize(Roles = "Buyer")]
        [SwaggerOperation(Summary = "Create a new buyer profile.", Description = "Requires buyer authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The buyer profile was created successfully.", typeof(BuyerProfileDto))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The user is not authorized to perform this action.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> CreateProfile([FromBody] BuyerProfileDto buyerProfile)
        {
            var response = await _buyerProfileServices.CreateProfile(buyerProfile);
            return Ok(response);

        }



        [HttpPost("updateProfile")]
        [Authorize(Roles = "Buyer")]
        [SwaggerOperation(Summary = "Updates a buyer's profile.", Description = "Requires authentication with buyer role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated buyer profile.", typeof(BuyerProfileDto))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> UpdateProfile([FromBody] BuyerProfileDto buyerProfile)
        {
            var response = await _buyerProfileServices.UpdateProfile(buyerProfile);
            return Ok(response);

        }
    }
}
