using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookstore.Domain.Books
{
    public  interface IBookRepository
    {
        Task<Book> GetAsync(int id);

        Task<IPaginatedList<Book>> ListAsync(BookFilters filters, int pageIndex, int pageSize);

        Task<IPaginatedList<Book>> ListAsync(string searchString, string sortBy, int pageIndex, int pageSize);

        Task<IEnumerable<Book>> ListBestSellingBooksAsync(int count);

        Task AddAsync(Book book);

        Task UpdateAsync(Book book);

        Task SaveChangesAsync();

        Task<BookStatistics> GetStatisticsAsync();
    }
}
