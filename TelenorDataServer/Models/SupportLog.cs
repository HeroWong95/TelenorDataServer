using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TelenorDataServer.Models
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator("support_log")]
    class SupportLog
    {
        [BsonElement("file_date")]
        public string FileDate { get; set; }

        [BsonElement("filename")]
        public string FileName { get; set; }

        [BsonElement("start_time")]
        public DateTime StartTime { get; set; }

        [BsonElement("source_lines")]
        public int? SourceLine { get; set; }

        [BsonElement("insert_lines")]
        public int? InsertLine { get; set; }

        [BsonElement("end_time")]
        public DateTime EndTime { get; set; }
    }
}
