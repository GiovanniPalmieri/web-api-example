namespace TestProject.Services {
    public class PaginationMetadata {
        public PaginationMetadata(int totalItemCount, int pageSize, int pageNumber) {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public int TotalItemCount { get; }
        public int PageSize { get; }
        public int PageNumber { get; }
    }
}
