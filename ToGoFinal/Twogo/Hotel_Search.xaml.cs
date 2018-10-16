using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace ToGo
{
    /// <summary>
    /// Hotel_Search.xaml 的互動邏輯
    /// </summary>
    public partial class Hotel_Search : Window
    {
        public Hotel_Search()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //CMLogin w = new CMLogin();
            //w.Show();

        }
        

        
       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=ToGoDB;Integrated Security=True"))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.Connection = conn;

            //        cmd.CommandText = "select Hotel.HotelNameCN,City.CityCHName,MIN( RoomInformation.UnitPrice )from RoomInformation join Hotel on Hotel.HotelID = RoomInformation.HotelID join City on Hotel.CityID = City.CityID " +
            //            "group by Hotel.HotelNameCN,City.CityCHName having City.CityCHName = @SearchCity";

            //        cmd.Parameters.Add("@SearchCity", SqlDbType.NVarChar).Value = Search_CityTextBox.Text;
            //        conn.Open();
            //        Hotel_Search ww = new Hotel_Search();
            //        using (SqlDataReader dr = cmd.ExecuteReader())
            //        {
            //            int a = 0;//搜尋出的資料筆數
            //            while (dr.Read())
            //            {
            //                Window_HotelButton xx = new Window_HotelButton();
            //                xx.DESC = dr["HotelNameCN"];
            //                xx.City = dr["CityCHName"];
            //                xx.price = dr[2];
            //                ww.StackPanel_ShowHotel.Children.Add(xx);
            //                a++;
            //            }
            //            ww.Label_HotelCount.Content = a;
            //        }
            //        this.Close();
            //        ww.Show();
            //    }
            //}
            //List<int> RoomIdCollection = new List<int>();
            //List<int> IsOrderedRoom = new List<int>();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //MainWindow w = new MainWindow();
            //w.Show();
            //MainWindow mainWindow = new MainWindow();
            //this.Close();
            //mainWindow.Show();

        }
    }
}
