using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TelenorDataServer.Models;

namespace TelenorDataServer.Impoter
{
    class ActiveSimCardDetailImpoter : TelenorImporter<ActiveSimCardDetail>
    {
        public ActiveSimCardDetailImpoter(string fileDate) : base(fileDate) { }

        protected override string CollectionName => "active_sim_card_details_test";

        protected override ActiveSimCardDetail Fetch(string data)
        {
            string[] props = data.Split('¤');
            return new ActiveSimCardDetail
            {
                Mobilnummer = props[0],
                Hovednummer_datakort1 = props[1],
                Hovednummer_datakort2 = props[2],
                Tvillingnummer = props[3],
                Mobilnummer_sist_brukt_dato = props[4],
                Produsent_handset = props[5],
                Mobilnummer_håndset = props[6],
                Produsent_datakort1_handset = props[7],
                Hovednummer_datakort1_håndset = props[8],
                Produsent_datakort2_handset = props[9],
                Hovednummer_datakort2_håndset = props[10],
                Produsent_Tvilling_handset = props[11],
                Tvilling_håndset = props[12],
                FileDate = FileDate
            };
        }
    }
}
