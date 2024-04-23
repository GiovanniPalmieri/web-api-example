using TestProject.Models;

namespace TestProject.Repositories {
    public class EmployRepo {
        private ICollection<EmployDto> repo = new List<EmployDto>() {
            new EmployDto(){ Name = "enzo", Description = "ladksjflaksdjf"}
        };

        public void Add(EmployDto employ) {
            repo.Add(employ);
        }

        public EmployDto GetEmploy(string name) {
            return repo.First(x => x.Name == name);
        }
    }
}
