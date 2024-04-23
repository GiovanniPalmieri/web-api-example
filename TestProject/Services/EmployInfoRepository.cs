using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TestProject.DbContexts;
using TestProject.Entities;

namespace TestProject.Services {
    public class EmployInfoRepository : IEmplyInfoRepository {
        private readonly EmployContext _employContext;

        public EmployInfoRepository(EmployContext employContext) {
            _employContext = employContext ?? throw new ArgumentNullException(nameof(employContext));
        }

        public async Task<Employ?> GetEmployAsync(int id) {
            return await _employContext.Employs.Where(employ => employ.Id == id).FirstOrDefaultAsync();
        }


        
        public async Task<(IEnumerable<Employ>, PaginationMetadata)> GetEmploysAsync(string? nameFilter, string? searchQuery,
            int pageNumber, int pageSize) {
            
            var collection = _employContext.Employs as IQueryable<Employ>;

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
                collection = collection.Where(x => x.Name.Contains(searchQuery)
                    || (x.Description != null && x.Description.Contains(searchQuery)));
            }

            var collectionToReturn = await collection
                .OrderBy(x => x.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            
            return (collectionToReturn, paginationMetadata);
        }

        public async Task<bool> EmployExist(int id) {
            return await _employContext.Employs.Where(employ => employ.Id == id).CountAsync() > 0;
        }

        public async Task AddEmployAsync(Employ employ) {
            if (await EmployExist(employ.Id)) {
                return;
            }
            _employContext.Employs.Add(employ);
        }

        public async Task<bool> SaveChangesAsync() {
            return await _employContext.SaveChangesAsync() >= 0;
        }
    }
}
