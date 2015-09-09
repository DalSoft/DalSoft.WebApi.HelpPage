using System.Collections.Generic;
using System.Web.Http;

namespace DalSoft.WebApi.HelpPage.Sample
{
    
    public class ValuesController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Gets values.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("values")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// Gets the value specified by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpGet, Route("values/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        /// <summary>
        /// Creates a new value.
        /// </summary>
        /// <param name="value">The value.</param>
        [HttpPost, Route("values")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        /// <summary>
        /// Updates a value.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        [HttpPut, Route("values/{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        /// <summary>
        /// Deletes the value.
        /// </summary>
        /// <param name="id">The id.</param>
        [HttpDelete, Route("values/{id}")]
        public void Delete(int id)
        {
        }
    }
}