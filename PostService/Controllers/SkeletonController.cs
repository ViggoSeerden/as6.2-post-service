using Microsoft.AspNetCore.Mvc;

namespace PostService.Controllers;

[ApiController]
[Route("posts")]
public class SkeletonController : ControllerBase
{
    [HttpGet("skeleton")]
    public string GetSkeletonMessage()
    {
        return "This is the Post Service Skeleton endpoint.";
    }
}