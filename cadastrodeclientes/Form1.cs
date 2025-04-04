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
using MySql.Data.MySqlClient;


namespace cadastrodeclientes
{
    public partial class frmCadastrodeClientes : Form
    {
        //Conexão com o banco de dados MySQL
        MySqlConnection Conexao;
        string data_source = "datasource=localhost; username=root; password=; database=db_cadastro";
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

                //Cria a conexão com o banco de dados
                Conexao = new MySqlConnection(data_source);
                Conexao.Open();

                //Teste de abertura de banco de dados
               // MessageBox.Show("Conexão aberta com sucesso");

                //Comando SQL para inserir um cliente no banco de dados
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = Conexao
                };

                cmd.Prepare();

                cmd.CommandText = "INSERT INTO dadosdecliente(nomecompleto, nomesocial, email, cpf)" +
                    "VALUES (@nomecompleto, @nomesocial, @email, @cpf)";

                //Adiciona os parâmetros com os dados do formulário
                cmd.Parameters.AddWithValue("@nomecompleto", txtNomeCompleto.Text.Trim());
                cmd.Parameters.AddWithValue("@nomesocial", txtNomeSocial.Text.Trim());
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@cpf", cpf);

                // Executa o comando de Inserção no banco
                cmd.ExecuteNonQuery();

                // mensagem de sucesso
                MessageBox.Show("Contato Inserido com sucesso: ",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            }
            catch (MySqlException ex )
            {
                //Trata erros relacionados ao MySQL
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                //Trata outros tipos de erro * erros no backEnd do formulário
                MessageBox.Show("Ocorreu: " + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            finally
            {
                //Garante que a conexão com o banco será fechada, mesmo se ocorrer erro
                if(Conexao != null && Conexao.State == ConnectionState.Open)
                {
                    Conexao.Close();

                    //Teste de fechamento do banco
                    //MessageBox.Show("Conexão fechada com sucesso");
                }
            }
        }
    }
}
