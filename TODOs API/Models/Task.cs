using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TODOs_API.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(110)]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Deadline { get; set; } = null;
        public bool IsCompleted { get; set; }
    }
}
