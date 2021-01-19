using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductsManagment
{
    public class AddUserResponse
    {
        public bool created;
        public string username;
        public AddUserResponse(bool created, string username)
        {
            this.created = created;
            this.username = username;
        }
    }

    public partial class RegisterWindow : Window
    {
        private string connectionString = "Data Source=DESKTOP-604RL6A;Initial Catalog=Users;Integrated Security=True;Pooling=False";
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow lw = new LogInWindow();
            lw.Show();
            this.Close();
        }

        private AddUserResponse AddUser(string username, string password, string name, string surname)
        {
            bool created = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                //CHECK IF USERNAME IS TAKEN
                DataSet ds = new DataSet();
                SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM Users WHERE username = '" + username + "'", con);
                cmd.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    MessageBox.Show("Username is already taken");
                else
                {
                    string sql = "INSERT INTO Users(username, password, name, surname) VALUES(@param1, @param2, @param3, @param4)";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, con))
                    {
                        sqlCommand.Parameters.Add("@param1", SqlDbType.VarChar, 50).Value = username;
                        sqlCommand.Parameters.Add("@param2", SqlDbType.VarChar, 50).Value = password;
                        sqlCommand.Parameters.Add("@param3", SqlDbType.VarChar, 50).Value = name;
                        sqlCommand.Parameters.Add("@param4", SqlDbType.VarChar, 50).Value = surname;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                        created = true;
                    }
                }
                con.Close();
                return new AddUserResponse(created, username);
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(usernameTextBox.Text) &&
                !String.IsNullOrWhiteSpace(passwordTextBox.Password) &&
                !String.IsNullOrWhiteSpace(nameTextBox.Text) &&
                !String.IsNullOrWhiteSpace(surnameTextBox.Text))
            {
                var response = AddUser(usernameTextBox.Text, passwordTextBox.Password, nameTextBox.Text, surnameTextBox.Text);
                if (response.created)
                {
                    MainWindow mf = new MainWindow(response.username);
                    mf.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Username already exists");
            } else
                MessageBox.Show("Please fill required fields");
        }
    }
}
