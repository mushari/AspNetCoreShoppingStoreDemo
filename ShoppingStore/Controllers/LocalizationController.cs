using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.JsonLocalization;
using Newtonsoft.Json;
using ShoppingStore.Models.LocalizedViewModels;
using PaginationTagHelper.Extensions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace ShoppingStore.Controllers
{
    public class LocalizationController : Controller
    {
        private List<JsonLocalizationFormat> jsonLocalization;
        private IStringLocalizer<LocalizationController> localizer;
        public LocalizationController(IStringLocalizer<LocalizationController> localizer)
        {
            jsonLocalization = JsonConvert.DeserializeObject<List<JsonLocalizationFormat>>(
                System.IO.File.ReadAllText(@"Resources/localization.json"));
            this.localizer = localizer;
        }
        public IActionResult Index(LocalizedViewModel model)
        {

            var jsonLocalizedPaging =
                jsonLocalization.ToPageList(model.Page, model.ItemPerPage);

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
        [Route("api/addNewKeyName")]
        public IActionResult AddNewKeyName(string pk, string culture, string value)
        {
            if (String.IsNullOrWhiteSpace(pk))
            {
                return BadRequest(new
                {
                    keyname = localizer["KeyName"],
                    notempty = localizer["CannotNullEmpty"]
                });
            }

            if (jsonLocalization.Any(l => l.Key.Equals(pk)))
            {
                return BadRequest(localizer["AlreadyHave"]);
            }


            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (!regexItem.IsMatch(pk))
            {
                return BadRequest(localizer["LettersNumbersOnly"]);
            }

            if (String.IsNullOrWhiteSpace(culture) || String.IsNullOrWhiteSpace(value))
            {
                var onlyKeyName = new JsonLocalizationFormat
                {
                    Key = pk
                };
                jsonLocalization.Add(onlyKeyName);

                var onlyKeyName_output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
                System.IO.File.WriteAllText(@"Resources/localization.json", onlyKeyName_output);

                return Ok();
            }

            var newData = new JsonLocalizationFormat
            {
                Key = pk,
                Value = new Dictionary<string, string>
                {
                    [culture] = value
                }
            };

            jsonLocalization.Add(newData);

            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok();
        }

        [HttpPost]
        [Route("api/editKeyName")]
        public IActionResult EditKeyName(string pk, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return BadRequest(new
                {
                    keyname = localizer["KeyName"],
                    notempty = localizer["CannotNullEmpty"]
                });
            }

            if (jsonLocalization.Any(l => l.Key.Equals(value)))
            {
                return BadRequest(localizer["AlreadyHave"]);
            }

            if (String.IsNullOrWhiteSpace(pk))
            {
                return NotFound(localizer["NotFound"]);
            }

            var regexItem = new Regex("^[a-zA-Z0-9]*$");

            if (!regexItem.IsMatch(value))
            {
                return BadRequest(localizer["LettersNumbersOnly"]);
            }

            jsonLocalization.Where(data => data.Key == pk)
                .ToList().ForEach(d => d.Key = value);

            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok();
        }

        [HttpDelete]
        [Route("api/deleteKeyName")]
        public IActionResult DeleteKeyName(string pk)
        {
            if (String.IsNullOrWhiteSpace(pk))
            {
                return NotFound();
            }

            var keyName = jsonLocalization.FirstOrDefault(p => p.Key == pk);
            if (keyName == null)
            {
                return NotFound(localizer["NotFound"]);
            }

            jsonLocalization.Remove(keyName);

            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok();
        }

        [HttpPost]
        [Route("api/addNewCulture")]
        public IActionResult AddNewCulture(string pk, string culture, string value)
        {
            if (String.IsNullOrWhiteSpace(culture))
            {
                return BadRequest(new
                {
                    culture = localizer["Culture"],
                    notempty = localizer["CannotNullEmpty"]
                });
            }

            var cultureFormat1 = new Regex("^[a-zA-Z]*-[a-zA-Z]*$");
            var cultureFormat2 = new Regex("^[a-zA-Z]*$");

            if (cultureFormat1.IsMatch(culture) || cultureFormat2.IsMatch(culture))
            {
                var jsonData = jsonLocalization.Where(data => data.Key == pk).ToList();

                foreach (var d in jsonData)
                {
                    if (d.Value.Keys.Any(k => k.Equals(culture, StringComparison.OrdinalIgnoreCase)))
                    {
                        return BadRequest(new { culture = localizer["Culture"], alreadyhave = localizer["AlreadyHave"] });
                    }
                    d.Value[culture] = value;
                }

                var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
                System.IO.File.WriteAllText(@"Resources/localization.json", output);

                return Ok(new { culture, value });
            }
            return BadRequest(new
            {
                culture = localizer["Culture"],
                formatnotright = localizer["FormatNotCorrect"]
            });
        }

        [HttpDelete]
        [Route("api/deleteCulture")]
        public IActionResult DeleteCulture(string culture, string value)
        {

            var jsonData = jsonLocalization.Where(d => d.Key == value).ToList();
            if (jsonData == null)
            {
                return NotFound();
            }

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
            if (String.IsNullOrWhiteSpace(pk))
            {
                return NotFound(localizer["NotFound"]);
            }

            jsonLocalization.Where(data => data.Key == pk)
              .ToList().ForEach(d => d.Value[name] = value);

            var output = JsonConvert.SerializeObject(jsonLocalization, Formatting.Indented);
            System.IO.File.WriteAllText(@"Resources/localization.json", output);

            return Ok();
        }


    }
}