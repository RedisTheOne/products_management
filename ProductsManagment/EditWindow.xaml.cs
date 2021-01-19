using System;
using System.Collections.Generic;
using System.Linq;
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
using ProductsManagment.models;

namespace ProductsManagment
{
    public partial class EditWindow : Window
    {
        private string connectionString = "Data Source=DESKTOP-604RL6A;Initial Catalog=Users;Integrated Security=True;Pooling=False";
        private Product product;
        private string username;
        public EditWindow(Product product, string username)
        {
            InitializeComponent();
            this.username = username;
            this.product = product;
            titleTextBox.Text = product.title;
            priceTextBox.Text = product.price.ToString();            
        }

        private void goHomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(username);
            mw.Show();
            this.Close();
        }

        private void editProductButton_Click(object sender, RoutedEventArgs e)
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
                        string sql = "UPDATE Products SET title=@param1, price=@param2 WHERE Id=@param3";
                        using (SqlCommand sqlCommand = new SqlCommand(sql, con))
                        {
                            sqlCommand.Parameters.Add("@param1", SqlDbType.VarChar, 50).Value = title;
                            sqlCommand.Parameters.Add("@param2", SqlDbType.Int).Value = price;
                            sqlCommand.Parameters.Add("@param3", SqlDbType.Int).Value = product.Id;
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.ExecuteNonQuery();
                        }
                        con.Close();
                        MainWindow mw = new MainWindow(username);
                        mw.Show();
                        this.Close();
                    }
                }
                else
                    MessageBox.Show("Please enter a valid title");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid price");
            }
        }
    }
}
