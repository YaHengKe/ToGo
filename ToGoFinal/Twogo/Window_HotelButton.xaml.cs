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
    /// Window_HotelButton.xaml 的互動邏輯
    /// </summary>
    public partial class Window_HotelButton : UserControl
    {


        public Window_HotelButton()
        {
            InitializeComponent();
        }

        public object DESC
        {
            get
            {
                return this.HotelName.Content;
            }
            set
            {
                this.HotelName.Content = value;
            }
        }


        public object price
        {
            get
            {
                return this.Lable_Price.Content;
            }
            set
            {
                this.Lable_Price.Content = value;
            }
        }

        public object City
        {
            get
            {
                return this.Lable_Address.Content;
            }

            set
            {
                this.Lable_Address.Content = value;
            }
        }
        private string _tempUrl;

        public string TempUrl
        {
            get { return _tempUrl; }
            set
            {
                _tempUrl = value;
            }
        }
        global::ToGo.ToGoEntities dbcontent = new ToGoEntities();

        private void Btn_Map_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(this.TempUrl);
            ToGoMAP tgm = new ToGoMAP();
            tgm.wbbrower1.Source = (uri);
            tgm.Show();
            //Uri uri = new Uri("https://www.google.com.tw/maps/place/Hotel+sardonyx+ueno/@35.7087171,139.7738073,17z/data=!3m1!4b1!4m7!3m6!1s0x60010784ddec2dcd:0x2db93a4e015c9ccd!5m1!1s2018-10-31!8m2!3d35.7087128!4d139.775996?hl=zh-TW&authuser=0");
            //wbbrower1.Source = (uri);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var q = this.dbcontent.Hotels.Where(x => x.HotelNameCN == HotelName.Content).Select(x => x.HotelID);
            int tempHotelID = 0;
            foreach (var item in q)
            {
                tempHotelID = item;
            }

            RoomMenu roomMenu = new RoomMenu(tempHotelID, MainWindow._SearchSDate, MainWindow._SearchEDate, MainWindow._RoomType.ToString());
            roomMenu.Show();
        }
    }
}
