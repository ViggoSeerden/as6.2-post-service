using PostServiceBusiness.Interfaces;
using PostServiceBusiness.Models;

namespace PostServiceBusiness.Services;

public class PostService(IPostRepository postRepository)
{
    private readonly IPostRepository _postRepository = postRepository;
    
    public Task<List<Post>> GetAllPostsAsync() => _postRepository.GetAllAsync();
    public Task<Post> GetPostByIdAsync(Guid id) => _postRepository.GetByIdAsync(id);
    public Task AddPostAsync(Post post) => _postRepository.AddAsync(post);
    public Task UpdatePostAsync(Post post) => _postRepository.UpdateAsync(post);
    public Task DeletePostAsync(Guid id) => _postRepository.DeleteAsync(id);
}