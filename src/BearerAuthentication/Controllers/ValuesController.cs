using System.Web.Http;

namespace BearerAuthentication.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "User")]
        public string Get()
        {
            return User.Identity.Name;
        }
    }
}