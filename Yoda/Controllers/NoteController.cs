using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yoda.Models.Entity;
using Yoda.Models.ViewModel;
using Yoda.Services.Interface;

namespace Yoda.Controllers
{
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        private readonly UserManager<User> userManager;

        public NoteController(INoteService noteService, UserManager<User> userManager)
        {
            _noteService = noteService;
            this.userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> NotesDateCreated()
        {
            var notes = await _noteService.GetAll(userManager.GetUserId(User));
            var result = notes.OrderByDescending(d => d.DateCreated).ToList();
            return View("Notes", result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> NotesDateEdit()
        {
            var notes = await _noteService.GetAll(userManager.GetUserId(User));
            var result = notes.OrderByDescending(d => d.DateEdit).ToList();
            return View("Notes", result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> NotesName()
        {
            var notes = await _noteService.GetAll(userManager.GetUserId(User));
            var result = notes.OrderBy(d => d.Title).ToList();
            return View("Notes", result);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var model = new NoteViewModel()
            {
                Title = "",
                Item = ""
            };
            return View("NewNote",model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(NoteViewModel model)
        {
            model.UserId = userManager.GetUserId(User);
            var result = await _noteService.Create(model);
            if (result == true)
            {
                return RedirectToAction("NotesDateCreated", "Note");
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            var responce = await _noteService.Delete(id);
            if (responce == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return RedirectToAction("NotesDateCreated", "Note");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(long id)
        {
            var response = await _noteService.Get(id);
            return View("UpdateNote", response);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(NoteViewModel model)
        {
            var responce = await _noteService.Update(model);
            return RedirectToAction("NotesDateCreated", "Note");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Confirm(long id)
        {

            var responce = await _noteService.UpdateConfirm(id);
            if (responce == true)
            {
                return RedirectToAction("NotesDateCreated", "Note");
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Favorite(long id)
        {

            var responce = await _noteService.UpdateFavorite(id);
            if (responce == true)
            {
                return RedirectToAction("NotesDateCreated", "Note");
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
