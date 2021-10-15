using Corcovado.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Corcovado.App.Controllers
{
    public class LogController : ApiController
    {
        // GET: api/Log
        private readonly ILog _repo;

        public LogController(ILog repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllLogsAsync();


                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Log/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Log
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Log/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Log/5
        public void Delete(int id)
        {
        }
    }
}
