using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;
public class User
{

    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public IEnumerable<UserRole>? UserRoles { get; set; }
    public UserDetail? UserDetail { get; set; }


}
