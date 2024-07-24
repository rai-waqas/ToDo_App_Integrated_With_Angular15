using Application.DTOs;
using AutoMapper;
using Core.AuthModels;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = Core.Models.Task;

namespace ToDo_WebApi__Presentation_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public TaskController(IAuthService authService, ITaskService taskService, IMapper mapper)
        {
            _authService = authService;
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var authResponse = await _authService.AuthenticateAsync(request);
                return Ok(authResponse);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByUserId(userId);
                if (tasks == null)
                {
                    return NotFound("No tasks found.");
                }
                return Ok(tasks);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching tasks.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            if (tasks == null)
            {
                return NotFound("No tasks found.");
            }

            var taskDtos = _mapper.Map<IEnumerable<TaskDto>>(tasks);
            return Ok(taskDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            var taskDto = _mapper.Map<TaskDto>(task);
            return Ok(taskDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto taskCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _mapper.Map<Task>(taskCreateDto);
            await _taskService.AddTaskAsync(task);
            return Ok(new { message = "Task Created Successfully", taskId = task.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto taskUpdateDto)
        {
            if (id != taskUpdateDto.Id)
            {
                return BadRequest("Task ID in the URL does not match the ID in the request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            var task = _mapper.Map<Task>(taskUpdateDto);
            await _taskService.UpdateTaskAsync(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}