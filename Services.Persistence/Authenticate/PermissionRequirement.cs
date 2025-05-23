﻿using Microsoft.AspNetCore.Authorization;

namespace Services.Persistence.Authenticate
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission) => Permission = permission;
    }
}
