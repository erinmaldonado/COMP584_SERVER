﻿namespace COMP584_SERVER.Data
{
    public class Comp584CSV
    {
        public required string city { get; set; }
        public required string city_ascii { get; set; }
        public decimal lat { get; set; }
        public required decimal lng { get; set; }
        public required string country { get; set; }
        public required string iso2 { get; set; }
        public required string iso3 { get; set; }
        public required string admin_name { get; set; }
        public required string capital { get; set; }
        public decimal? population { get; set; }
        public long id { get; set; }
    }
}