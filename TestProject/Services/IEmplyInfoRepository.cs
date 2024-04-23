using TestProject.Entities;

namespace TestProject.Services {
    public interface IEmplyInfoRepository {

        Task<Employ?> GetEmployAsync(int id);

        Task AddEmployAsync(Employ employ);

        Task<bool> SaveChangesAsync();
        Task<(IEnumerable<Employ>,PaginationMetadata)> GetEmploysAsync(string? nameFilter, string? searchQuery,
            int pageNumber, int pageSize);
    }
}
