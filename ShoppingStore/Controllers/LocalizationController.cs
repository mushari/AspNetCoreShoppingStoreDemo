using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.JsonLocalization;
using Newtonsoft.Json;

namespace ShoppingStore.Controllers
{
    public class LocalizationController : Controller
    {
        private List<JsonLocalizationFormat> jsonLocalization;
        public LocalizationController()
        {
            jsonLocalization = JsonConvert.DeserializeObject<List<JsonLocalizationFormat>>(
                System.IO.File.ReadAllText(@"Resources/localization.json"));
        }
        public IActionResult Index()
        {
            return View(jsonLocalization);
        }

        [HttpPost]
        [Route("api/editKeyName")]
        public IActionResult EditKeyName(string pk, string value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            jsonLocalization.Where(data => data.Key == pk)
                .ToList().ForEach(d => d.Key = value);

            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok();
        }

        [HttpPost]
        [Route("api/editCulture")]
        public IActionResult EditCulture(string pk, string value, string name)
        {
            return Ok();
        }

        [HttpPost]
        [Route("api/editValue")]
        public IActionResult EditLocalizedValue(string pk, string value)
        {
            return Ok();
        }


    }
}