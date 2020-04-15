using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherEmailer.Logic.Api.AccuWeatherService.Models
{
    public class CitySearchResult
    {
        public decimal Version { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public decimal Rank { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
        public string PrimaryPostalCode { get; set; }
        public Region Region { get; set; }
        public Country Country { get; set; }
        public Administrativearea AdministrativeArea { get; set; }
        public Timezone TimeZone { get; set; }
        public Geoposition GeoPosition { get; set; }
        public bool IsAlias { get; set; }
        public IEnumerable<Supplementaladminarea> SupplementalAdminAreas { get; set; }
        public IEnumerable<string> DataSets { get; set; }
    }

    public class Region
    {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
    }

    public class Country
    {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
    }

    public class Administrativearea
    {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
        public decimal Level { get; set; }
        public string LocalizedType { get; set; }
        public string EnglishType { get; set; }
        public string CountryID { get; set; }
    }

    public class Timezone
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal GmtOffset { get; set; }
        public bool IsDaylightSaving { get; set; }
        public DateTime NextOffsetChange { get; set; }
    }

    public class Geoposition
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Elevation Elevation { get; set; }
    }

    public class Elevation
    {
        public Metric Metric { get; set; }
        public Imperial Imperial { get; set; }
    }

    public class Metric
    {
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public decimal UnitType { get; set; }
    }

    public class Imperial
    {
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public decimal UnitType { get; set; }
    }

    public class Supplementaladminarea
    {
        public decimal Level { get; set; }
        public string LocalizedName { get; set; }
        public string EnglishName { get; set; }
    }

}
