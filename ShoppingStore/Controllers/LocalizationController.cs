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

            var dataList = new List<JsonLocalizationFormat>();
            if (model.SearchItem != null)
            {
                dataList = jsonLocalization.Where(
                    d => d.Key.Contains(model.SearchItem) ||
                    d.Value.Values.Any(v => v.Equals(model.SearchItem, StringComparison.OrdinalIgnoreCase))).ToList();
            }
            else
            {
                dataList = jsonLocalization.ToList();
            }

            var pageList = dataList.ToPageList(model.Page, model.ItemPerPage);

            var jsonData = new LocalizedViewModel
            {
                Page = model.Page,
                ItemPerPage = model.ItemPerPage,
                Items = pageList,
                TotalItems = dataList.Count(),
                SearchItem = model.SearchItem
            };
            return View(jsonData);
        }

        [HttpGet]
        [Route("api/getJsonData")]
        public IActionResult GetJsonData()
        {
            List<string> keyName = new List<string>();
            List<string> localizedValue = new List<string>();
            foreach (var item in jsonLocalization)
            {
                keyName.Add(item.Key);
                foreach (var value in item.Value)
                {
                    localizedValue.Add(value.Value);
                }
            }

            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>
            {
                ["keyname"] = keyName,
                ["localizedValue"] = localizedValue
            };

            return Ok(data);
        }

        [HttpGet]
        [Route("api/getKeyNamesData")]
        public IActionResult GetKeyNamesData()
        {
            List<string> keyNames = new List<string>();
            foreach (var item in jsonLocalization)
            {
                keyNames.Add(item.Key);
            }
            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>
            {
                ["keyname"] = keyNames
            };
            var jsondata = JsonConvert.SerializeObject(data);
            return Ok(jsondata);
        }

        [HttpGet]
        [Route("api/getLocalizedValuesData")]
        public IActionResult GetLocalizedValuesData()
        {
            List<string> values = new List<string>();
            foreach (var item in jsonLocalization)
            {
                foreach (var value in item.Value.Values)
                {
                    values.Add(value);
                }
            }

            var LocalizedValue = JsonConvert.SerializeObject(values);

            return Ok(LocalizedValue);
        }

        [HttpPost]
        [Route("api/addKeyName")]
        public IActionResult AddKeyName(string pk, string culture, string value)
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


            var regexItem = new Regex("^[a-zA-Z0-9_]*$");

            if (!regexItem.IsMatch(pk))
            {
                return BadRequest(localizer["LettersNumbersOnly"]);
            }


            if (String.IsNullOrWhiteSpace(culture))
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