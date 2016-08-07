using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BearerAuthentication
{
    public class AuthorizationServerProviderRefreshToken : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var guid = Guid.NewGuid().ToString();

            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
            };

            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            _refreshTokens.TryAdd(guid, refreshTokenTicket);
            context.SetToken(guid);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            Guid token;
            if (Guid.TryParse(context.Token, out token))
            {
                AuthenticationTicket ticket;
                if (_refreshTokens.TryRemove(context.Token, out ticket))
                    context.SetTicket(ticket);
            }
        }
    }
}