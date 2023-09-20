using Microsoft.AspNetCore.Mvc;
using FiveInARow.Services.FiveInARow;
using AutoMapper;
using FiveInARow.Dto;
using FiveInARow.Models;

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
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        { 
            if (userCreate == null)
                return BadRequest(ModelState);
            
            var user = _userService.GetUsers()
                .FirstOrDefault(u => u.Email.ToLower() == userCreate.Email.TrimEnd().ToLower());

            if (user != null)
            {
                ModelState.AddModelError("Email", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);
            userMap.CreatedAt = DateTime.Now;

            if (!_userService.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating user");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int id)
        {
            if (!_userService.UserExists(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userService.GetUser(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userService.GetUsers());
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(users);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpsertUser(int userId, [FromBody] UserDto userUpsert)
        {
            if (userUpsert == null)
                return BadRequest(ModelState);

            if (userId != userUpsert.Id)
                return BadRequest(ModelState);

            if (!_userService.UserExists(userId))
                return NotFound();
            
            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpsert);

            if (!_userService.UpsertUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}