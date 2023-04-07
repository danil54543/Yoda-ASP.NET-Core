using Yoda.Data;
using Yoda.Models.Entity;
using Yoda.Services.Interface;

namespace Yoda.Services.Implementation
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext db;

        public NoteRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        /// <summary>
        /// Get all note from database.
        /// </summary>
        public IQueryable<Note> GetAll()
        {
            return db.Notes;
        }

        /// <summary>
        /// Delete note from database.
        /// </summary>
        /// <param name="entity">Note.</param>
        public async Task Delete(Note entity)
        {
            db.Notes.Remove(entity);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Adding note to database.
        /// </summary>
        /// <param name="entity">Note.</param>
        public async Task Create(Note entity)
        {
            await db.Notes.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Updating note in database.
        /// </summary>
        /// <param name="entity">Note.</param>
        public async Task<Note> Update(Note entity)
        {
            db.Notes.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
