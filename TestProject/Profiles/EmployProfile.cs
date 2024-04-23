using AutoMapper;

namespace TestProject.Profiles {
    public class EmployProfile : Profile {
        public EmployProfile() {
            CreateMap<Entities.Employ, Models.EmployDto>();
            CreateMap<Models.EmployForCreationDto, Entities.Employ>();
        }
    }
}
