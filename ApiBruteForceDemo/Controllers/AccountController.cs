using System.Security.Claims;
using ApiBruteForceDemo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBruteForceDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        string message;

        if (loginModel is { Username: "admin", Password: "ramirez" } or { Username: "jeremy", Password: "07061955" })
        {
            if (loginModel.Username == "admin")
            {
                message = "Congratulations! https://youtu.be/NVIbCvfkO3E";
            }
            else
            {
                message = "Congratulations! https://youtu.be/XAYhNHhxN0A";
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, loginModel.Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(claimsPrincipal, authenticationProperties);

            return Ok(new { Message = message });
        }

        if (loginModel.Username is not "admin" and not "jeremy")
        {
            message = "Invalid username";
            return Unauthorized(new { Message = message });
        }

        message = "Invalid password!";
        return Unauthorized(new { Message = message });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return NoContent();
    }
}