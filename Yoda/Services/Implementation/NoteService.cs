using Microsoft.EntityFrameworkCore;
using Yoda.Models.Entity;
using Yoda.Models.ViewModel;
using Yoda.Services.Interface;

namespace Yoda.Services.Implementation
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;
        }

        public async Task<bool> Create(NoteViewModel model)
        {
            try
            {
                var note = new Note()
                {
                    Item = model.Item,
                    Title = model.Title ?? "My note",
                    Marker = model.Marker,
                    DateCreated = DateTime.Now,
                    DateEdit = DateTime.Now,
                    YodaUserId = model.UserId,
                    IsConfirm = false,
                    IsFavorite = false
                };
                await noteRepository.Create(note);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(long noteId)
        {
            try
            {
                var note = await noteRepository.GetAll().FirstOrDefaultAsync(x => x.Id == noteId);
                if (note == null)
                {
                    return false;
                }
                await noteRepository.Delete(note);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<NoteViewModel> Get(long noteId)
        {
            var note = await noteRepository.GetAll().FirstOrDefaultAsync(x => x.Id == noteId);
            var noteViewModel = new NoteViewModel()
            {
                Id = note.Id,
                Title = note.Title,
                Item = note.Item,
                DateCreated = note.DateCreated,
                DateEdit = note.DateEdit,
                Marker = note.Marker,
                IsConfirm = note.IsConfirm,
                IsFavorite = note.IsFavorite
            };
            return noteViewModel;
        }

        public async Task<List<NoteViewModel>> GetAll(string userId)
        {
            try
            {
                var notes = await noteRepository.GetAll().Where(x => x.YodaUserId == userId).ToListAsync();
                var noteViewModels = from t in notes
                                     select new NoteViewModel()
                                     {
                                         Id = t.Id,
                                         Title = t.Title,
                                         Item = t.Item,
                                         DateCreated = t.DateCreated,
                                         DateEdit = t.DateEdit,
                                         Marker = t.Marker,
                                         IsConfirm = t.IsConfirm,
                                         IsFavorite = t.IsFavorite
                                     };
                return noteViewModels.ToList();
            }
            catch (Exception ex)
            {
                return new List<NoteViewModel>();
            }

        }

        public async Task<bool> Update(NoteViewModel model)
        {
            var note = await noteRepository.GetAll().FirstOrDefaultAsync(x => x.Id == model.Id);
            note.Marker = model.Marker;
            note.Item = model.Item;
            note.Title = model.Title;
            note.IsConfirm = model.IsConfirm;
            note.IsFavorite = model.IsFavorite;
            note.DateEdit = DateTime.Now;
            await noteRepository.Update(note);
            return true;
        }
        public async Task<bool> UpdateConfirm(long id)
        {
            var note = await noteRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (note == null)
            {
                return false;
            }
            switch (note.IsConfirm)
            {
                case true:
                    note.IsConfirm = false;
                    break;
                case false:
                    note.IsConfirm = true;
                    break;
            }
            await noteRepository.Update(note);
            return true;
        }

        public async Task<bool> UpdateFavorite(long id)
        {
            var note = await noteRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (note == null)
            {
                return false;
            }
            switch (note.IsFavorite)
            {
                case true:
                    note.IsFavorite = false;
                    break;
                case false:
                    note.IsFavorite = true;
                    break;
            }
            await noteRepository.Update(note);
            return true;
        }
    }
}
