using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SecurityPrivacyTest.Services
{
    public class CRUDService
    {
        private IMongoDatabase db;

        public CRUDService()
        {
            var mongoClient = new MongoClient(ConfigurationManager.AppSettings["mongoDBHost"]);
            db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"]);
        }

        public void InsertAccount<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
        }

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            var result = collection.ReplaceOne(
                filter,
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecords<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}