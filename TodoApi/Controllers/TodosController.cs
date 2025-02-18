using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoLibrary.DataAccess;
using TodoLibrary.Models;   

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoData data;
        private readonly ILogger<TodosController> logger;

        public TodosController(ITodoData data, ILogger<TodosController> logger)
        {
            this.data = data;
            this.logger = logger;
        }

        private int GetUserId()
        {
            var userIdText = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdText);
        }

        // GET: api/Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoModel>>> Get()
        {
            logger.LogInformation("Getting all todos");
            try
            {
                var output = await data.GetAllAssigned(GetUserId());
                return Ok(output);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting all todos");
                return BadRequest();
            }

        }

        // GET api/Todos/5
        [HttpGet("{todoId}")]
        public async Task<ActionResult<TodoModel>> Get(int todoId)
        {

            try
            {
                var output = await data.GetOneAssigned(GetUserId(), todoId);
                return Ok(output);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error getting todo {todoId}", todoId);
                return BadRequest();
            }

            
        }


        // POST api/Todos
        [HttpPost]
        public async Task<ActionResult<TodoModel>> Post([FromBody] string task)
        {
            var output = await data.Create(GetUserId(), task);
            return Ok(output);

        }

        // PUT api/Todos/5
        [HttpPut("{todoId}")]
        public async Task<ActionResult> Put(int todoId, [FromBody] string task)
        {
            await data.UpdateTask(GetUserId(), todoId, task);
            return Ok();
        }


        // PUT api/Todos/5
        [HttpPut("{todoId}/Complete")]
        public async Task<ActionResult> Complete(int todoId)
        {
            await data.CompleteTodo(GetUserId(), todoId);
            return Ok();

        }
        // DELETE api/Todos/5
        [HttpDelete("{todoId}")]
        public async Task<ActionResult> Delete(int todoId)
        {
            await data.Delete(GetUserId(), todoId);
            return Ok();

        }
    }
}
