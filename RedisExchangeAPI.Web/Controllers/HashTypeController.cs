using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashKey { get; set; } = "sozluk";
        public HashTypeController(RedisService redisService) : base(redisService)
        {

        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (_db.KeyExists(hashKey))
            {
                _db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(string key, string val)
        {
            _db.HashSet(hashKey, key, val);
            return RedirectToAction("Index");
        }
    }
}
