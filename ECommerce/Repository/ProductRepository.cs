using ECommerce.Data.Context;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class ProductRepository : IProductRepository
{
    private readonly ECommerceDBContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductRepository(ECommerceDBContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task CreateProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteProduct(int productid)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.PId == productid);

        if (product == null)
            return false;

        // Delete image from wwwroot/images
        if (!string.IsNullOrEmpty(product.PImage))
        {
            // product.PImage is like "/images/abc.png"
            var imagePath = Path.Combine(_env.WebRootPath, product.PImage.TrimStart('/'));

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        // Remove from database
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> Deleteallproducts()
    {
        List<Product> allproducts = await _context.Products.ToListAsync();
        if (allproducts == null) return false;

        foreach (var p in allproducts)
        {
            if (!string.IsNullOrEmpty(p.PImage)){
                var path = Path.Combine(_env.WebRootPath, p.PImage);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

            }
            _context.Products.Remove(p);
            await _context.SaveChangesAsync();
            
        }
        return true;
    }
    public async Task<Product> GetAProduct(int productid)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.PId == productid);
    }
    public async Task<bool> EditProduct(Product product)
    {
        var existing = await _context.Products.FindAsync(product.PId);
        if (existing == null) return false;

        //_context.Entry(existing).CurrentValues.SetValues(product);
        existing.PName = product.PName;
            existing.Price = product.Price;
            existing.Product_Desc = product.Product_Desc;

        await _context.SaveChangesAsync();
        return true;
    }



}
