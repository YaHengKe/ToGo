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
    /// RoomPage2.xaml 的互動邏輯
    /// </summary>
    public partial class RoomPage2 : UserControl
    {
        public RoomPage2(string roomNameCn, int? hotelID) //加入一個建構子方法 當這個控制向被new 出來的時候 用Linq去找他的3張圖片
        {
            InitializeComponent();
            var q = this.dbContext.RoomImage2.Where(x => x.RoomNameCN == roomNameCn && x.HotelID == hotelID).Select(x => x.Image);

            int i = 0; //變數i 用來判斷值是否為1
            foreach (var item in q)
            {
                if (i == 0) //若i 變數的值為1 就把RoomImage2裡面的第一張圖片當作最大的圖
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
                i += 1;
            }

            this.cancellabel.Content = DateTime.Now.AddDays(4).ToShortDateString() + " 之前";
        }
        public static int _Quantity;
        

        global::ToGo.ToGoEntities dbContext = new ToGoEntities();//加入dbContext

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            RoomMenu.HN_RN_Qty_UnP.Add(new HotelName_RN_qty_UnPrice(HotelNameRoomPage.Tag.ToString(), RoomNameRoomPage.Tag.ToString(), int.Parse(QuantityComeBox.SelectedValue.ToString()),(int)RoomUnitPrice.Tag));
            #region  To 亞衡的資料
            

            var q = (from nr in dbContext.RoomInformations
                     where nr.HotelID == RoomMenu._HotelID && nr.RoomType == MainWindow._RoomType && nr.UnitPrice == (int)RoomUnitPrice.Tag && nr.RoomNameCN == RoomName.Content.ToString()//加入判斷式 價錢和房名相符
                     select new
                     {
                         nr.RoomID                        

                     }).Except(from x in dbContext.RoomInformations
                               from p in dbContext.Orders
                               from t in dbContext.OrderDetails
                               where x.HotelID == RoomMenu._HotelID && x.RoomID == t.RoomID && p.OrderID == t.OrderID
                   && ((p.StartDate <= MainWindow._StartDate.Date && p.EndDate > MainWindow._StartDate.Date) || (p.StartDate <= MainWindow._EndDate.Date && p.EndDate > MainWindow._EndDate.Date))
                               select new
                               {
                                   x.RoomID
                                  
                               }).ToList(); //先ToList 下面用的是for迴圈取「部分RoomID」

            //用for迴圈將RoomID 不逐一加入靜態集合(RoomIDCollection) ，只加入qty 個   
            for (int i = 0; i < (int)QuantityComeBox.SelectedItem; i++)
            {
                RoomMenu.RoomIDCollection.Add(int.Parse(q[i].RoomID.ToString()));
            }
            
            ((Button)sender).IsEnabled = false;   //只限點選一次
            ((Button)sender).Content = "已下訂";  //顯示出已下訂

            Payment.RoomNameType += 1;
            YesOrNo y = new YesOrNo();
            y.ShowDialog();//.................................
            #endregion

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RoomInformationDetail r = new RoomInformationDetail(RoomNameRoomPage.Tag, RoomIDRoomPage.Tag);
            r.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void QuantityComeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_Quantity = (int)QuantityComeBox.SelectedItem;
        }
    }
}
