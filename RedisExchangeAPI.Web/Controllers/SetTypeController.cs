using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;

        private string listKey = "hashNames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            //ListType dan farkı unique kaydeder
            if (_db.KeyExists(listKey))
            {
                _db.SetMembers(listKey).ToList().ForEach(name => namesList.Add(name.ToString()));
            }

            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            if (!_db.KeyExists(listKey))
            {
                _db.KeyExpire(listKey, System.DateTime.Now.AddMinutes(5));
            }

            _db.SetAdd(listKey, name);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await _db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
