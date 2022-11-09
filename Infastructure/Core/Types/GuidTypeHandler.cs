using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Infastructure.Core.Types;
public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    // Handles how data is De-serialized into the object
    public override Guid Parse(object value)
    {
        //return new Guid((string)value);
        _ = Guid.TryParse(value.ToString(), out Guid result);
        return result;
    }

    // Handles how data is saved into the database
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }
}
