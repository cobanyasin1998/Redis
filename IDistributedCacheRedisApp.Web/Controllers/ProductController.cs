using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30)
            };
            //await _distributedCache.GetStringAsync("name", "Yasin", cacheEntryOptions);
            ProductDTO productDTO = new ProductDTO()
            {
                Id = 1,
                Name = "Kalem",
                Price = 100
            };
            string jsonProduct = JsonConvert.SerializeObject(productDTO);

            Byte[] byteProduct_1 = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:3", byteProduct_1);

            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);
            await _distributedCache.SetStringAsync("product:2", jsonProduct, cacheEntryOptions);

            return View();
        }
        public IActionResult Show()
        {
            ViewBag.Name = _distributedCache.GetString("name");
            string jsonProduct = _distributedCache.GetString("product:1");
            string jsonProduct2 = _distributedCache.GetString("product:2");

            ProductDTO p = JsonConvert.DeserializeObject<ProductDTO>(jsonProduct);
            ProductDTO p_2 = JsonConvert.DeserializeObject<ProductDTO>(jsonProduct2);
            byte[] p_3_byteArray = _distributedCache.Get("product:3");

            ProductDTO p_3 = JsonConvert.DeserializeObject<ProductDTO>(Encoding.UTF8.GetString(p_3_byteArray));
            ViewBag.Product1 = p;
            ViewBag.Product2 = p_2;


            return View("Index");
        }
        public IActionResult Remove()
        {

            _distributedCache.Remove("name");
            return View("Index");
        }


        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/download.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("resim", imageByte);


            byte[] resimByte = _distributedCache.Get("resim");

            return File(resimByte, "image/jpg");

            //return View();
        }

    }
}
