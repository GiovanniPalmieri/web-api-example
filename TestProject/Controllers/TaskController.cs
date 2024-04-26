using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers {
    [ApiController]
    [Route("api/project/{projectId}/task")]
    public class TaskController : Controller {
        private readonly IGiraRepository _giraRepository;
        private readonly IMapper _mapper;

        public TaskController(IGiraRepository giraRepository, IMapper mapper) {
            _giraRepository = giraRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(int projectId) {
            if(! await _giraRepository.ProjectExistsAsync(projectId)) {
                return NotFound();
            }
            var tasks = await _giraRepository.GetTasksForProjectAsync(projectId);
            return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
        }
    }
}
