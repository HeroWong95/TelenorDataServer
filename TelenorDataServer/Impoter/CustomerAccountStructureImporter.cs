using System;
using System.Collections.Generic;
using System.Text;
using TelenorDataServer.Models;

namespace TelenorDataServer.Impoter
{
    class CustomerAccountStructureImporter : TelenorImporter<CustomerAccountStructure>
    {
        public CustomerAccountStructureImporter(string fileDate) : base(fileDate) { }

        protected override string CollectionName => "customer_account_structure";

        protected override CustomerAccountStructure Fetch(string data)
        {
            string[] props = data.Split('¤');
            return new CustomerAccountStructure
            {
                Organisasjonsnummer = props[0],
                Kundenavn = props[1],
                brukerfornavn = props[2],
                brukeretternavn = props[3],
                kontonummer_i_Telenor = props[4],
                kontonavn = props[5],
                kontostatus = props[6],
                bruker_adresse = props[7],
                bruker_postnummer = props[8],
                bruker_postadresse = props[9],
                Anropsnummer = props[10],
                Abonnementstype = props[11],
                Abonnementetsstartdato = props[12],
                Abonnementetsreferanse = props[13],
                Prisplan_for_datapakke = props[14],
                Abonnementsstatus = props[15],
                Binding = props[16],
                Binding_startdato = props[17],
                Binding_sluttdato = props[18],
                Datapakke_binding_startdato = props[19],
                Datapakke_binding_sluttdato = props[20],
                FileDate = FileDate
            };
        }
    }
}
