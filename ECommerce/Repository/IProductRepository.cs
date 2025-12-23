using ECommerce.Models;

public interface IProductRepository
{
    Task<List<Product>> GetAllProducts();
    Task CreateProduct(Product product);
    Task<bool> DeleteProduct(int productid);
    Task<bool> Deleteallproducts();
    Task<Product> GetAProduct(int productid);
    Task<bool> EditProduct(Product product);
}
