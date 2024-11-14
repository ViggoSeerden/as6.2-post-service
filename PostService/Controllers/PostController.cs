using Microsoft.AspNetCore.Mvc;

namespace PostService.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController(PostServiceBusiness.Services.PostService postService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await postService.GetAllPostsAsync();
        return Ok(posts);
    }
}