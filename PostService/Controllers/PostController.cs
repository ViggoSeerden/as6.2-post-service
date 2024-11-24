using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}