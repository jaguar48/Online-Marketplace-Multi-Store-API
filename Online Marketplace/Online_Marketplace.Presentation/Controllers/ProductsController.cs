
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Marketplace.BLL.Interface.IMarketServices;
using Online_Marketplace.DAL.Entities;
using Online_Marketplace.Shared.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace Online_Marketplace.Presentation.Controllers
{

    [ApiController]
    [Route("marketplace/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [Authorize(Roles = "Seller")]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create a new product.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the newly created product.", typeof(Product))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> CreateProduct(ProductCreateDto productdto)
        {

            var product = await _productService.CreateProductAsync(productdto);
            return Ok(product);

        }

        [Authorize(Roles = "Seller")]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Create a new product category.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the newly created category.", typeof(Category ))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto categoryDto )
        {

            var category = await _productService.AddCategory(categoryDto);
            return Ok(category);

        }
        
        [HttpGet ("categories")]
        [SwaggerOperation(Summary = "Get all product categories.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns all created category.", typeof(Category))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Getategories()
        {

            var category = await _productService.GetAllCategoriesAsync ();
            return Ok(category);

        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a single product.", Description = "Requires no authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the single product detail.", typeof(Product))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> ProductDetail(string id)
        {
            if (!int.TryParse(id, out int productId))
            {
                return BadRequest("Invalid product ID");
            }

            var product = await _productService.ProductDetailAsync(productId);
            return Ok(product);
        }

        [Authorize(Roles = "Buyer")]
        [HttpGet("cart/{id}")]
        [SwaggerOperation(Summary = "Get a single cart.", Description = "Requires no authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the single product detail.", typeof(Cart))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> CartDetail(string id)
        {
            if (!int.TryParse(id, out int cartId))
            {
                return BadRequest("Invalid cart ID");
            }

            var cart = await _productService.GetCartIdAsync(cartId);
            return Ok(cart);
        }


        [HttpGet("Search-Products")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Search for products.", Description = "Does not require authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of matching products.", typeof(List<ProductCreateDto>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<List<ProductCreateDto>>> SearchProducts([FromQuery] ProductSearchDto searchDto)
        {

            var products = await _productService.SearchProductsAsync(searchDto);
            return Ok(products);

        }


        [HttpGet]
        [SwaggerOperation(Summary = "Get all products.", Description = "Does not require authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of all products.", typeof(IEnumerable<ProductViewDto>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<IEnumerable<ProductViewDto>>> GetProducts()
        {

            var products = await _productService.ViewProductsAsync();
            return Ok(products);

        }


        [Authorize(Roles = "Buyer")]
        [HttpPost("cart")]
        [SwaggerOperation(Summary = "Add a product to the cart.", Description = "Requires buyer authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The product was successfully added to the cart.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to add product to cart.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
       
        public async Task<IActionResult> AddToCart(string id, int quantity)
        {
            if (!int.TryParse(id, out int productId))
            {
                return BadRequest("Invalid product ID");
            }

            var result = await _productService.AddToCartAsync(productId, quantity);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to add product to cart.");
            }

        }

        [Authorize(Roles = "Buyer")]
        [HttpPost ("removecart")]
        [SwaggerOperation(Summary = "remove a product to the cart.", Description = "Requires buyer authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The product was successfully removed from the cart.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to remove product from cart.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]

        public async Task<IActionResult> RemoveCart(int productId)
        {
           
            var result = await _productService.RemoveFromCartAsync(productId);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to add product to cart.");
            }

        }
        [Authorize(Roles = "Buyer")]
        [HttpGet("viewcart")]
        [SwaggerOperation(Summary = "get carts.", Description = "Requires buyer authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The product was successfully removed from the cart.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to retreive cart.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]

        public async Task<ActionResult<IList<CartItemDto>>> GetCart()
        {
           

            var result = await _productService.GetCartItemsAsync ();

           
                return Ok(result);
       
        }



        [Authorize(Roles = "Seller")]
        [HttpPut("edit/{id}")]
        [SwaggerOperation(Summary = "Update a product.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The product was successfully updated.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
       
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductCreateDto productDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
            return Ok(updatedProduct);
        }


        [Authorize(Roles = "Seller")]
        [HttpDelete("delete/{id}")]
        [SwaggerOperation(Summary = "Delete a product.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The product was successfully deleted.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }


        [Authorize(Roles = "Seller")]
        [HttpGet("seller/products")]
        [SwaggerOperation(Summary = "Get seller products.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The seller products were successfully retrieved.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<List<ProductCreateDto>>> GetSellerProducts()
        {

            var products = await _productService.GetSellerProductsAsync();
            return Ok(products);
        }

        
        [HttpGet("category")]
        [SwaggerOperation(Summary = "Get category with products.", Description = "no authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The category products were successfully retrieved.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetCategoryProducts (int id)
        {

            var category = await _productService.GetCategoryWithProductsAsync (id);
            return Ok(category);
        }


        [Authorize(Roles = "Buyer")]
        [HttpPost("Add-Reviews")]
        [SwaggerOperation(Summary = "Add a review for a product.", Description = "Requires buyer authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The review was successfully added.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> AddReview(ReviewDto reviewDto)
        {

            var result = await _productService.AddReview(reviewDto);
            return Ok(result);

        }



    }

}
