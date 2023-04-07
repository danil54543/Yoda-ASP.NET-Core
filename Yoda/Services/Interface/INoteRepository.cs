using Yoda.Models.Entity;

namespace Yoda.Services.Interface
{
    public interface INoteRepository
    {
        Task Create(Note entity);
        Task Delete(Note entity);
        Task<Note> Update(Note entity);
        IQueryable<Note> GetAll();
    }
}
