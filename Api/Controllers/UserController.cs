using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using HomeCTRL.Backend.Api.DTO;
using HomeCTRL.Backend.Core.Exceptions;
using HomeCTRL.Backend.Features.Users;
using HomeCTRL.Backend.Features.Users.Repository;
using HomeCTRL.Backend.Features.Users.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCTRL.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserRepository userRepo;
        private readonly ClaimsPrincipal currentPrincipal;
        
        public UserController(
            IUserService userService, 
            IUserRepository userrepo,
            IPrincipal currentPrincipal)
        {
            this.userService = userService;
            this.userRepo = userrepo;
            this.currentPrincipal = currentPrincipal as ClaimsPrincipal;
        }

        // POST /api/user/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] LoginInfoDTO loginInfo)
        {
            try 
            {
                var token = await this.userService.AuthenticateUser(loginInfo);

                if (token == null) 
                    throw new InputException("INVALID_CREDENTIALS");

                return Ok(token);
            }
            catch (InputException e) 
            {
                return BadRequest(new { message = e.Message });
            }
        }

        // GET /api/user/me
        [HttpGet("me")]
        public Task<User> Me()
        {
            var currentUserId = this.currentPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;

            return this.userService.Get(new Guid(currentUserId));
        }
    }
}