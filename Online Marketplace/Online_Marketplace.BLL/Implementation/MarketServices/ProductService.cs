using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Online_Marketplace.BLL.Helpers;
using Online_Marketplace.BLL.Interface.IMarketServices;
using Online_Marketplace.DAL.Entities;
using Online_Marketplace.DAL.Entities.Models;
using Online_Marketplace.Logger.Logger;
using Online_Marketplace.Shared.DTOs;
using System.Net.Http.Headers;
using System.Security.Claims;


namespace Online_Marketplace.BLL.Implementation.MarketServices
{

    public class ProductServices : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Buyer> _buyerRepo;
        private readonly IRepository<Seller> _sellerRepo;
        private readonly IRepository<Cart> _cartRepo;
        private readonly IRepository<Category> _catRepo;
        private readonly IRepository<OrderItem> _orderitemRepo;
        private readonly IRepository<ProductReviews> _productreivewRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductServices(IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IUnitOfWork unitOfWork, IMapper mapper)
        {

            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepo = _unitOfWork.GetRepository<Product>();
            _catRepo = _unitOfWork.GetRepository<Category>();
            _sellerRepo = _unitOfWork.GetRepository<Seller>();
            _cartRepo = _unitOfWork.GetRepository<Cart>();
            _buyerRepo = _unitOfWork.GetRepository<Buyer>();
            _orderitemRepo = _unitOfWork.GetRepository<OrderItem>();
            _productreivewRepo = unitOfWork.GetRepository<ProductReviews>();

        }




        public async Task<string> CreateProductAsync(ProductCreateDto productDto)
        {
            try
            {
                if (productDto.File == null || productDto.File.Length == 0)
                {
                    throw new Exception("Image file is required");
                }

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

              
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.File.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName).Replace('\\', '/');


                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await productDto.File.CopyToAsync(stream);
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    throw new Exception("User not found");
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,
                    Brand = productDto.Brand,
                    CategoryId = productDto.CategoryId,
                    ImagePath = dbPath 
                };

                Seller seller = await _sellerRepo.GetSingleByAsync(s => s.UserId == userId);

                if (seller == null)
                {
                    throw new Exception("Seller not found");
                }

                product.SellerId = seller.Id;

            
                var categories = await _catRepo.GetAllAsync(c => c.SellerId == seller.Id);

               
                if (productDto.CategoryId > 0)
                {
                    var category = categories.FirstOrDefault(c => c.Id == productDto.CategoryId);
                    if (category != null)
                    {
                        product.Category = category;
                    }
                }

