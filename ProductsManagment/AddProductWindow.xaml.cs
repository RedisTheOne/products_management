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
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private int userId;
        private string username;
        private string connectionString = "Data Source=DESKTOP-604RL6A;Initial Catalog=Users;Integrated Security=True;Pooling=False";
        public AddProductWindow(int userId, string username)
        {
            InitializeComponent();
            this.userId = userId;
            this.username = username;
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = titleTextBox.Text;
                int price = Convert.ToInt32(priceTextBox.Text);
                if (!String.IsNullOrWhiteSpace(title))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string sql = "INSERT INTO Products(user_id, title, price) VALUES(@param1, @param2, @param3)";
                        using (SqlCommand sqlCommand = new SqlCommand(sql, con))
                        {
                            sqlCommand.Parameters.Add("@param1", SqlDbType.Int, 50).Value = userId;
                            sqlCommand.Parameters.Add("@param2", SqlDbType.VarChar, 50).Value = title;
                            sqlCommand.Parameters.Add("@param3", SqlDbType.Int, 50).Value = price;
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.ExecuteNonQuery();
                        }
                        con.Close();
                        MainWindow mw = new MainWindow(username);
                        mw.Show();
                        this.Close();
                    }
                } else
                    MessageBox.Show("Please enter a valid title");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid price");
            }
        }

        private void goHomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(username);
            mw.Show();
            this.Close();
        }
    }
}
