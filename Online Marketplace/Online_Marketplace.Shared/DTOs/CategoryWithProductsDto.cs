using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Marketplace.Shared.DTOs
{
   
        public class CategoryWithProductsDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
          
            public List<ProductsDto> Products { get; set; }
        }

    public class ProductsDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }


}
