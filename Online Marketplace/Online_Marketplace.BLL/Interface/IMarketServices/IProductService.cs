using Online_Marketplace.BLL.Helpers;
using Online_Marketplace.Shared.DTOs;

namespace Online_Marketplace.BLL.Interface.IMarketServices
{
    public interface IProductService
    {

        public Task<string> CreateProductAsync(ProductCreateDto productDto);
        public  Task<List<ProductCreateDto>> SearchProductsAsync(ProductSearchDto searchDto);
        public Task<bool> AddToCartAsync(int productId, int quantity);

        public Task<string> UpdateProductAsync(int productId, ProductCreateDto productDto);
        public Task<string> DeleteProductAsync(int productId);

        public Task<List<ProductViewDto>> GetSellerProductsAsync();

        public Task<List<ProductViewDto>> ViewProductsAsync();
        public Task<string> AddReview(ReviewDto reviewDto);

        public Task<ProductViewDto> ProductDetailAsync(int id);

        public Task<bool> RemoveFromCartAsync(int productId);
        public Task<List<CartItemDto>> GetCartItemsAsync();
        public Task<CartItemDto> GetCartIdAsync(int id);
        public Task<string> AddCategory(CreateCategoryDto categoryDto);

        public Task<List<CategoryDto>> GetAllCategoriesAsync();

        public Task<CategoryWithProductsDto> GetCategoryWithProductsAsync(int categoryId);
    }
}
