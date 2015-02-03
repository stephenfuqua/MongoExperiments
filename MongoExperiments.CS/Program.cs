using MongoDB.Bson;
using MongoDB.Driver;
using MongoExperiments.CS.Entities;
using System;

namespace MongoExperiments.CS
{
    class Program
    {
        private const string MONGO_CONNECTION_STRING = "mongodb://localhost";
        private const string MONGO_DATABASE_NAME = "IbaMonitoring";

        static void Main(string[] args)
        {
            MongoDal<SurveySite> repository = InitializeMongoRepository();
            try
            {
                var surveySite = InitializeFirstSite();
                var surveySite2 = InitializeSecondSite();

                ClearAnyExistingRecords(repository);
                InsertRecords(surveySite, surveySite2, repository);
                var results = RetrieveTheSavedRecords(repository);
                WriteResultsToTheConsole(results);
            }
            catch (AggregateException aggregate)
            {
                HandleAggregateException(aggregate);
            }

            //PressAnyKeyToContinue();
        }

        private static void HandleAggregateException(AggregateException aggregate)
        {
            foreach (var exception in aggregate.InnerExceptions)
            {
                ProcessInnerException(exception);
            }
        }

        private static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press enter to exit...");
            Console.Read();
        }

        private static void WriteResultsToTheConsole(System.Collections.Generic.List<SurveySite> results)
        {
            //results.ForEach(x => Console.WriteLine(x.ToString()));
            results.ForEach(x => Console.WriteLine(x.ToJson()));
        }

        private static System.Collections.Generic.List<SurveySite> RetrieveTheSavedRecords(MongoDal<SurveySite> repository)
        {
            var task = repository.Select(x => true);
            task.Wait();
            var results = task.Result;
            return results;
        }

        private static void InsertRecords(SurveySite surveySite, SurveySite surveySite2, MongoDal<SurveySite> repository)
        {
            repository.Insert(new[] { surveySite, surveySite2 }).Wait();
        }

        private static void ClearAnyExistingRecords(MongoDal<SurveySite> repository)
        {
            repository.Delete(x => true).Wait();
        }

        private static MongoDal<SurveySite> InitializeMongoRepository()
        {
            var mongoClient = new MongoClient(MONGO_CONNECTION_STRING);
            var repository = new MongoDal<SurveySite>(mongoClient, MONGO_DATABASE_NAME);
            return repository;
        }

        private static void ProcessInnerException(Exception exception)
        {
            Console.WriteLine(exception.Message);
            var mongEx = exception as MongoWriteException;
            if (mongEx != null)
            {
                Console.WriteLine(mongEx.WriteError);
                Console.WriteLine(mongEx.WriteConcernError);
            }
            if (exception.InnerException != null)
            {
                ProcessInnerException(exception.InnerException);
            }
        }

        private static SurveySite InitializeSecondSite()
        {
            var surveySite2 = new SurveySite
            {
                CodeName = "abd",
                SiteName = "A Different Location"
            };
            surveySite2.SiteBoundaries.AddRange(new[]
            {
                new SiteBoundary { Latitude = 2.1m, Longitude = 3.1m, VertexSequence = 1 },
                new SiteBoundary { Latitude = 2.2m, Longitude = 3.2m, VertexSequence = 2 },
                new SiteBoundary { Latitude = 2.3m, Longitude = 3.3m, VertexSequence = 3 },
                new SiteBoundary { Latitude = 2.4m, Longitude = 3.4m, VertexSequence = 4 }
            });
            surveySite2.SurveyPoints.AddRange(new[]
            {
                new SurveyPoint { Latitude = 2.11m, Longitude = 3.11m, CodeName ="P_2_1" },
                new SurveyPoint { Latitude = 2.12m, Longitude = 3.12m, CodeName ="P_2_2" },
                new SurveyPoint { Latitude = 2.13m, Longitude = 3.13m, CodeName ="P_2_3" },
                new SurveyPoint { Latitude = 2.14m, Longitude = 3.14m, CodeName ="P_2_4" },
            });
            return surveySite2;
        }

        private static SurveySite InitializeFirstSite()
        {
            var surveySite = new SurveySite
            {
                CodeName = "abc",
                SiteName = "A Park Somewhere"
            };
            surveySite.SiteBoundaries.AddRange(new[]
            {
                new SiteBoundary { Latitude = 1.1m, Longitude = 2.1m, VertexSequence = 1 },
                new SiteBoundary { Latitude = 1.2m, Longitude = 2.2m, VertexSequence = 2 },
                new SiteBoundary { Latitude = 1.3m, Longitude = 2.3m, VertexSequence = 3 },
                new SiteBoundary { Latitude = 1.4m, Longitude = 2.4m, VertexSequence = 4 },
                new SiteBoundary { Latitude = 1.5m, Longitude = 2.5m, VertexSequence = 5 }
            });
            surveySite.SurveyPoints.AddRange(new[]
            {
                new SurveyPoint { Latitude = 1.11m, Longitude = 2.11m, CodeName ="P_1_1" },
                new SurveyPoint { Latitude = 1.12m, Longitude = 2.12m, CodeName ="P_1_2" },
                new SurveyPoint { Latitude = 1.13m, Longitude = 2.13m, CodeName ="P_1_3" },
                new SurveyPoint { Latitude = 1.14m, Longitude = 2.14m, CodeName ="P_1_4" },
                new SurveyPoint { Latitude = 1.15m, Longitude = 2.15m, CodeName ="P_1_5" },
            });
            return surveySite;
        }
    }
}
