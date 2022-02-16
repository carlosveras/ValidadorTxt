using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidadorTxt
{
    public class Dados
    {
        public string CUSTOMER { get; set; }
        public string DT_INICIAL { get; set; }
        public string Dt_FIM { get; set; }
        public string PAI { get; set; }
        public string INCONFORMIDADES { get; set; }
        

        internal static List<Dados> LoadDadosListFromFile(List<string> p)
        {
            var qDados = new List<Dados>();
            int qLinha = 0;

            foreach (var line in p)
            {
                var columns = line.Split('|');
                if (qLinha > 0)
                {
                    qDados.Add(new Dados
                    {
                        CUSTOMER = columns[0],
                        DT_INICIAL = columns[1],
                        Dt_FIM = columns[2],
                        PAI = columns[3],
                        INCONFORMIDADES = columns[4]
                        
                        
                    });
                }
                qLinha++;
            }
            return qDados;
        }
              
    }
}
