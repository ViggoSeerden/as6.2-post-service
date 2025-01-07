using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostServiceBusiness.Models;

namespace PostService.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController(PostServiceBusiness.Services.PostService postService) : ControllerBase
{
    [HttpGet("")]
    [Authorize("read:posts")]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await postService.GetAllPostsAsync();
        return Ok(posts);
    }
    
    [HttpGet("{id}")]
    [Authorize("read:post")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var post = await postService.GetPostByIdAsync(id);
        return Ok(post);
    }
        
    [HttpPost("")]
    [Authorize("write:add_post")]
    public async Task<IActionResult> AddPost([FromBody] Post post)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await postService.AddPostAsync(post);
        return NoContent();
    }
        
    [HttpPut("{id}")]
    [Authorize("write:update_post")]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] Post updatedPost)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var post = await postService.GetPostByIdAsync(id);
        if (post == null)
            return NotFound();
            
        await postService.UpdatePostAsync(id, updatedPost);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    [Authorize("write:delete_post")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        await postService.DeletePostAsync(id);
        return Ok();
    }
}