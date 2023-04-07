using Yoda.Models.ViewModel;

namespace Yoda.Services.Interface
{
    public interface INoteService
    {
        Task<bool> Create(NoteViewModel model);
        Task<bool> Delete(long noteId);
        Task<bool> Update(NoteViewModel model);
        Task<bool> UpdateConfirm(long id);
        Task<bool> UpdateFavorite(long id);
        Task<List<NoteViewModel>> GetAll(string userId);
        Task<NoteViewModel> Get(long noteId);
    }
}
