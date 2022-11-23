using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;

        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();

            if (_db.KeyExists(listKey))
            {
                _db.ListRange(listKey).ToList().ForEach(x => namesList.Add(x));
            }

            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            _db.ListRightPush(listKey, name);
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> DeleteItem(string name)
        {
            await _db.ListRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
