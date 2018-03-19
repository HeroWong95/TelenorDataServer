using MongoDB.Bson.Serialization.Attributes;

namespace TelenorDataServer.Models
{
    [BsonIgnoreExtraElements]
    class CustomerAccountStructure
    {
        public string Organisasjonsnummer { get; set; }

        public string Kundenavn { get; set; }

        public string brukerfornavn { get; set; }

        public string brukeretternavn { get; set; }

        public string kontonummer_i_Telenor { get; set; }

        public string kontonavn { get; set; }

        public string kontostatus { get; set; }

        public string bruker_adresse { get; set; }

        public string bruker_postnummer { get; set; }

        public string bruker_postadresse { get; set; }

        public string Anropsnummer { get; set; }

        public string Abonnementstype { get; set; }

        public string Abonnementetsstartdato { get; set; }

        public string Abonnementetsreferanse { get; set; }

        public string Prisplan_for_datapakke { get; set; }

        public string Abonnementsstatus { get; set; }

        public string Binding { get; set; }

        public string Binding_startdato { get; set; }

        public string Binding_sluttdato { get; set; }

        public string Datapakke_binding_startdato { get; set; }

        public string Datapakke_binding_sluttdato { get; set; }

        [BsonElement("file_date")]
        public string FileDate { get; set; }
    }
}
