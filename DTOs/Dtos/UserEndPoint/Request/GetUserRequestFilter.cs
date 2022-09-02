using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Dtos.UserEndPoint.Request;

public class GetUserRequestFilter
{
    public bool IncludeDetails { get; set; }

    public bool IncludeRoles { get; set; }
}
