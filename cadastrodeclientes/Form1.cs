using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace cadastrodeclientes
{
    public partial class frmCadastrodeClientes : Form
    {
        public frmCadastrodeClientes()
        {
            InitializeComponent();
        }

        // Validação Regex - para avisar o usuário caso esqueça de preencher algum elemento doseu e-mail
        private bool isValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
        // Função para validar se o cpf tem exatamente 11 digítos númericos.
        private bool isValidCPFLegth(string cpf)
        {
            // Remover quaisquer caracteres não númericos (como pontos e traços)
            cpf = cpf.Replace(".", "").Replace("-", "");
          
            // Verificar se o CPF tem exatamente 11 caracteres númericos
            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            return true;
        }


        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // valiadeção de campos obrigátorios
                if(string.IsNullOrEmpty(txtNomeCompleto.Text.Trim()) ||
                   string.IsNullOrEmpty(txtEmail.Text.Trim()) || // o Trim ( .Trim) elimina o espaço vazio de um campo (Ex: gui fran) o espaço não entraria na contagem de caracteres. 
                    string.IsNullOrEmpty(txtCPF.Text.Trim()))

                {
                    MessageBox.Show("Todos os campos devem ser preenchidos.",
                        "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return; //Impede o prosseguimento se algum campo estiver vazio
                }

                //validação do e-mail
                string email = txtEmail.Text.Trim();
                if (!isValidEmail(email))
                {
                    MessageBox.Show("Certifique-se de que o e-mail está no formato correto.",
                        "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return; // impede o prosseguimento se o e-mail for inválido.
                }
                //Validadção do CPF
                string cpf = txtCPF.Text.Trim();
                if (!isValidCPFLegth(cpf))
                {
                    MessageBox.Show("CPF inválido. Certifique-se de que o CPF tenha 11 digítos númericos",
                         "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return; //Impede o prosseguimento se algum campo estiver vazio
                }

            }
            catch (Exception ex)
            {
                //Trata outros ipos de erro
                MessageBox.Show("Ocorreu: " + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
        }
    }
}
