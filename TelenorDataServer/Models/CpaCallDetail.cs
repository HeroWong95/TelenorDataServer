using MongoDB.Bson.Serialization.Attributes;

namespace TelenorDataServer.Models
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator("cpa_call_details")]
    class CpaCallDetail
    {
        public string Faktura_nr { get; set; }
        public string Fakturadato { get; set; }
        public string Abonnementets_eierkonto { get; set; }
        public string Kontoreferanse { get; set; }
        public string Anropsnummer { get; set; }
        public string Brukerfornavn { get; set; }
        public string Brukeretternavn { get; set; }
        public string Brukerreferanse1 { get; set; }
        public string Brukerreferanse2 { get; set; }
        public string Brukerreferanse3 { get; set; }
        public string Hovedgruppe { get; set; }
        public string Undergruppe { get; set; }
        public string Fakturatekst { get; set; }
        public string B_Number { get; set; }
        public string Start { get; set; }
        public string Antall { get; set; }
        public string Volum { get; set; }
        public string Netto { get; set; }
        public string Mva { get; set; }
        public string Mva_Indikator { get; set; }

        [BsonElement("file_date")]
        public string FileDate { get; set; }
    }
}
