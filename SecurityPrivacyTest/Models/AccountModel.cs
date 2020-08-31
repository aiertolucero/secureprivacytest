using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SecurityPrivacyTest.Models
{
    public class AccountModel
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("ip")]
        public string Ip { get; set; }

        [BsonElement("os")]
        public string OS { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("binary")]
        public string Binary { get; set; }
    }
}