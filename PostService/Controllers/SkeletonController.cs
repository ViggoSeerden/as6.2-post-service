using Microsoft.AspNetCore.Mvc;

namespace PostService.Controllers;

[ApiController]
[Route("api/skeleton")]
public class SkeletonController : ControllerBase
{
    [HttpGet("")]
    public string GetSkeletonMessage()
    {
        return "This is the Post Service Skeleton endpoint.";
    }
}