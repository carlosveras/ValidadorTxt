using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ValidadorTxt
{
    public partial class Form1 : Form
    {

        string[] filePaths;
        List<string> list_of_string = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            formataWindows();
            removerBrancos();
            montaGrid();
            validarColunas();
            MessageBox.Show("Arquivo carregado com sucesso !!");
        }

        private void formataWindows()
        {
            string[] allLines = File.ReadAllLines(@"c:\FIDELIZACAO_ASSINATURA\FIDELIZACAO_ASSINATURA.txt");

            using (StreamWriter sw = new StreamWriter(@"c:\FIDELIZACAO_ASSINATURA\FIDELIZACAO_ASSINATURA.txt"))
            {
                foreach (string line in allLines)
                {
                    if (!string.IsNullOrEmpty(line) && line.Length > 1)
                    {
                        sw.WriteLine(line.Replace("\r\n", "\r"));
                    }
                }
            }
        }

        private void removerBrancos()
        {
            string[] allLines = File.ReadAllLines(@"c:\FIDELIZACAO_ASSINATURA\FIDELIZACAO_ASSINATURA.txt");

            using (StreamWriter sw = new StreamWriter(@"c:\FIDELIZACAO_ASSINATURA\FIDELIZACAO_ASSINATURA.txt"))
            {
                foreach (string line in allLines)
                {
                    if (!string.IsNullOrEmpty(line) && line.Length > 1)
                    {
                        sw.WriteLine(line.Replace(" ", string.Empty).Insert(line.Length, "|"));                      
                    }
                }
            }
        }

        private void montaGrid()
        {
            filePaths = Directory.GetFiles(@"c:\FIDELIZACAO_ASSINATURA\", "*.txt");

            foreach (string file in filePaths)
            {
                string[] lines = File.ReadAllLines(file);

                foreach (string line in lines)
                {
                    list_of_string.Add(line);
                    
                }
            };

            dgvDados.DataSource = Dados.LoadDadosListFromFile(list_of_string);
        }        

        private void validarColunas()
        {
            int indice = 0;
            dgvDados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            foreach (var item in dgvDados.Rows)
            {
                var cust = dgvDados.Rows[indice].Cells[0].Value.ToString();
                var dt_inicial = dgvDados.Rows[indice].Cells[1].Value.ToString();
                var dt_fim = dgvDados.Rows[indice].Cells[2].Value.ToString();
                var pai = dgvDados.Rows[indice].Cells[3].Value.ToString();

                string Header = checaDados(cust, dt_inicial, dt_fim, pai);

                if (!string.IsNullOrEmpty(Header))
                {
                    dgvDados.Rows[indice].Cells[0].Style.BackColor = Color.Yellow;
                    dgvDados.Rows[indice].Cells[1].Style.BackColor = Color.Yellow;
                    dgvDados.Rows[indice].Cells[2].Style.BackColor = Color.Yellow;
                    dgvDados.Rows[indice].Cells[3].Style.BackColor = Color.Yellow;
                    dgvDados.Rows[indice].Cells[4].Value = Header.ToString();
                    dgvDados.Rows[indice].Cells[4].Style.BackColor = Color.Yellow;
                }

                indice++;
            }
        }

        private string checaDados(string cust, string dt_inicial, string dt_fim, string pai)
        {
            string inconform = string.Empty;
            string padrao_data = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";

            if (string.IsNullOrEmpty(cust))
            {
                inconform = "Customer" ;
            }

            if (!string.IsNullOrEmpty(cust))
            {
                var match = System.Text.RegularExpressions.Regex.Match(cust, @"^\d+$");
                if (!match.Success)
                {
                    inconform += "Customer";
                }
            }

            //Valida data inicial

            if (string.IsNullOrEmpty(dt_inicial))
            {
                inconform += " / Data Inicial";
            }

            if (!string.IsNullOrEmpty(dt_inicial))
            {
                var match = System.Text.RegularExpressions.Regex.Match(dt_inicial, padrao_data);
                if (!match.Success)
                {
                    inconform += "Data Inicial";
                }
            }

            //Valida data final

            if (string.IsNullOrEmpty(dt_fim))
            {
                inconform += " / Data Fim";
            }

            if (!string.IsNullOrEmpty(dt_fim))
            {
                var match = System.Text.RegularExpressions.Regex.Match(dt_fim, padrao_data);
                if (!match.Success)
                {
                    inconform += " / Data Fim";
                }
            }

            //Valida Pai (S ou N)
            if (pai != "S" && pai != "N")
            {
                inconform += " / Pai";
            }

            return inconform;
        }

    }
}

