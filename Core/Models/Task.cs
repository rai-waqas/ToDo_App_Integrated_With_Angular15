using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Task
    {
        public Task()
        {
            
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = null!;
        public bool Completed { get; set; }
        public string? Tags { get; set; }
        public virtual User? User { get; set; }
    }
}
