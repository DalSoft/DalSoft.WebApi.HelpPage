using System.Web.Http;

namespace DalSoft.WebApi.HelpPage.Example.SelfHost
{
    public class UserController : ApiController
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("users"),]
        public string[] Get()
        {
            return new[] { "user1", "user2" };
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpDelete, Route("users/{id}"),]
        public bool Delete(int id)
        {
            return true;
        }
    }
}