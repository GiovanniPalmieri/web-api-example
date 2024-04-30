using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TestProject.DbContexts;
using TestProject.Entities;
using TestProject.Models;

namespace TestProject.Services {
    public class GiraRepository : IGiraRepository {
        private readonly GiraContext _giraContext;

        public GiraRepository(GiraContext employContext) {
            _giraContext = employContext ?? 
                throw new ArgumentNullException(nameof(employContext));
        }

        public async Task<Employee?> GetEmployAsync(int id) {
            return await _giraContext.Employs
                .Where(employ => employ.Id == id).FirstOrDefaultAsync();
        }


        
        public async Task<(IEnumerable<Employee>, PaginationMetadata)> 
            GetEmploysAsync(string? nameFilter, string? searchQuery,
            int pageNumber, int pageSize) {
            
            var collection = _giraContext.Employs as IQueryable<Employee>;

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);
            
            if (string.IsNullOrEmpty(nameFilter) && 
                string.IsNullOrWhiteSpace(searchQuery)) {

                var toReturn = await collection
                    .OrderBy(x => x.Name)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync();

                return (toReturn, paginationMetadata);
            }
            

            if(!string.IsNullOrEmpty(nameFilter)) {
                nameFilter = nameFilter.Trim();
                collection = collection.Where(x => x.Name == nameFilter);
            }

            if(!string.IsNullOrWhiteSpace(searchQuery)) {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.Name.Contains(searchQuery));
            }

            var collectionToReturn = await collection
                .OrderBy(x => x.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            
            return (collectionToReturn, paginationMetadata);
        }

        public async Task<bool> EmployExist(int id) {
            return await _giraContext.Employs.Where(employ => employ.Id == id).CountAsync() > 0;
        }

        public async Task AddEmployAsync(Employee employ) {
            if (await EmployExist(employ.Id)) {
                return;
            }
            _giraContext.Employs.Add(employ);
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync() {
            return await _giraContext.Projects.Include(p => p.Tasks).ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksForProjectAsync(int projectId) {
            return await _giraContext.Tasks.Where(t => t.FromProject.Id == projectId).ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAssignedProjectsAsync(int employeeId) {
            var collection = _giraContext.Projects as IQueryable<Project>;
            var filteredCollection = collection.Where(p => p.Employees.Select(e => e.Id).Contains(employeeId));
            return await filteredCollection.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync() {
            return await _giraContext.SaveChangesAsync() >= 0;
        }

        public async Task<bool> ProjectExistsAsync(int projectId) {
            return await _giraContext.Projects.AnyAsync(p => p.Id == projectId);  
        }

        public async Task DeleteProjectAsync(int projectId) {
            var project = await _giraContext.Projects.FindAsync(projectId);
            if (project != null) {
                _giraContext.Projects.Remove(project);
            }
        }

        public async Task<bool> ManagerExists(int managerId) {
            return await _giraContext.Managers.AnyAsync(p => p.Id == managerId);
        }

        public async Task AddProjectAsync(Project project) {
            if (await ProjectExistsAsync(project.Id)) {
                return;
            }
            await _giraContext.Projects.AddAsync(project);
        }

        public async Task<Project?> GetProjectAsync(int projectId) {
            return await _giraContext.Projects.FindAsync(projectId);
        }

        public async Task AddTaskAsync(TaskEntity task) {
            if (await TaskExist(task.Id)) {
                return;
            }

            await _giraContext.Tasks.AddAsync(task);
        }

        public async Task<bool> TaskExist(int id) {
           return await _giraContext.Tasks.AnyAsync(t => t.Id == id);
        }

        public async Task DeleteTaskAsync(int taskId) { 
            var task = await _giraContext.Tasks.FindAsync(taskId);
            if (task == null) return;

            _giraContext.Tasks.Remove(task);
        }

        public async Task<Manager?> GetManagerAsync(int managerId) {
            return await _giraContext.Managers.FindAsync(managerId);
        }
    }
}
