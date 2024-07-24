using Application.ResponseDtos;
using Application.DTOs;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ToDo_WebApi__Presentation_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("token-decode")]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var username = User.FindFirstValue(ClaimTypes.Name);

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Username = username
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null)
            {
                return NotFound("No users found.");
            }

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserByUsernameAsync(userCreateDto.Username);
            if (existingUser != null)
            {
                return Conflict(new GenResDto { Message = "Username already exists. Please try another one." });
            }

            var user = _mapper.Map<User>(userCreateDto);
            await _userService.AddUserAsync(user);
            return Ok(new GenResDto{ Message = "User Created Successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (id != userUpdateDto.Id)
            {
                return BadRequest("User ID in the URL does not match the ID in the request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var user = _mapper.Map<User>(userUpdateDto);
            await _userService.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
