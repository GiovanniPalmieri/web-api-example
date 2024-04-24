using AutoMapper;

namespace TestProject.Profiles {
    public class EmployProfile : Profile {
        public EmployProfile() {
            CreateMap<Entities.Employee, Models.EmployDto>();
            CreateMap<Models.EmployForCreationDto, Entities.Employee>();
        }
    }
}
