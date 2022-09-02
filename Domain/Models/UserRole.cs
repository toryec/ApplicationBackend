using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;
public class UserRole
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
    public string RoleCode { get; set; } = default!;
}
