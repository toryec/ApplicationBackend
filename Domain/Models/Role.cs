﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;
public class Role
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string RoleCode { get; set; } = default!;
}
