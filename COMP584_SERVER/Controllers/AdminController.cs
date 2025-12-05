using COMP584_SERVER.Data;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using COMP584_SERVER.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Globalization;
using WorldModel;
using System.IdentityModel.Tokens.Jwt;

namespace COMP584_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldModelUser> userManager, JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            WorldModelUser? worldUser = await userManager.FindByNameAsync(loginRequest.Username);

            /*if (worldUser is null || 
                !await userManager.CheckPasswordAsync(worldUser, loginRequest.Password))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }*/

            if (worldUser == null)
            {
                return Unauthorized("Invalid username");
            }

            bool loginStatus = await userManager.CheckPasswordAsync(worldUser, loginRequest.Password);
            if(!loginStatus)
            {
                return Unauthorized("Invalid password");
            }
            JwtSecurityToken jwtToken = await jwtHandler.GenerateTokenAsync(worldUser);
            string stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Token = stringToken
            });
        }
    }
}
