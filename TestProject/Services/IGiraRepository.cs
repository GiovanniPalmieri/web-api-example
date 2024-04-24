using TestProject.Entities;

namespace TestProject.Services {
    public interface IGiraRepository {

        Task<Employee?> GetEmployAsync(int id);

        Task AddEmployAsync(Employee employ);

        Task<bool> SaveChangesAsync();
        Task<(IEnumerable<Employee>,PaginationMetadata)> GetEmploysAsync(string? nameFilter, string? searchQuery,
            int pageNumber, int pageSize);

        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<IEnumerable<Project>> GetAssignedProjectsAsync(int employeeId);
    }
}
