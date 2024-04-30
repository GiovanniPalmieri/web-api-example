﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Entities;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers {
    [ApiController]
    [Route("employee/{employeeId}/project")]
    public class ProjectController : ControllerBase {

        private readonly IGiraRepository _giraRepository;
        private readonly IMapper _mapper;

        public ProjectController(IGiraRepository giraRepository, IMapper mapper) {
            _giraRepository = giraRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects(int employeeId) {
            var projects = await _giraRepository.GetProjectsAsync();
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpGet("{projectId}", Name ="GetProject")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAssignedProjects(int employeeId,int projectId) {
            var projects = await _giraRepository.GetAssignedProjectsAsync(projectId);
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject(int employeeId, int projectId) {
            if(!await _giraRepository.ProjectExistsAsync(projectId)) {
                return NotFound();
            }
            
            await _giraRepository.DeleteProjectAsync(projectId);

            await _giraRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProjectToReturnDto>> CreateProject(int projectId, ProjectForCreationDto project) {
            var manager = await _giraRepository.GetManagerAsync(project.managerId);
            if(manager == null) {
                return NotFound(); 
            }

            var finalProject = _mapper.Map<Project>(project);
            finalProject.Manager = manager;

            await _giraRepository.AddProjectAsync(finalProject);

            await _giraRepository.SaveChangesAsync();

            var projectToReturn = _mapper.Map<ProjectToReturnDto>(finalProject);

            return CreatedAtRoute("GetProject", new { id = projectToReturn.Id}, projectToReturn);
        }
    }
}
