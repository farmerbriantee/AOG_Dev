using System;
using System.Collections.Generic;

namespace AOG.Classes
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
        public Guid Id;
        public string Name;
        public double OriginLat;
        public double OriginLon;
        public double Convergence;
        public List<CoordinateDto> OuterBoundary;
        public List<List<CoordinateDto>> InnerBoundaries;
        public List<AbLineUploadDto> AbLines;
    }
}
