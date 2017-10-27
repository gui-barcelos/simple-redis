using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace SimpleApiWithRedis.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public TodoController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string todo)
        {
            var key = Guid.NewGuid().ToString();

            await _distributedCache.SetStringAsync(key, todo);

            return Ok(key);
        }

        [Route("{key}")]
        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            var todo = await _distributedCache.GetStringAsync(key);

            return Ok(todo);
        }

        [Route("{key}")]
        [HttpPut]
        public async Task<IActionResult> Update(string key, string todo)
        {
            var currentTodo = await _distributedCache.GetStringAsync(key);

            await _distributedCache.SetStringAsync(key, todo);

            return Ok(todo);
        }

        [Route("{key}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string key)
        {
            var todo = await _distributedCache.GetStringAsync(key);

            await _distributedCache.RemoveAsync(key);

            return Ok();
        }
    }
}
