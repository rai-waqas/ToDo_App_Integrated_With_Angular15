namespace Application.DTOs
{
    public class TaskCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; }
        public bool Completed { get; set; }
        public int UserId { get; set; }
        public string? Tags { get; set; }
    }
}