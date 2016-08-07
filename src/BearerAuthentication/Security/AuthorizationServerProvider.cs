﻿using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace BearerAuthentication
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Request.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                var username = context.UserName;
                var password = context.Password;

                if ((username != "mkspinto") || (password != "mkspinto"))
                {
                    context.SetError("invalid_grant", "Usuário ou senha inválidos");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, username));

                var roles = new List<string>();
                roles.Add("User");

                foreach (var role in roles)
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));

                GenericPrincipal principal = new GenericPrincipal(identity, roles.ToArray());
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch
            {
                context.SetError("invalid_grant", "Falha ao autenticar");
            }
        }
    }
}