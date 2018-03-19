using System;
using System.Collections.Generic;
using System.Text;
using TelenorDataServer.Models;

namespace TelenorDataServer.Impoter
{
    class InvoiceDetailImporter : TelenorImporter<InvoiceDetail>
    {
        public InvoiceDetailImporter(string fileDate) : base(fileDate) { }

        protected override string CollectionName => "invoice_details";
        protected override InvoiceDetail Fetch(string data)
        {
            string[] props = data.Split('¤');
            return new InvoiceDetail
            {
                Fakturanummer = props[0],
                Fakturadato = props[1],
                Abonnementets_eierkonto = props[2],
                Eierkontoreferanse = props[3],
                Konto_for_rollen_betalende_mobilnummer = props[4],
                Mobilnummer = props[5],
                Organisasjonsnummer = props[6],
                Brukerreferanse = props[7],
                Brukerreferanse2 = props[8],
                Brukerreferanse3 = props[9],
                Hovedgruppe = props[10],
                Undergruppe = props[11],
                Fakturatekst = props[12],
                Fakturalinjestartdato = props[13],
                Fakturalinjesluttdato = props[14],
                Varighet = props[15],
                Antall = props[16],
                Volume = props[17],
                Netto = props[18],
                MVA = props[19],
                MVA_prosent = props[20],
                FileDate = FileDate
            };
        }
    }
}
