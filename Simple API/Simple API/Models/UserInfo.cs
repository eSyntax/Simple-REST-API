using Simple_API.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Simple_API.Models
{
    public class UserInfo
    {
        [Key]
        //EF will create an IDENTITY column in the SQL database for this property
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        //ErrorMessage in string only enforces when data is entered
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "* Email must be between 4 and 30 character in length.")]
        public string Email { get; set; }

        //ErrorMessage in string only enforces when data is entered
        [StringLength(30, MinimumLength = 12, ErrorMessage = "* Password must be between 12 and 30 character in length.")]
        public string Password { get; set; }

        //Admin = 0, User = 1
        public UserPrivileges Privileges { get; set; }

        [JsonIgnore]
        public ICollection<ToDoList> UserToDoList { get; set; }

    }
}