                await _productRepo.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                var result = new { success = true, message = "Product created successfully", product.ImagePath };
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { success = false, message = ex.Message });
            }
        }




        public async Task<string> AddCategory(CreateCategoryDto categoryDto)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            { 
                throw new Exception("User not found");
            }

            var category = _mapper.Map<Category>(categoryDto);

            Seller seller = await _sellerRepo.GetSingleByAsync(s => s.UserId == userId);

            if (seller == null)
            {
                throw new Exception("Seller not found");
            }

            category.SellerId = seller.Id;

            await _catRepo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            var result = new { success = true, message = "category created successfully" };
            return JsonConvert.SerializeObject(result);

        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _catRepo.GetAllAsync(include: c => c.Include(p => p.Products));

            return _mapper.Map<List<CategoryDto>>(categories);
        }

    
        public async Task<string> UpdateProductAsync(int productId, ProductCreateDto productDto)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new Exception("User not found");
            }

            var existingProduct = await _productRepo.GetSingleByAsync(x => x.Id == productId, include: x => x.Include(x => x.Seller));

            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }


            if (existingProduct.Seller.UserId != userId)
            {
                throw new Exception("You do not have permission to update this product");
            }


            var ProductCreateDto = _mapper.Map(productDto, existingProduct);



            await _productRepo.UpdateAsync(ProductCreateDto);
            await _unitOfWork.SaveChangesAsync();

            var result = new { success = true, message = "Product updated successfully" };
            return JsonConvert.SerializeObject(result);
            
        }
       

        public async Task<bool> RemoveFromCartAsync(int productId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyer = await _buyerRepo.GetSingleByAsync(b => b.UserId == userId);

            if (buyer == null)
            {
                throw new Exception("Buyer not found");
            }

            var cart = await _cartRepo.GetSingleByAsync(c => c.BuyerId == buyer.Id, include: q => q.Include(c => c.CartItems));

            if (cart == null || cart.CartItems == null)
            {
                return false;
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                return false;
            }

            cart.CartItems.Remove(cartItem);

            await _cartRepo.UpdateAsync(cart);

            _logger.LogInfo($"Removed product with ID {productId} from cart of buyer with ID {buyer.Id}");

            return true;
        }
        public async Task<List<CartItemDto>> GetCartItemsAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyer = await _buyerRepo.GetSingleByAsync(b => b.UserId == userId);

            if (buyer == null)
            {
                throw new Exception("Buyer not found");
            }

            var cart = await _cartRepo.GetSingleByAsync(c => c.BuyerId == buyer.Id, include: q => q.Include(c => c.CartItems).ThenInclude(ci => ci.Product));

            if (cart == null || cart.CartItems == null)
            {
                return new List<CartItemDto>();
            }

            return cart.CartItems.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                Name = ci.Product.Name,
                Quantity = ci.Quantity,
                Price = ci.Product.Price,
                CartId = ci.CartId
            }).ToList();
        }
        public async Task<CartItemDto> GetCartIdAsync(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyer = await _buyerRepo.GetSingleByAsync(b => b.UserId == userId);

            if (buyer == null)
            {
                throw new Exception("Buyer not found");
            }

            var cart = await _cartRepo.GetSingleByAsync(c => c.Id == id && c.BuyerId == buyer.Id, include: q => q.Include(c => c.CartItems).ThenInclude(ci => ci.Product));

            if (cart == null)
            {
                throw new Exception("Cart not found");
            }


            return _mapper.Map<CartItemDto>(cart);

        
        }

       
        public async Task<string> DeleteProductAsync(int productId)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new Exception("User not found");
            }

            var existingProduct = await _productRepo.GetSingleByAsync(x => x.Id == productId, include: x => x.Include(s => s.Seller));

            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }


            var buyer = await _sellerRepo.GetSingleByAsync(b => b.UserId == userId);



            if (existingProduct.Seller.UserId != userId)
            {
                throw new Exception("You do not have permission to delete this product");
            }

            await _productRepo.DeleteAsync(existingProduct);
            

            var result = new { success = true, message = "Product deleted successfully" };
            return JsonConvert.SerializeObject(result);

          
        }


        public async Task<List<ProductViewDto>> GetSellerProductsAsync()
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User not found");
            }

            Seller seller = await _sellerRepo.GetSingleByAsync(x => x.UserId == userId);

            IEnumerable<Product> sellerPruducts = await _productRepo.GetByAsync(p => p.SellerId == seller.Id);


            return _mapper.Map<List<ProductViewDto>>(sellerPruducts);
        }



        public async Task<List<ProductCreateDto>> SearchProductsAsync(ProductSearchDto searchDto)
        {

            var products = await _productRepo.GetAllAsync();



            if (!string.IsNullOrEmpty(searchDto.Search))
            {
                products = products.Where(p => p.Name.Contains(searchDto.Search, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            var productDtos = _mapper.Map<List<ProductCreateDto>>(products);

            return productDtos;


        }

        public async Task<List<ProductViewDto>> ViewProductsAsync()
        {
            var products = await _productRepo.GetAllAsync(include: p => p.Include(r => r.ProductReview));

            var productDtos = _mapper.Map<List<ProductViewDto>>(products);

            return productDtos;
        }




        public async Task<bool> AddToCartAsync(int productId, int quantity)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyer = await _buyerRepo.GetSingleByAsync(b => b.UserId == userId);

            if (buyer == null)
            {
                throw new Exception("Buyer not found");
            }

            var product = await _productRepo.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            if (quantity <= 0 || quantity > product.StockQuantity)
            {
                throw new ArgumentException("Invalid quantity.");
            }


            var cart = await _cartRepo.GetSingleByAsync(c => c.BuyerId == buyer.Id, include: q => q.Include(c => c.CartItems));

            if (cart == null)
            {
                cart = new Cart { BuyerId = buyer.Id };
                await _cartRepo.AddAsync(cart);

            }

            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.CartItems.Add(cartItem);
            }

            await _cartRepo.UpdateAsync(cart);

            _logger.LogInfo($"Added product with ID {productId} to cart of buyer with ID {buyer.Id}");

            return true;

        }


        public async Task<CategoryWithProductsDto> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await _catRepo.GetSingleByAsync(c => c.Id == categoryId, include: c => c.Include(p => p.Products));

            if (category == null)
            {
                throw new Exception("Category not found");
            }


            var categoryDto = new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(p => new ProductsDto
                {

                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                }).ToList()
            };

            return _mapper.Map<CategoryWithProductsDto>(category);
        }


        public async Task<ProductViewDto> ProductDetailAsync(int id)
        {
            var product = await _productRepo.GetSingleByAsync(
        x => x.Id == id,
        include: q => q
            .Include(p => p.Category)
            .Include(p => p.Seller)
    );

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            return new ProductViewDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Brand = product.Brand,
                CategoryId = product.Category.Id,
                CategoryName = product.Category.Name,
                BusinessName = product.Seller.BusinessName,
                ImageUrl = product.ImagePath

            };
        }

        public async Task<string> AddReview(ReviewDto reviewDto)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyer = await _buyerRepo.GetSingleByAsync(b => b.UserId == userId);

            var order = await _orderitemRepo.GetSingleByAsync(c => c.ProductId == reviewDto.ProductId && c.Order.BuyerId == buyer.Id,
                include: oi => oi.Include(oi => oi.Order));

            if (order == null)
            {
                _logger.LogError("Order not found");
                return "Order not found";
            }

            var review = _mapper.Map<ProductReviews>(reviewDto);

            review.BuyerId = buyer.Id;
            review.DateCreated = DateTime.UtcNow;

            await _productreivewRepo.AddAsync(review);

            _logger.LogInfo($"Added review with ID {review.Id}");

            return "Review added successfully";
        }

        
    }
}
