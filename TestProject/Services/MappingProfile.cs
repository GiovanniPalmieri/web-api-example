using AutoMapper;
using TestProject.Entities;
using TestProject.Models;

namespace TestProject.Services {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.TasksIds,
                act => act.MapFrom(src => src.Tasks.Select(t => t.Id)));
            CreateMap<ProjectForCreationDto, Project>();
            CreateMap<Project, ProjectToReturnDto>();
            CreateMap<TaskEntity, TaskDto>()
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.FromProject.Id));
            CreateMap<TaskForCreationDto, TaskEntity>();
            CreateMap<TaskEntity, TaskToReturnDto>()
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.FromProject.Id));
        }
    }
}
