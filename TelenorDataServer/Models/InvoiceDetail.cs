using MongoDB.Bson.Serialization.Attributes;

namespace TelenorDataServer.Models
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator("invoice_details")]
    class InvoiceDetail
    {
        public string Fakturanummer { get; set; }

        public string Fakturadato { get; set; }

        public string Abonnementets_eierkonto { get; set; }

        public string Eierkontoreferanse { get; set; }

        public string Konto_for_rollen_betalende_mobilnummer { get; set; }

        public string Mobilnummer { get; set; }

        public string Organisasjonsnummer { get; set; }

        public string Brukerreferanse { get; set; }

        public string Brukerreferanse2 { get; set; }

        public string Brukerreferanse3 { get; set; }

        public string Hovedgruppe { get; set; }

        public string Undergruppe { get; set; }

        public string Fakturatekst { get; set; }

        public string Fakturalinjestartdato { get; set; }

        public string Fakturalinjesluttdato { get; set; }

        public string Varighet { get; set; }

        public string Antall { get; set; }

        public string Volume { get; set; }

        public string Netto { get; set; }

        public string MVA { get; set; }

        public string MVA_prosent { get; set; }

        [BsonElement("file_date")]
        public string FileDate { get; set; }
    }
}
