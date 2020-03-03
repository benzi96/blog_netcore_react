using Blog.Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Route("identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]IdentityUserDto identityUserDto)
        {
            var user = new IdentityUser
            {
                UserName = identityUserDto.UserName,
                Email = identityUserDto.UserName
            };
            var result = await _userManager.CreateAsync(user, identityUserDto.Password);

            if (!result.Succeeded)
            {
                return new JsonResult(result.Errors);
            }

            var currentUser = await _userManager.FindByNameAsync(user.UserName);

            var roleresult = await _userManager.AddToRoleAsync(currentUser, "NormalUser");

            return Ok();
        }
    }
}