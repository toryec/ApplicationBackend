using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Core.Interfaces;
public interface IAuthenticationManager
{
    Task<string?> Authenticate(User user, CancellationToken cancellationToken);
}
