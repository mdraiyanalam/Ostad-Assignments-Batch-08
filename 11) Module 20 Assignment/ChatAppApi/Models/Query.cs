using ChatAppApi.Models;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace ChatAppApi.GraphQL
{
    public class Query
    {
        public async Task<User?> getUserById(
            [Service] AppDbContext db,
            int id)
        {
            return await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public IQueryable<User> GetUsers([Service] AppDbContext db)
        {
            return db.Users.AsQueryable();
        }
    }
}