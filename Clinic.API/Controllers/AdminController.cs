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
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepo _adminRepo;
        public AdminController(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }
        [HttpPatch("Account/Activate")]
        public IActionResult ActivateAccount(AccountStatus account)
        {
            return StatusCode((int)_adminRepo.ActivateAccount(account));
        }
        [HttpPatch("Account/Deactivate")]
        public IActionResult DeactivateAccount(AccountStatus account)
        {
            return StatusCode((int)_adminRepo.DeactivateAccount(account));
        }
        [HttpPatch("Account/Banned")]
        public IActionResult BanAccount(AccountStatus account)
        {
            return StatusCode((int)_adminRepo.BanAccount(account));
        }
        [HttpPatch("Account/RequestConfirm")]
        public IActionResult ConfrimAccount(AccountStatus account)
        {
            return StatusCode((int)_adminRepo.ConfirmationRequest(account));
        }
        [HttpPost("Account/RequestKey")]
        public IActionResult RequestKey(AccountStatus account)
        {
            return Ok(_adminRepo.GetKeypass(account))   ;
        }
    }
}
