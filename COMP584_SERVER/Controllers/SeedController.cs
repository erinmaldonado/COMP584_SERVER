using COMP584_SERVER.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using WorldModel;

namespace COMP584_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(Comp584DbContext context, IHostEnvironment environment) : ControllerBase
    {
        string _pathName = Path.Combine(environment.ContentRootPath, "Data/worldcities.csv");

        [HttpPost("Countries")]
        public async Task<ActionResult> PostCountries()
        {
            Dictionary<string, Country> countries = await context.Countries.AsNoTracking().
                ToDictionaryAsync(c => c.Name, StringComparer.OrdinalIgnoreCase);
            CsvConfiguration config = new(CultureInfo.InvariantCulture) { 
                HasHeaderRecord = true, 
                HeaderValidated = null 
            }; 

            using StreamReader reader = new(_pathName); 
            using CsvReader csv = new(reader, config); 
            List<Comp584CSV> records = csv.GetRecords<Comp584CSV>().ToList();

            foreach (Comp584CSV record in records)
            {
                if (!countries.ContainsKey(record.country))
                {
                    Country country = new() {
                        Name = record.country,
                        Iso2 = record.iso2,
                        Iso3 = record.iso3
                    };

                    countries.Add(country.Name, country);
                    await context.Countries.AddAsync(country);
                }
            }

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Cities")]
        public async Task<ActionResult> PostCities()
        {
            Dictionary<string, Country> countries = await context.Countries.AsNoTracking().
                    ToDictionaryAsync(c => c.Name, StringComparer.OrdinalIgnoreCase);
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
             };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);
            List<Comp584CSV> records = csv.GetRecords<Comp584CSV>().ToList();
            int cityCount = 0;
            foreach (Comp584CSV record in records)
            {
                if (record.population.HasValue && record.population.Value > 0)
                {
                    City city = new()
                    {
                        Name = record.city,
                        Latitude = record.lat,
                        Longitude = record.lng,
                        Population = (int)record.population.Value,
                        CountryId = countries[record.country].Id
                    };
                    cityCount++;
                    await context.Cities.AddAsync(city);
                }
            }

            await context.SaveChangesAsync();

            return new JsonResult(cityCount);
        }
    }

}
