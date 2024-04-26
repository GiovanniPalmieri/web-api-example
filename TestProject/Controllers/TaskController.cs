﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Entities;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers {
    [ApiController]
    [Route("employee/{employeeId}/project/{projectId}/task")]
    public class TaskController : Controller {
        private readonly IGiraRepository _giraRepository;
        private readonly IMapper _mapper;

        public TaskController(IGiraRepository giraRepository, IMapper mapper) {
            _giraRepository = giraRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetTask")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(int employeeId, int projectId) {
            if(! await _giraRepository.ProjectExistsAsync(projectId)) {
                return NotFound();
            }
            var tasks = await _giraRepository.GetTasksForProjectAsync(projectId);
            return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
        }

        [HttpPost]
        public async Task<ActionResult<TaskToReturnDto>> CreateTask(int employeeId, int projectId, TaskForCreationDto task) {
            var project = await _giraRepository.GetProjectAsync(projectId);
            if(project == null) return NotFound();

            var finalTask = _mapper.Map<TaskEntity>(task);
            finalTask.FromProject = project;


            await _giraRepository.AddTaskAsync(finalTask);
            await _giraRepository.SaveChangesAsync();
            var taskToReturn = _mapper.Map<TaskToReturnDto>(finalTask);
            return CreatedAtRoute("GetTask", new { projectId = taskToReturn.ProjectId, 
                taskId = taskToReturn.TaskId }, taskToReturn);
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(int employeeId, int projectId, int taskId) {
            if(! await _giraRepository.TaskExist(taskId)) {
                return NotFound();
            }

            await _giraRepository.DeleteTaskAsync(taskId);
            await _giraRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
