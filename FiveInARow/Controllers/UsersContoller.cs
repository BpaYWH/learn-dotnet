using Microsoft.AspNetCore.Mvc;
using FiveInARow.Contracts;

using FiveInARow.Contracts.FiveInARow;

namespace FiveInARow.Controllers
{
    [ApiController]
    public class UsersContoller : ControllerBase
    {
        [HttpPost("/users")]
        public IActionResult CreateUser(CreateUserRequest request)
        {
            return Ok(request);
        }
    }
}