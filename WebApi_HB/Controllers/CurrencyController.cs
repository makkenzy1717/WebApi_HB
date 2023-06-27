using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
namespace Test3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public CurrencyController(IConfiguration configuration, AppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpGet("save/{date}")]
        public async Task<IActionResult> SaveCurrencyData(string date)
        {
            try
            {
                string apiUrl = $"https://nationalbank.kz/rss/get_rates.cfm?fdate={date}";
                HttpClient httpClient = new HttpClient();
                string response = await httpClient.GetStringAsync(apiUrl);

                // Распарсить данные из формата XML
                // ...
// Предполагается, что у вас есть строка response, содержащая XML-данные
// Подключите необходимые пространства имён
                

// Распарсить XML-данные
                XDocument xmlDoc = XDocument.Parse(response);

// Получить все элементы "item"
                IEnumerable<XElement> items = xmlDoc.Descendants("item");

// Проход по каждому элементу "item" и получение необходимых данных
                foreach (XElement item in items)
                {
                    string title = item.Element("fullname")?.Value;
                    string code = item.Element("title")?.Value;
                    string value = item.Element("description")?.Value;

                    // Создать объект Currency и сохранить данные в базу данных
                    Currency currency = new Currency
                    {
                        Title = title,
                        Code = code,
                        Value = decimal.Parse(value),
                        A_DATE = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture)
                    };

                    _dbContext.Currencies.Add(currency);
                }

// Сохранить изменения в базе данных
                _dbContext.SaveChanges();

                // Сохранить данные в базу данных
                // ...
            
                return Ok(new { count = 1 });
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to save currency data.");
            }
        }

        [HttpGet("{date}/{code?}")]
        public IActionResult GetCurrencyData(string date, string code = null)
        {
            try
            {
                IQueryable<Currency> query = _dbContext.Currencies.Where(c => c.A_DATE == DateTime.Parse(date));

                if (!string.IsNullOrEmpty(code))
                    query = query.Where(c => c.Code == code);

                var data = query.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to retrieve currency data.");
            }
        }
    }
    }