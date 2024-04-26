namespace TestProject.Models {
    public class EmployDto {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<int> AssignedProjectsIds { get; set; } = new List<int>();
    }
}
