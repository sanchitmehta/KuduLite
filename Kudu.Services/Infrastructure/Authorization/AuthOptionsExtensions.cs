﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Kudu.Services.Infrastructure.Authentication;
using Kudu.Core;

namespace Kudu.Services.Infrastructure.Authorization
{
    public static class AuthOptionsExtensions
    {
        public static void AddInstanceAdminPolicies(this AuthorizationOptions options, IEnvironment environment)
        {
            options.AddPolicy(AuthPolicyNames.AdminAuthLevel, p =>
            {
                p.AddInstanceAuthenticationSchemes();
                p.AddRequirements(new AuthLevelRequirement(AuthorizationLevel.Admin));
            });

            options.AddPolicy(AuthPolicyNames.LinuxConsumptionRestriction, p =>
            {
                p.AddInstanceAuthenticationSchemes();
                if (Kudu.Core.Environment.IsOnLinuxConsumption)
                {
                    p.AddRequirements(new AuthLevelRequirement(AuthorizationLevel.Admin));
                }
                else
                {
                    p.AddRequirements(new AuthLevelRequirement(AuthorizationLevel.Anonymous));
                }
            });
        }

        private static void AddInstanceAuthenticationSchemes(this AuthorizationPolicyBuilder builder)
        {
            builder.AuthenticationSchemes.Add(ArmAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
