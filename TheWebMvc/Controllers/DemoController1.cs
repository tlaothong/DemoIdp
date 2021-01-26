using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheWebMvc.Controllers
{
    public class DemoController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("https://https://localhost:44364//api/demoapi");

            var jobject = JsonSerializer.Deserialize<JsonDocument>(content);
            ViewBag.Json = JsonSerializer.Serialize(jobject);
            return View("json");
        }
    }
}
