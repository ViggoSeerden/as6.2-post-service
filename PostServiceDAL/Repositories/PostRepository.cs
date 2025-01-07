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
        var filter = Builders<Post>.Filter.Eq(post => post.Id, id);
        return _postCollection.Find(filter).FirstOrDefaultAsync();
    }

    public Task AddAsync(Post post)
    {
        return _postCollection.InsertOneAsync(post);
    }

    public Task UpdateAsync(Guid id, Post updatedPost)
    {
        var filter = Builders<Post>.Filter.Eq(post => post.Id, id);
        var update = Builders<Post>.Update
            .Set(post => post.City, updatedPost.City)
            .Set(post => post.Street, updatedPost.Street)
            .Set(post => post.Description, updatedPost.Description);
        
        return _postCollection.UpdateOneAsync(filter, update);
    }

    public Task DeleteAsync(Guid id)
    {
        var filter = Builders<Post>.Filter.Eq(post => post.Id, id);
        return _postCollection.DeleteOneAsync(filter);
    }
}