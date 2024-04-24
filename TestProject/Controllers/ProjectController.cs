using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers {
    [ApiController]
    [Route("api/project")]
    public class ProjectController : ControllerBase {

        private readonly IGiraRepository _giraRepository;
        private readonly IMapper _mapper;

        public ProjectController(IGiraRepository giraRepository, IMapper mapper) {
            _giraRepository = giraRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects() {
            var projects = await _giraRepository.GetProjectsAsync();
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAssignedProjects(int id) {
            var projects = await _giraRepository.GetAssignedProjectsAsync(id);
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

    }
}
