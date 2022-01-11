using Corcovado.Contexto;
using Corcovado.Modelos;
using Corcovado.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public  static async void RegistraLogComDiferencaMaiorQue(int diferenca)
        {
            using (var ctx = new DPSyncContext())
            {

                var result = await ctx.logNaoEncontrados.FromSqlRaw($@"
                WITH cte AS (
                SELECT 
                mobile,
                id,
                data_criacao as data_pos,
                LEAD (id,1) OVER (PARTITION BY mobile ORDER BY data_criacao desc) AS id_anterior,
                LEAD (data_criacao,1) OVER (PARTITION BY mobile ORDER BY data_criacao desc) AS data_pos_anterior,
                NOW() as data_criacao
                FROM tb_dpsync
                )
                select * ,  FLOOR((EXTRACT(EPOCH FROM (data_pos - data_pos_anterior))/60)/{diferenca})::int as qtd_falha  from cte 
                WHERE EXTRACT(EPOCH FROM (data_pos - data_pos_anterior))/60>={diferenca}  
                and (id not in (select id from tb_log_nao_encontrado)
                and id_anterior not in (select id_anterior from tb_log_nao_encontrado))
                and data_pos >= NOW()::DATE - 1 AND data_pos_anterior >= NOW()::DATE - 1  

                ").ToListAsync();
                foreach (var item in result)
                {
                    await ctx.logNaoEncontrados.AddAsync(item);
                    await ctx.SaveChangesAsync();

                    for (int i = 1; i <= item.QtdFalha; i++)
                    {
                        await ctx.logNaoEncontradoProcessados.AddAsync(
                            new LogNaoEncontradoProcessado
                            {
                                DataPos = item.DataPosAnterior.AddMinutes(diferenca*i),
                                mobile = item.mobile
                            }
                            );
                    }
                    await ctx.SaveChangesAsync();

                }


                

            }
        }




        [HttpGet]
        public static async void RegistraLogPorcentagem(int diasAnteriores)
        {
            List<DateTime> listDiasAnteriores = new List<DateTime>();
            for (int i = 0; i < diasAnteriores; i++)
            {
                listDiasAnteriores.Add(DateTime.Now.AddDays(-i));
            }



            using (var ctx = new DPSyncContext())
            {
                foreach (DateTime dia in listDiasAnteriores)
                {
                    var logNaoEncontradoProcessados = await (from  A in ctx.logNaoEncontradoProcessados
                                            where A.DataPos.Date == dia.Date 
                                                   select A).ToListAsync();

                    int countNaoEncontrados = logNaoEncontradoProcessados.Count();

                  

                    int barcosCont = ctx.barcos.Count();
                    int esperado = barcosCont * 72;
                    int realizado = esperado - countNaoEncontrados;
                    double porcentagem = ((double)realizado / (double)esperado)*100;

                    //Add or update
                    LogPorcentagem logPorcentagem = await ctx.logPorcentagens.FirstOrDefaultAsync(x => x.DataCriacao.Date == dia.Date);

                    if (logPorcentagem == null)
                    {
                        logPorcentagem = new LogPorcentagem
                        {
                        DataCriacao = dia,
                        DataAlteracao = DateTime.Now,
                        Esperado = esperado,
                        Porcentagem = porcentagem,
                        Realizado = realizado,
                        Posicoes = 72,
                        QtdMobile = barcosCont
                        };

                        await ctx.AddAsync<LogPorcentagem>(logPorcentagem);
                    }
                    else
                    {
                        logPorcentagem.QtdMobile = barcosCont;
                        logPorcentagem.Posicoes = 72;
                        logPorcentagem.Realizado = realizado;
                        logPorcentagem.Porcentagem = porcentagem;
                        logPorcentagem.Esperado = esperado;
                        logPorcentagem.DataAlteracao = DateTime.Now;
                        ctx.Update<LogPorcentagem>(logPorcentagem);
                    }

                    string caminho_txt = @"c:\dpsync_log_porcentagem_txt\" ;

                    if (!Directory.Exists(caminho_txt))
                        Directory.CreateDirectory(caminho_txt);
                    var txt = new StringBuilder();
                    string newLine = porcentagem.ToString();
                    txt.Append(newLine);
                    caminho_txt += "\\" + dia.ToString("yyyMMdd") + ".txt";
                    File.WriteAllText(
                        caminho_txt,
                        txt.ToString());


                    await ctx.SaveChangesAsync();
                }


               


            }
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
