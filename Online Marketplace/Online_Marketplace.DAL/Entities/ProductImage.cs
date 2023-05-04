using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Marketplace.DAL.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

}
