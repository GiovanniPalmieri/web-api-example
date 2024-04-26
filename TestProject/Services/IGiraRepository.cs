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
        Task<bool> ProjectExistsAsync(int projectId);
        Task DeleteProjectAsync(int projectId);
        Task<bool> ManagerExists(int managerId);
        Task AddProjectAsync(Project project);
        Task<Manager?> GetManagerAsync(int managerId);
        Task<IEnumerable<TaskEntity>> GetTasksForProjectAsync(int projectId);
        Task<Project?> GetProjectAsync(int projectId);
        Task AddTaskAsync(TaskEntity task);
    }
}
