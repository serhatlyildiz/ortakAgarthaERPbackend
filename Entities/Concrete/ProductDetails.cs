using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductDetails: IEntity
    {
        [Key]
        public int ProductDetailsId { get; set; }
        public int ProductId { get; set; }
        public string ProductSize { get; set; }
        public bool Status { get; set; }
    }
}
