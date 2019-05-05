using System;
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
        
        public UserController(IUserService userService, IUserRepository userrepo)
        {
            this.userService = userService;
            this.userRepo = userrepo;
            
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
    }
}