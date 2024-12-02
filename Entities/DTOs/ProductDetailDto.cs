public class ProductDetailDto
{
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int ProductStocksId { get; set; }
    public int SuperCategoryId { get; set; }
    public int CategoryId { get; set; }
    public string ProductName { get; set; }
    public string SuperCategoryName { get; set; }
    public string CategoryName { get; set; }
    public string ProductDescription { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
    public string ColorName { get; set; }
    public string ProductSize { get; set; }
    public List<string> Images { get; set; }
    public bool Status { get; set; }
}
