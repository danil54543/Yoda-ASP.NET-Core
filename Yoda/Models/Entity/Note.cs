namespace Yoda.Models.Entity
{
    public class Note
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Item { get; set; }
        public string? Marker { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdit { get; set; }
        public string YodaUserId { get; set; }
        public User User { get; set; }
    }
}
