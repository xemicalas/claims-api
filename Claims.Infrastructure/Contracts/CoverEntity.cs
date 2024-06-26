﻿using Claims.Domain.Contracts;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Infrastructure.Contracts
{
    public class CoverEntity
    {
        [BsonId]
        public required string Id { get; set; }

        [BsonElement("startDate")]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime EndDate { get; set; }

        [BsonElement("claimType")]
        public CoverType Type { get; set; }

        [BsonElement("premium")]
        public decimal Premium { get; set; }
    }
}

