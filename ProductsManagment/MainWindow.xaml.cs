using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ProductsManagment.models;
using System.IO;

namespace ProductsManagment
{
    public partial class MainWindow : Window
    {
        private int userId;
        private string username;
        private List<Product> products = new List<Product>();
        private string connectionString = "Data Source=DESKTOP-604RL6A;Initial Catalog=Users;Integrated Security=True;Pooling=False";
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                string[] lines = File.ReadAllLines("./data.bin");
                if (lines.Length == 0)
                {
                    LogInWindow lW = new LogInWindow();
                    this.Close();
                    lW.Show();
                }
                else
                {
                    FetchUserInfo(lines[0]);
                    FetchProducts();
                }
            }   catch(Exception)
            {
                LogInWindow lW = new LogInWindow();
                this.Close();
                lW.Show();
            }
        }
        public MainWindow(string username)
        {
            InitializeComponent();
            FetchUserInfo(username);
            FetchProducts();
        }

        private void FetchProducts()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter cmd = new SqlDataAdapter("select * from products", con);
                cmd.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    productsListBox.Items.Add(row["title"]);
                    products.Add(new Product(Convert.ToInt32(row["Id"]), Convert.ToInt32(row["price"]), row["title"].ToString()));              
                }
                con.Close();
            }

        }
        private void FetchUserInfo(string username)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter cmd = new SqlDataAdapter("select * from users where username = '" + username + "'", con);
                cmd.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    usernameTextBlock.Text = "Username: " + username;
                    nameTextBlock.Text = "Name: " + row["name"];
                    surnameTextBlock.Text = "Surname: " + row["surname"];
                    userId = Convert.ToInt32(row["Id"]);
                    this.username = username;
                }
                con.Close();
            }
        }

        private void signOutButton_Click(object sender, RoutedEventArgs e)
        {
            File.Delete("./data.bin");
            LogInWindow lw = new LogInWindow();
            lw.Show();
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow aw = new AddProductWindow(userId, username);
            aw.Show();
            this.Close();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = productsListBox.SelectedIndex;
            if (selectedIndex != -1)
            {
                Product product = products[selectedIndex];
                EditWindow ew = new EditWindow(product, username);
                ew.Show();
                this.Close();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = productsListBox.SelectedIndex;
            if (selectedIndex != -1)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "DELETE FROM Products WHERE Id=@param1";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, con))
                    {
                        sqlCommand.Parameters.Add("@param1", SqlDbType.Int, 50).Value = products[selectedIndex].Id;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    con.Close();
                }
                productsListBox.Items.Clear();
                products.RemoveAt(selectedIndex);
                foreach(var product in products)
                {
                    productsListBox.Items.Add(product.title);
                }
            }
        }


        private void productsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = productsListBox.SelectedIndex;
            productTitle.Text = "Title: " + products[selectedIndex].title;
            productPrice.Text = "Price: " + products[selectedIndex].price.ToString();
        }
    }
}
