using Online_Marketplace.BLL.Helpers;
using Online_Marketplace.Shared.DTOs;

namespace Online_Marketplace.BLL.Interface.IServices
{
    public interface ISellerServices
    {
        Task<string> RegisterSeller(SellerForRegistrationDto sellerForRegistration);

    }
}
