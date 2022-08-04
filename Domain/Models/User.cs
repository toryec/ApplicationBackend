using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;
public class User
{
    [Key]
    public Guid Id { get; set; } 
    [Required]
    public string UserName { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
 
    public IEnumerable<Role>? Roles { get; set; }

    //[Required]
    //public byte[] Salt { get; set; } = default!;
    public UserDetail? UserDetail { get; set; }

    public UserType UserType { get; set; }
    

}
