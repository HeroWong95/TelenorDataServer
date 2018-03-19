using MongoDB.Bson.Serialization.Attributes;

namespace TelenorDataServer.Models
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator("active_sim_card_details")]
    class ActiveSimCardDetail
    {
        public string Mobilnummer { get; set; }
        public string Hovednummer_datakort1 { get; set; }
        public string Hovednummer_datakort2 { get; set; }
        public string Tvillingnummer { get; set; }
        public string Mobilnummer_sist_brukt_dato { get; set; }
        public string Produsent_handset { get; set; }
        public string Mobilnummer_håndset { get; set; }
        public string Produsent_datakort1_handset { get; set; }
        public string Hovednummer_datakort1_håndset { get; set; }
        public string Produsent_datakort2_handset { get; set; }
        public string Hovednummer_datakort2_håndset { get; set; }
        public string Produsent_Tvilling_handset { get; set; }
        public string Tvilling_håndset { get; set; }

        [BsonElement("file_date")]
        public string FileDate { get; set; }
    }
}
