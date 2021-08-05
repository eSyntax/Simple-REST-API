using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_API.Models
{
    public class ToDoList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToDoId { get; set; }

        // ErrorMessage in string only enforces when data is entered
        [StringLength(100, MinimumLength = 2, ErrorMessage = "* Description must be between 2 and 100 character in length.")]
        public string Description { get; set; }

        public TaskStatus TaskCompleted { get; set; }
    }
}
