using Core.YardSale.Contracts;
using Core.YardSale.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.YardSale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("GetUserByUsername")]
        public IActionResult GetUserByUsername(string username)
        {
            var result = _userRepository.GetUserByUsername(username);
            return Ok(result);
        }

        [HttpPost]
        [Route("RegisterNewUser")]
        public IActionResult RegisterNewUser([FromBody]User user)
        {
            var result = _userRepository.RegisterNewUser(user);
            return Ok(result);
        }
    }
}

