using Core.Entities;
using Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProductImages : IEntity
{
    [Key]  // Bu özellik birincil anahtar olarak tanımlanıyor
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }  // Her varlık için birincil anahtar özelliği ekleyin

    public int ProductId { get; set; }  // Foreign key
    public string ProductImageUrl { get; set; }

}
