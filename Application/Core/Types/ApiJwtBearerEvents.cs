﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Application.Core.Types;
public class ApiJwtBearerEvents : JwtBearerEvents
{
    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        return base.AuthenticationFailed(context);
    }

    public override Task Challenge(JwtBearerChallengeContext context)
    {
        return base.Challenge(context);
    }

    public override Task Forbidden(ForbiddenContext context)
    {
        return base.Forbidden(context);
    }

    public override Task MessageReceived(MessageReceivedContext context)
    {
        return base.MessageReceived(context);
    }

    public override Task TokenValidated(TokenValidatedContext context)
    {
        return base.TokenValidated(context);
    }
}
