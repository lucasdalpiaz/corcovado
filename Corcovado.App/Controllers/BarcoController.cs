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
    public class BarcoController : ApiController
    {
        private readonly IBarco _repo;

        public BarcoController(IBarco repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IHttpActionResult> RetornarTodos()
        {
            try
            {
                var results = await _repo.RetornarTodos();

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
        [HttpGet]
        public async Task<IHttpActionResult> Ativados()
        {
            try
            {
                var results = await _repo.Ativados();

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Desativados()
        {
            try
            {
                var results = await _repo.Desativados();

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Ativar(string barco)
        {
            try
            {
                var results = await _repo.Ativar(barco);

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Desativar(string barco)
        {
            try
            {
                var results = await _repo.Desativar(barco);

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
