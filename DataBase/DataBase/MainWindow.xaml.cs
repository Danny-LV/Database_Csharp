using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBase
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlDataAdapter adapter;
        MySqlConnection conn = null;
        DataTable datatable;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            datatable = new DataTable();
            datatable.Clear();

            string query = tb_query.Text;
            string connStr = tb_conn.Text;

            try
            {
                conn = new MySqlConnection(connStr); // Устанавливаем соединение
                conn.Open(); // Открываем соединение
                MySqlCommand cmd = new MySqlCommand  // Создаем SQL команду
                {
                    Connection = conn, // Указываем по какому подключению
                    CommandText = query // Указываем SQL текст запроса
                };
                cmd.ExecuteNonQuery(); // Запускаем команду  на выполнение
                adapter = new MySqlDataAdapter(cmd); // По команде создаем адаптер
                adapter.Fill(datatable); // С помощью адаптера получаем данные в таблицу
                /*DataRow newRow = datatable.NewRow();
                newRow["ID"] = "9";
                newRow["NAME"] = "DС";
                newRow["FNAME"] = "bluebeetle";
                newRow["AGE"] = 70;
                datatable.Rows.Add(newRow);*/
                grd.ItemsSource = datatable.AsDataView(); //Преобразуем в DataView
                // Выводим в грид
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                adapter.Update(datatable);
                lb_status.Content = $"Successfully received {datatable.Rows.Count} lines";
            }catch (Exception ex)
            {
                lb_status.Content = $"Error {ex.Message}";
            }
            finally
            {
                if (conn != null) conn.Close(); // Закрыть соединение
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string connStr = tb_conn.Text;
            try
            {
                conn = new MySqlConnection(connStr); // Устанавливаем соединение
                conn.Open(); // Открываем соединение

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                adapter.Update(datatable);
            }catch (Exception ex)
            {
                lb_status.Content = $"Error {ex.Message}";
            }
            finally
            {
                if (conn != null) conn.Close(); // Закрыть соединение
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataTable table = new DataTable();
            listTable.Items.Clear(); // Cleans
            string connStr = tb_conn.Text;
            try
            {
                conn = new MySqlConnection(connStr); // Устанавливаем соединение
                conn.Open(); // Открываем соединение
                string query = "SHOW TABLES";
                MySqlCommand cmd = new MySqlCommand // Создаем SQL команду
                {
                    Connection = conn, // Указываем по какому подключению
                    CommandText = query // Указываем SQL текст запроса
                };
                cmd.ExecuteNonQuery(); // Запускаем команду на выполнение
                MySqlDataAdapter adapter1 = new MySqlDataAdapter(cmd); // По команде создаем адаптер
                adapter1.Fill(table);
                foreach (DataRow dr in table.Rows)
                {
                    listTable.Items.Add(dr[0]);
                }
            }catch (Exception ex)
            {
                lb_status.Content = $"Error {ex.Message}";
            }
            finally
            {
                if (conn != null) conn.Close(); // Закрыть соединение
            }
        }

        private void listTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listTable.SelectedItem == null) return;
            string selection = listTable.SelectedItem.ToString();
            tb_query.Text = "SELECT * FROM " + selection;
        }

    }
}
