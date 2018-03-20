using System;
using System.Collections.Generic;
using System.Text;
using TelenorDataServer.Models;

namespace TelenorDataServer.Impoter
{
    class CpaCallDetailImporter : TelenorImporter<CpaCallDetail>
    {
        public CpaCallDetailImporter(string fileDate) : base(fileDate) { }

        protected override string CollectionName => "cpa_call_details";

        protected override CpaCallDetail Fetch(string data)
        {
            string[] props = data.Split('¤');
            return new CpaCallDetail
            {
                Faktura_nr = props[0],
                Fakturadato = props[1],
                Abonnementets_eierkonto = props[2],
                Kontoreferanse = props[3],
                Anropsnummer = props[4],
                Brukerfornavn = props[5],
                Brukeretternavn = props[6],
                Brukerreferanse1 = props[7],
                Brukerreferanse2 = props[8],
                Brukerreferanse3 = props[9],
                Hovedgruppe = props[10],
                Undergruppe = props[11],
                Fakturatekst = props[12],
                B_Number = props[13],
                Start = props[14],
                Antall = props[15],
                Volum = props[16],
                Netto = props[17],
                Mva = props[18],
                Mva_Indikator = props[19],
                FileDate = FileDate
            };
        }
    }
}
