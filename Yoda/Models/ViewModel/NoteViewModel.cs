namespace Yoda.Models.ViewModel
{
    public class NoteViewModel
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string Item { get; set; }
        public string? Marker { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdit { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsFavorite { get; set; }
        public string UserId { get; set; }
    }
}
