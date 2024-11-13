using Core.Entities;
using Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProductImages : IEntity
{
    [Key]  
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }  

    public int ProductId { get; set; }  // Foreign key
    public string ProductImageUrl { get; set; }

}
