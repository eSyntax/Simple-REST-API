using Simple_API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_API.Models
{
    public class UserInfo
    {
        
        [Key]
        //EF will create an IDENTITY column in the SQL database for this property
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        // ErrorMessage in string only enforces when data is entered
        [StringLength(15, MinimumLength = 4, ErrorMessage = "* Username must be between 4 and 15 character in length.")]
        public string UserName { get; set; }

        // ErrorMessage in string only enforces when data is entered
        [StringLength(25, MinimumLength = 4, ErrorMessage = "* Password must be between 4 and 25 character in length.")]
        public string Password { get; set; }

        public UserPrivileges Privileges { get; set; }
    }
}
