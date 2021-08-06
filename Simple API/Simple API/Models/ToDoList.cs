using Simple_API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_API.Models
{
    public class ToDoList
    {
        [Key]
        //EF will create an IDENTITY column in the SQL database for this property
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToDoId { get; set; }

        //ErrorMessage in string only enforces when data is entered
        [StringLength(100, MinimumLength = 2, ErrorMessage = "* Description must be between 2 and 100 character in length.")]
        public string Description { get; set; }

        //NotCompleted = 0, Completed = 1
        public TaskStatus TaskCompleted { get; set; }
    }
}
