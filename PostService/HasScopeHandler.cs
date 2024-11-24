using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace PostService;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        // Find the 'permissions' claim
        var permissionsClaim = context.User.FindAll(c => c.Type == "permissions" && c.Issuer == requirement.Issuer);

        foreach (var claim in permissionsClaim)
        {
            try
            {
                // Check if the required scope matches any permission
                if (claim.Value == requirement.Scope)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing permissions: {ex.Message}");
            }
        }

        return Task.CompletedTask;
    }
}