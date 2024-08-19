using BlossomApi.Models;

namespace BlossomApi.Repositories
{
    public interface IShownProductRepository
    {
        IQueryable<Product> GetProducts(bool isShown = true);
    }
}
