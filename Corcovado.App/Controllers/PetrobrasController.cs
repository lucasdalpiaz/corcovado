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
    public class PetrobrasController : ApiController
    {
        private readonly IMessageFile _repo;

        public PetrobrasController(IMessageFile repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var results = await _repo.RetornaTodos();


                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> MaioresQue(string data, string mobile = null)
        {
            try
            {
                var results = await _repo.RetornaMaioresQueData(data, mobile);


                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> MenoresQue(string data, string mobile = null)
        {
            try
            {
                var results = await _repo.RetornaMenoresQueData(data, mobile);


                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> IguaisA(string data, string mobile = null)
        {
            try
            {
                var results = await _repo.RetornaIguaisData(data, mobile);


                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Entre(string data_ini, string data_fim, string mobile = null)
        {
            try
            {
                var results = await _repo.RetornaEntreData(data_ini, data_fim, mobile);


                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
