using Clinic.Core.DTO;
using Clinic.Core.Repos;
using Clinic.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers
{
    [Server_NotFound_Exceptions]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _UserRepo;
        public UserController(IUserRepo UserRepo)
        {
            _UserRepo = UserRepo;
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginDTO log)
        {
            return Ok(_UserRepo.Login(log));
        }
        [HttpPatch("changepassword")]
        public IActionResult changePassword(ChangePassword change)
        {
            return StatusCode((int)_UserRepo.changePassword(change));
        }
        [HttpPatch("changepfp")]
        public IActionResult changePFP([FromForm]AddPic profilePic)
        {
            return StatusCode((int)_UserRepo.changeProfilePic(profilePic));
        }
        [HttpDelete("deletepfp")]
        public IActionResult deletePFP(UserInfo user)
        {
            return StatusCode((int)_UserRepo.DeleteProfilePic(user));
        }
    }
}
