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


        public Window_HotelButton(int _hotelID) //加入一個建構子方法 當這個控制向被new 出來的時候 用Linq去找他的3張圖片
        {
            InitializeComponent();

            var q = this.dbContext.HotelImages.Where(x => x.HotelID == _hotelID).Select(x => x.Image);

            int i = 0; //變數i 用來判斷值是否為1
            foreach (var item in q)
            {
                if (i == 0) //若i 變數的值為1 就把hotelImage裡面的第一張圖片當作最大的圖
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(item);
                    bi3.StreamSource = ms;
                    bi3.EndInit();
                    this.image1.Source = bi3;
                    //直接將剛剛轉好的圖檔 當作Image1的Source
                }
                else if (i == 1) //其餘圖片皆為小圖
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(item);
                    bi3.StreamSource = ms;
                    bi3.EndInit();
                    this.image2.Source = bi3;
                }
                else if (i == 2)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(item);
                    bi3.StreamSource = ms;
                    bi3.EndInit();
                    this.image3.Source = bi3;
                }
                else if (i == 3)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(item);
                    bi3.StreamSource = ms;
                    bi3.EndInit();
                    this.image4.Source = bi3;
                }
                i += 1;
            }

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
        global::ToGo.ToGoEntities dbContext = new ToGoEntities();

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
            var q = this.dbContext.Hotels.Where(x => x.HotelNameCN == HotelName.Content).Select(x => x.HotelID);
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
