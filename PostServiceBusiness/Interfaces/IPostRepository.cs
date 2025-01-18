using PostServiceBusiness.Models;

namespace PostServiceBusiness.Interfaces;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync();
    Task<Post> GetByIdAsync(Guid id);
    Task AddAsync(Post post);
    Task UpdateAsync(Guid id, Post post);
    Task DeleteAsync(Guid id);
}