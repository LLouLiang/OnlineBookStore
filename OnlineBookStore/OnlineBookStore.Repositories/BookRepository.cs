﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Repositories.Data;
using OnlineBookStore.Services.DB;

namespace OnlineBookStore.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly OnlineBookStoreDbContext _context;
        private readonly ISqlQueryContext _sqlQueryContext;
        public BookRepository(OnlineBookStoreDbContext context, ISqlQueryContext sqlQueryContext) : base(context)
        {
            _context = context;
            _sqlQueryContext = sqlQueryContext;
        }

        public async Task<IEnumerable<Book>> GetBooksByIds(List<long> ids)
        {
            var query = _sqlQueryContext.GetBooksByIds();

            var param = ids.Select((id, index) => new SqliteParameter($"@id{index}", id)).ToArray();

            var inClause = string.Join(", ", param.Select(p => p.ParameterName));

            query = query.Replace("@Ids", inClause);

            return await _context.Books.FromSqlRaw(query, param).ToListAsync();
        }
    }
}
