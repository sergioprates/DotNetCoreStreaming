using DotNetCoreStreaming.Enums;
using DotNetCoreStreaming.Models;
using DotNetCoreStreaming.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreStreaming.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClienteController : ControllerBase
    {
        private static ConcurrentBag<StreamWriter> _clients;

        static ClienteController()
        {
            _clients = new ConcurrentBag<StreamWriter>();
        }

        [HttpPost]
        public IActionResult Post(Cliente cliente)
        {
            //Fazer o Insert
            EnviarEvento(cliente, EventoEnum.Insert);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(Cliente cliente)
        {
            //Fazer o Update
            EnviarEvento(cliente, EventoEnum.Update);
            return Ok();
        }
        private static async Task EnviarEvento(object dados, EventoEnum evento)
        {
            foreach (var client in _clients)
            {
                string jsonEvento = string.Format("{0}\n", JsonConvert.SerializeObject(new { dados, evento }));
                await client.WriteAsync(jsonEvento);
                await client.FlushAsync();
            }
        }

        [HttpGet]
        [Route("Streaming")]
        public IActionResult Stream()
        {
            return new PushStreamResult(OnStreamAvailable, "text/event-stream", HttpContext.RequestAborted);
        }

        private void OnStreamAvailable(Stream stream, CancellationToken requestAborted)
        {
            var wait = requestAborted.WaitHandle;
            var client = new StreamWriter(stream);
            _clients.Add(client);

            wait.WaitOne();

            StreamWriter ignore;
            _clients.TryTake(out ignore);
        }
    }
}