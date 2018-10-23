using System;
using System.Collections;
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
    /// RoomMenu.xaml 的互動邏輯
    /// </summary>
    public partial class RoomMenu : Window
    {
        //Source="Image/門口.JPG"
        //Source="Image/房間1.JPG"
        //Source="Image/房間2.JPG"
        //Source="Image/房間3.JPG"
        //Source="Image/招牌.JPG"



        public RoomMenu()
        {
            InitializeComponent();
        }
        global::ToGo.ToGoEntities dbContent = new ToGoEntities();

        Thickness marginnuber = new Thickness(5, 5, 5, 5);
        Thickness Roommarginnuber = new Thickness(5, 5, 5, 5);

        private DateTime SDate;
        private DateTime EDate;
        private string text;

        //宣告一個靜態變數 用來存HotelID 以便下訂單時能夠使用
        public static int _HotelID;
        //宣告一個List 用來存「可被訂」的RoomID集合 以便下訂單時能夠使用
        public static List<int> RoomIDCollection = new List<int>();


        public static List<HotelName_RN_qty_UnPrice> HN_RN_Qty_UnP = new List<HotelName_RN_qty_UnPrice>();

        public RoomMenu(object tag)
        {
            InitializeComponent();
        }

        int HotelIdNumber;
        public RoomMenu(object tag, DateTime SDatefromHotel, DateTime EDatefromHotel, string People) : this(tag)
        {
            this.SDate = SDatefromHotel;       //存從Hotel頁面丟過來的時間
            this.EDate = EDatefromHotel;       //存從Hotel頁面丟過來的時間
            this.text = People;                //存從Hotel頁面丟過來的人數


            //判斷有無重複,來決定要不要產生UserControl //
            List<string> list = new List<string>();

            //ComBeox 數量的產生 //
            List<string> listQuantity = new List<string>();

            HotelIdNumber = (int)tag;


            _HotelID = HotelIdNumber; //存HotelID值 其他表單才有辦法使用                                   


            var q = (from nr in dbContent.RoomInformations
                     where nr.HotelID == HotelIdNumber && nr.RoomType == MainWindow._RoomType
                     select new
                     {
                         nr.RoomID,
                         nr.RoomNameCN,
                         nr.PermitSmoking,
                         nr.OfferBreakfast,
                         nr.CanAddBed,
                         nr.UnitPrice,
                         nr.RoomType,
                         nr.Hotel.HotelNameCN,
                         nr.HotelID

                     }).Except(from x in dbContent.RoomInformations
                               from p in dbContent.Orders
                               from t in dbContent.OrderDetails
                               where x.HotelID == HotelIdNumber && x.RoomID == t.RoomID && p.OrderID == t.OrderID
                   && ((p.StartDate <= MainWindow._StartDate.Date && p.EndDate > MainWindow._StartDate.Date) || (p.StartDate <= MainWindow._EndDate.Date && p.EndDate > MainWindow._EndDate.Date))
                               select new
                               {
                                   x.RoomID,
                                   x.RoomNameCN,
                                   x.PermitSmoking,
                                   x.OfferBreakfast,
                                   x.CanAddBed,
                                   x.UnitPrice,
                                   x.RoomType,
                                   x.Hotel.HotelNameCN,
                                   x.HotelID
                               });

            var q3 = from n in dbContent.Hotels
                     where n.HotelID == HotelIdNumber
                     select new { n.HotelNameCN, n.HotelNameEN, n.AddressEN };

            foreach (var n in q3)  
            {
                titlename.Content = n.HotelNameCN + " ( " + n.HotelNameEN + " ) ";
                HotelAddress.Content = n.AddressEN;

            }

            foreach (var n in q)   //ComeBox房間數量用
            {
                listQuantity.Add(n.RoomNameCN);
            }

            foreach (var n in q)
            {
                bool flag = true;

                #region 產生房間的UserControls 的RoomPage 不重複
                for (int i = 0; i < list.Count(); i++)
                {
                    if (n.RoomNameCN == list[i])
                    {
                        flag = false;
                    }
                }
                list.Add(n.RoomNameCN);
                #endregion

                if (flag)
                {
                    int Quantity = 1;
                    RoomPage2 rp2 = new RoomPage2(n.HotelNameCN, n.HotelID);
                    rp2.Margin = marginnuber;
                    rp2.RoomName.Content = n.RoomNameCN;
                    rp2.RoomUnitPrice.Content = $"{n.UnitPrice:c0}";
                    rp2.RoomUnitPrice.Tag = n.UnitPrice;                     //暫存UnitPrice
                    rp2.HotelNameRoomPage.Tag = n.HotelNameCN;               //暫存HotelNameCN     
                    rp2.RoomNameRoomPage.Tag = n.RoomNameCN;                 //暫存RoomNameCN 丟給RoomInformationDetail
                    rp2.RoomIDRoomPage.Tag = n.HotelID;                      //暫存HotelID 丟給RoomInformationDetail



                    for (int i = 0; i < listQuantity.Count; i++)    //房間數量加入ComeBox中
                    {
                        if (rp2.RoomName.Content.ToString() == listQuantity[i].ToString())
                        {
                            rp2.QuantityComeBox.Items.Add(Quantity);
                            Quantity += 1;
                        }
                    }
                    rp2.QuantityComeBox.SelectedIndex = 0;


                    if (n.OfferBreakfast == true)
                        newlabel(ref rp2, n.RoomType, "人份早餐" /*"OfferBreakfast"*/);
                    if (n.PermitSmoking == true)
                        newlabel(ref rp2, "可吸菸" /*"PermitSmoking"*/);
                    if (n.CanAddBed == true)
                        newlabel(ref rp2, "可以加床" /*"CanAddBed"*/);

                    this.StackOfRoom.Children.Add(rp2);
                    Roommarginnuber.Top = 5;
                }

            }
            rb1.IsChecked = true;
        }

        private void newlabel(ref RoomPage2 rp2, string option)
        {
            Label label = new Label();
            label.SetValue(Grid.RowProperty, 1);
            label.SetValue(Grid.ColumnProperty, 1);
            label.Margin = Roommarginnuber;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Content = "■  " + option;

            rp2.SmallGrid.Children.Add(label);
            Roommarginnuber.Top += 30;
        }

        private void newlabel(ref RoomPage2 rp2, int? roomtype, string option)
        {
            Label label = new Label();
            label.SetValue(Grid.RowProperty, 1);
            label.SetValue(Grid.ColumnProperty, 1);
            label.Margin = Roommarginnuber;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Content = "■   " + roomtype + "   " + option;

            rp2.SmallGrid.Children.Add(label);
            Roommarginnuber.Top += 30;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Thickness hsfmarginnumber = new Thickness(5, 5, 5, 5);
            int a = 0;

            HSFusercontrol.servicename.Content = ((RadioButton)sender).Content;

            switch (HSFusercontrol.servicename.Content.ToString())
            {
                case "可使用語言":
                    a = 11;
                    break;
                case "網路服務":
                    a = 12;
                    break;
                case "休閒娛樂設施":
                    a = 13;
                    break;
                case "餐飲服務":
                    a = 14;
                    break;
                case "服務與便利設施":
                    a = 15;
                    break;
                case "接待服務":
                    a = 16;
                    break;
                case "交通服務/設施":
                    a = 17;
                    break;
                case "兒童專屬":
                    a = 18;
                    break;
            }
            HSFusercontrol.hsfgrid.Children.Clear();

            var q = from n in dbContent.ServiceAndFacilities
                    from p in n.HotelServiceAndFacilities
                    where p.HotelID == HotelIdNumber && n.ServiceFacilityID == p.ServiceFacilityID && n.ServiceFacilityID / 100 == a
                    select new { n.ServiceFacilityCHName };

            int newlabel = 0;
            foreach (var n in q)
            {
                Label label = new Label();
                label.Width = 230;
                label.Margin = hsfmarginnumber;
                label.Content = "■  " + n.ServiceFacilityCHName;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Left;

                this.HSFusercontrol.hsfgrid.Children.Add(label);
                hsfmarginnumber.Top += 20;
                newlabel += 1;
                if (newlabel % 7 == 0)
                {
                    hsfmarginnumber.Top = 5;
                    hsfmarginnumber.Left += 230;
                }
            }
        }
    }
    public class HotelName_RN_qty_UnPrice
    {
        public HotelName_RN_qty_UnPrice(string HN, string RN, int qty, int UPrice)
        {
            HotelName = HN;
            RoomName = RN;
            Quantity = qty;
            UnitPrice = UPrice;
        }
        public string HotelName;
        public string RoomName;
        public int Quantity;
        public int UnitPrice;

    }


}
