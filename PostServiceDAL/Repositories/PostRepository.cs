using MongoDB.Driver;
using PostServiceBusiness.Interfaces;
using PostServiceBusiness.Models;

namespace PostServiceDAL.Repositories;

public class PostRepository() : IPostRepository
{
    private readonly IMongoCollection<Post> _postCollection;

    public PostRepository(IMongoClient mongoClient) : this()
    {
        var database = mongoClient.GetDatabase("ossodb");
        _postCollection = database.GetCollection<Post>("posts");
    }

    public Task<List<Post>> GetAllAsync()
    {
        return _postCollection.Find(_ => true).ToListAsync();
    }
    
    public Task<Post> GetByIdAsync(Guid id)
    {
        return null;
    }

    public Task AddAsync(Post post)
    {
        return null;
    }

    public Task UpdateAsync(Post post)
    {
        return null;
    }

    public Task DeleteAsync(Guid id)
    {
        return null;
    }
}