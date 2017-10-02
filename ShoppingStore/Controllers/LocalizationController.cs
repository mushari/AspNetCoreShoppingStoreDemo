using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.JsonLocalization;
using Newtonsoft.Json;
using ShoppingStore.Models.LocalizedViewModels;
using PaginationTagHelper.Extensions;

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
        public IActionResult Index(LocalizedViewModel model)
        {

            var jsonLocalizedPaging = jsonLocalization.ToPageList(model.Page, model.ItemPerPage);

            var jsonData = new LocalizedViewModel
            {
                Page = model.Page,
                ItemPerPage = model.ItemPerPage,
                Items = jsonLocalizedPaging,
                TotalItems = jsonLocalization.Count()
            };
            return View(jsonData);
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
        [Route("api/addNewCulture")]
        public IActionResult AddNewCulture(string pk, string culture, string value)
        {
            if (culture == null)
            {
                return BadRequest("Culture value can't be empty");
            }

            var jsonData = jsonLocalization.Where(data => data.Key == pk).ToList();
            foreach (var d in jsonData)
            {
                if (d.Value.Keys.Any(k => k.Equals(culture, StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest(culture + " has repeated");
                }
                d.Value[culture] = value;
            }

            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok(new { culture, value });
        }

        [HttpDelete]
        [Route("api/deleteCulture")]
        public IActionResult DeleteCulture(string culture, string value)
        {
            if (culture == null)
            {
                return BadRequest("Culture doesn't exist");
            }
            var jsonData = jsonLocalization.Where(d => d.Key == value).ToList();

            foreach (var data in jsonData)
            {
                data.Value.Remove(culture);
            }


            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok();
        }

        [HttpPost]
        [Route("api/editValue")]
        public IActionResult EditLocalizedValue(string pk, string name, string value)
        {

            jsonLocalization.Where(data => data.Key == pk)
              .ToList().ForEach(d => d.Value[name] = value);

            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok();
        }


    }
}