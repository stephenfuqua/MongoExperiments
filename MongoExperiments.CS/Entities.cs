using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MongoExperiments.CS.Entities
{
    public class SurveySite
    {
        public ObjectId Id { get; set; }
        public string SiteName { get; set; }
        public string CodeName { get; set; }
        public List<SurveyPoint> SurveyPoints { get; set; } = new List<SurveyPoint>();
        public List<SiteBoundary> SiteBoundaries { get; set; } = new List<SiteBoundary>();


        public int NumberOfPoints => SurveyPoints.Count;
        public int NumberOfBoundaries => SiteBoundaries.Count;

        public override string ToString() => "Id: \{this.Id}\nSite Name: \{this.SiteName}\nCode Name: \{this.CodeName}\nNumber of Points: \{this.NumberOfPoints}\nNumber of Boundaries: \{this.NumberOfBoundaries}\n";
    }

    public class SurveyPoint
    {
        public string CodeName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class SiteBoundary
    {
        public int VertexSequence { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
