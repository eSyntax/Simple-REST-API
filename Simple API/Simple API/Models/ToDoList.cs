using Simple_API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [Required]
        public TaskStatus TaskCompleted { get; set; }

        [Required]
        [JsonIgnore]
        public int UserId { get; set; }

        //ToDoList user data from foreign key
        [JsonIgnore]
        [ForeignKey("UserId")]
        public UserInfo UserDetails { get; set; }

        //Logged user data from JWT
        [NotMapped]
        [JsonIgnore]
        public UserInfo LoggedUserDetails { get; set; }

        //Newtonsoft serialize variable only if logged user is admin
        public bool ShouldSerializeUserDetails()
        {
            return LoggedUserDetails.Privileges == UserPrivileges.Admin;
        }

        //Newtonsoft serialize variable only if logged user is admin
        public bool ShouldSerializeUserId()
        {
            return LoggedUserDetails.Privileges == UserPrivileges.Admin;
        }

        //Newtonsoft never serialize this object. [JsonIgnore] not working alone since newtonsoft is installed.
        public bool ShouldSerializeLoggedUserDetails()
        {
            return false;
        }
    }
}
