using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoExperiments.CS.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoExperiments.CS
{
    public class MongoDal<T>
    {
        private readonly IMongoClient _client;
        private readonly string _databaseName;


        private IMongoCollection<T> Collection
        {
            get
            {

                var database = _client.GetDatabase(_databaseName);

                var collectionName = typeof(T).Name.ToString();
                var collection = database.GetCollection<T>(collectionName);

                return collection;
            }
        }


        public MongoDal(IMongoClient client, string databaseName)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            if (databaseName == null)
            {
                throw new ArgumentNullException(nameof(databaseName));
            }
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException(nameof(databaseName) + " cannot be whitespace", nameof(databaseName));
            }

            _client = client;
            _databaseName = databaseName;

            BsonClassMap.RegisterClassMap<T>();
        }

        public async Task Insert(T site)
        {
            await Collection.InsertOneAsync(site);
        }

        public async Task Insert(IEnumerable<T> sites)
        {
            await Collection.InsertManyAsync(sites);
        }

        public async Task<List<T>> Select(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public async Task Delete(Expression<Func<T, bool>> predicate)
        {
            await Collection.DeleteManyAsync(predicate);
        }
    }

    public class SurveySiteRepositoryMongo : MongoDal<SurveySite>
    {
        public SurveySiteRepositoryMongo(IMongoClient client, string databaseName) : base(client, databaseName)
        {
        }
    }
}
