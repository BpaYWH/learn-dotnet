using Microsoft.AspNetCore.Mvc;
using FiveInARow.Contracts.FiveInARow;
using FiveInARow.Services.FiveInARow;
using AutoMapper;
using FiveInARow.Dto;

namespace FiveInARow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest request)
        { 
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id)
        {
            if (!_userService.UserExists(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userService.GetUser(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userService.GetUsers());
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpsertUser(int id, UpsertUserRequest request)
        {

            // _userService.UpsertUser(user);

            // TODO: return 201 if a new user was created
            return NoContent();
        }
    }
}