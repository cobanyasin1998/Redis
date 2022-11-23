using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;

        private string listKey = "sortedSetNames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();
            if (_db.KeyExists(listKey))
            {
                //_db.SortedSetScan(listKey).ToList().ForEach(x =>
                //{
                //    list.Add(x.ToString());
                //});

                _db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x => {

                    list.Add(x.ToString());
                });
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            _db.SortedSetAdd(listKey, name, score);
            _db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteItem(string name)
        {
            _db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            _db.SortedSetRemove(listKey, name);

            return RedirectToAction("Index");
        }
    }
}
