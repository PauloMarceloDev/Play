using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Play.Identity.Service.Entities;

using Swashbuckle.AspNetCore.Annotations;

using static Duende.IdentityServer.IdentityServerConstants;

namespace Play.Identity.Service.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
[Route("users")]
[Authorize(Policy = LocalApi.PolicyName)]
public sealed class UsersController(UserManager<ApplicationUser> userManager)
    : ControllerBase
{
    [SwaggerOperation(Summary = "Fetches the list of user.")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> Get() =>
        Ok(userManager.Users.ToList().Select(u => u.AsDto()));

    [SwaggerOperation(Summary = "Fetch the user by id.")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto?>> GetByIdAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        return user is null ? NotFound() : user.AsDto();
    }


    [SwaggerOperation(Summary = "Update the user by id.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserDto request)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return NotFound();
        }

        user.Email = request.Email;
        user.UserName = request.Email;
        user.Gil = request.Gil;

        await userManager.UpdateAsync(user);

        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete the user by id.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return NotFound();
        }

        await userManager.DeleteAsync(user);

        return NoContent();
    }
}
