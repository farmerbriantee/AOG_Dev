using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AOG
{
    public class FieldSnapshot
    {
        public string FieldName { get; set; }
        public string FieldDirectory { get; set; }
        public Guid FieldId { get; set; }
        public double OriginLat { get; set; }
        public double OriginLon { get; set; }
        public double Convergence { get; set; }
        public List<List<vec3>> Boundaries { get; set; }
        public List<CTrk> Tracks { get; set; }
        public CNMEA Converter { get; set; }
    }

    public class CoordinateDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class AgShareFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }

    public class AbLineUploadDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<CoordinateDto> Coords { get; set; }
    }

    public class FieldDownloadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double OriginLat { get; set; }

        [JsonProperty("longitude")]
        public double OriginLon { get; set; }

        [JsonProperty("boundary")]
        public string BoundaryGeoJson { get; set; }

        [JsonProperty("abLines")]
        public JObject AbLinesRaw { get; set; }
    }

    public class AgShareGetOwnFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CoordinateDto> OuterBoundary { get; set; }
        public double AreaHa => GeoUtils.CalculateAreaInHa(OuterBoundary);

    }

}



