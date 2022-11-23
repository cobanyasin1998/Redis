using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringType : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        public StringType(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            byte[] resimByte = default(byte[]);
            _db.StringSet("resim", resimByte);
            _db.StringSet("name", "Yasin Çoban");
            _db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            RedisValue value = _db.StringGet("name");
            _db.StringDecrement("ziyaretci", 2);
            RedisValue ziyaretciValue = _db.StringGet("ziyaretci");

            if (value.HasValue)
            {
                ViewBag.Value = value.ToString();
            }
            if (ziyaretciValue.HasValue)
            {
                ViewBag.ZiyaretciValue = ziyaretciValue.ToString();
            }



            return View();
        }
    }
}
