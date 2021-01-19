using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductsManagment
{
    public class LogInResponse
    {
        public bool valid;
        public string username;

        public LogInResponse(bool valid, string username)
        {
            this.valid = valid;
            this.username = username;
        }
    }
    public partial class LogInWindow : Window
    {
        private string connectionString = "Data Source=DESKTOP-604RL6A;Initial Catalog=Users;Integrated Security=True;Pooling=False";
        public LogInWindow()
        {
            InitializeComponent();
        }

        private LogInResponse UserLogIn(string username, string password)
        {
            bool userExists = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM Users WHERE username = '" + username + "' AND password = '" + password + "'", con);
                cmd.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    userExists = true;
                con.Close();
            }
            return new LogInResponse(userExists, username);
        }

        private void loginTextBox_Click(object sender, RoutedEventArgs e)
        {
            var response = UserLogIn(usernameTextBox.Text, passwordTextBox.Password);
            if (response.valid)
            {
                byte[] data = Encoding.ASCII.GetBytes(response.username);
                File.WriteAllBytes("./data.bin", data);
                MainWindow mw = new MainWindow(response.username);
                mw.Show();
                this.Close();
            }
            else
                MessageBox.Show("Please enter a valid user");
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow rw = new RegisterWindow();
            rw.Show();
            this.Close();
        }
    }
}
