using WebLyThuyet.Models;

namespace WebLyThuyet.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> SearchAsync(string query);
        Task<int> CountAsync();
        Task<List<Product>> GetPagedAsync(int page, int pageSize);
    }
}
