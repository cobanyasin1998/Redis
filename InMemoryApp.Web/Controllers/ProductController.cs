using System;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index1()
        {

            //1.RoadMap
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));

            }
            //2.RoadMap
            if (_memoryCache.TryGetValue("zaman", out string zamanCache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            }
            return View();
        }

        public IActionResult Show1()
        {
            //_memoryCache.Remove("zaman");

            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.Zaman = _memoryCache.Get<string>("zaman");
            return View();
        }

        public IActionResult Expiration()
        {


            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                SlidingExpiration = TimeSpan.FromSeconds(90),
                Priority = CacheItemPriority.High

            };
            cacheEntryOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value}=> Sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString("dd.MM.yyyy HH:mm"), cacheEntryOptions);

            //}

            //if (_memoryCache.TryGetValue("zaman", out string zamanCache))
            //{
            //    ViewBag.Zaman = zamanCache;
            //}
            return View("ExpirationShow");
        }
        public IActionResult ExpirationShow()
        {
            _memoryCache.TryGetValue("zaman", out string zamanOut);

            _memoryCache.TryGetValue("callback", out string callBack);
            ViewBag.callBack = callBack;
            ViewBag.Zaman = zamanOut;
            return View();
        }




        public IActionResult Product()
        {
            //Complex Types Caching
            ProductDTO p = new ProductDTO
            {
                Id = 1,
                Name = "Kalem",
                Price = 1000
            };

            _memoryCache.Set<ProductDTO>("product:1", p);




            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                SlidingExpiration = TimeSpan.FromSeconds(90),
                Priority = CacheItemPriority.High

            };
            cacheEntryOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value}=> Sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString("dd.MM.yyyy HH:mm"), cacheEntryOptions);

            //}

            //if (_memoryCache.TryGetValue("zaman", out string zamanCache))
            //{
            //    ViewBag.Zaman = zamanCache;
            //}
            return View("ExpirationShow");
        }





    }
}
