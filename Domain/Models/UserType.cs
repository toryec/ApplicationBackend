using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    User = 1,
    Admin = 2
}
