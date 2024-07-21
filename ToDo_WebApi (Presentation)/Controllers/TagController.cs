using Application.DTOs;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ToDo_WebApi__Presentation_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            if (tags == null)
            {
                return NotFound("No tags found.");
            }

            var tagDtos = _mapper.Map<IEnumerable<TagDto>>(tags);
            return Ok(tagDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound($"Tag with ID {id} not found.");
            }

            var tagDto = _mapper.Map<TagDto>(tag);
            return Ok(tagDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] TagCreateDto tagCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tag = _mapper.Map<Tag>(tagCreateDto);
            await _tagService.AddTagAsync(tag);
            return Ok("Tag Created Successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] TagDto tagUpdateDto)
        {
            if (id != tagUpdateDto.Id)
            {
                return BadRequest("Tag ID in the URL does not match the ID in the request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTag = await _tagService.GetTagByIdAsync(id);
            if (existingTag == null)
            {
                return NotFound($"Tag with ID {id} not found.");
            }

            var tag = _mapper.Map<Tag>(tagUpdateDto);
            await _tagService.UpdateTagAsync(tag);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var existingTag = await _tagService.GetTagByIdAsync(id);
            if (existingTag == null)
            {
                return NotFound($"Tag with ID {id} not found.");
            }

            await _tagService.DeleteTagAsync(id);
            return NoContent();
        }
    }
}
