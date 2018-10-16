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

namespace ToGo
{
    /// <summary>
    /// Add_Hotel.xaml 的互動邏輯
    /// </summary>
    public partial class Add_Hotel : Window
    {
        public Add_Hotel()
        {
            InitializeComponent();

        }

        global::ToGo.ToGoEntities dbContext = new ToGo.ToGoEntities();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hotelViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hotelViewSource")));
            // 透過設定 CollectionViewSource.Source 屬性載入資料: 
            // hotelViewSource.Source = [泛用資料來源]
            hotelViewSource.Source = dbContext.Hotels.Local;

            //this.cityENNameComboBox.ItemsSource = this.dbContext.Countries.ToList();

            var q = dbContext.Countries.Select (c =>c.CountryENName).ToList();
            for (int i = 0; i < q.Count; i++)
            {
                this.countryENComboBox.Items.Add(q[i]);
            }

        }
        int countryno = 0;
        int cityno =0;
        internal void Button_Click(object sender, RoutedEventArgs e)
        {
            //Owner輸入資料後, 確定加入資料庫

            //this.countryComboBox1.ItemsSource = this.dbContext.Countries.ToList();

            //var country = dbContext.Countries.Where(c => c.CountryENName == ctryname).Select(ci => ci.CountryID).ToList();

            //var cityname = dbContext.Cities.Where(ci => ci.CountryID == ctryID).Select(ci => ci.CityENName);
      

            this.dbContext.Hotels.Local.Add(new ToGo.Hotel
            {
                //**TODO:輸入StarRated字串不正確, 跳出提示

                //HotelNameCN = hnCNTextBox.Text,
                //HotelNameEN = hnENTextBox.Text,
                //AddressCH = adrCHTextBox.Text,
                //AddressEN = adrENTextBox.Text,
                //RegisterDate =  DateTime.Now,
                //StarRated = int.Parse (starTextBox.Text),
                //TaxIDNumber =taxIDTextBox.Text,
                //Description = desTextBox.Text,
                CountryID =countryno,
                CityID = cityno

            });

            this.dbContext.SaveChanges();

            this.rgDateLabel.Content = DateTime.Now;
            //this.hotelDataGrid
        }

        string countryName = "";
        string cityName = "";
        //int ctryID=0;

        private void countryENComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var country = countryENComboBox.SelectedValue as Country;
            //countryID = country.CountryID;
            //this.countryENComboBox.Items.Clear();
            //this.cityENComboBox.Items.Clear();
            countryName = this.countryENComboBox.SelectedValue.ToString();

            //var q = dbContext.Countries.Where(c => c.CountryENName == ctryname).Select(ci => ci.CountryID).ToList();
            //for(int i=0; i<q.Count;i++)
            //{
            //    ctryID = q[i];
            //}
            var q = dbContext.Countries.Where(c => c.CountryENName == countryName).Select(co => co.CountryID).ToList();
            foreach(var item in q)
            {
                countryno = item;
            }

            var q1 = dbContext.Cities.Where(ci => ci.CountryID == countryno).Select(ci => ci.CityENName);
            foreach(var item in q1)
            {
                this.cityENComboBox.Items.Add(item);
            }
        }

        private void cityENComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.cityENComboBox.Items.Clear();
            cityName = this.cityENComboBox.SelectedValue.ToString();
            var q3 = dbContext.Cities.Where(ci => ci.CityENName == cityName).Select(ci => ci.CityID).ToList();
            foreach(var item in q3)
            {
                cityno = item;
            }
        }
    }
}
