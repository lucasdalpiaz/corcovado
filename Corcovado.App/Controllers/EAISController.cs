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
    public class EAISController : ApiController
    {
        private readonly IEAIS _repo;

        public EAISController(IEAIS repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllEais();

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
