using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace DapperApi;

public class Logging
{
    public Loglevel LogLevel { get; set; }
}

public class Loglevel
{
    [Required]
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class MyService
{
    public MyService(IOptions<Logging> options)
    {
    }
}

