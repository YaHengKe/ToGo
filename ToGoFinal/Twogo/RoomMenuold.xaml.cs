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

        public RoomMenu(object tag, DateTime SDatefromHotel, DateTime EDatefromHotel, string People) : this(tag)
        {
            this.SDate = SDatefromHotel;       //存從Hotel頁面丟過來的時間
            this.EDate = EDatefromHotel;       //存從Hotel頁面丟過來的時間
            this.text = People;                //存從Hotel頁面丟過來的人數


            //判斷有無重複,來決定要不要產生UserControl //
            List<string> list = new List<string>();

            //ComBeox 數量的產生 //
            List<string> listQuantity = new List<string>();
           
            int HotelIdNumber = (int)tag;


            _HotelID = HotelIdNumber; //存HotelID值 其他表單才有辦法使用                                   


            var q = (from nr in dbContent.RoomInformations
                     where nr.HotelID == HotelIdNumber && nr.RoomType==MainWindow._RoomType
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
                    && ((p.StartDate <=MainWindow._StartDate.Date && p.EndDate < MainWindow._StartDate.Date) || (p.StartDate <= MainWindow._EndDate.Date && p.EndDate<MainWindow._EndDate.Date) )
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
            

            foreach (var n in q)    //暫時確認搜出的RoomID
            {
                titlename.Content = n.HotelNameCN;
                //將篩選條件的結果(roomID)逐一加入至RoomIDCollection集合裡
                //RoomIDCollection.Add(n.RoomID);
                //unitPrice 傳給下一個頁面
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
                    RoomPage2 rp2 = new RoomPage2();
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
       
     }
   public class HotelName_RN_qty_UnPrice
    {
        public HotelName_RN_qty_UnPrice(string HN,string RN,int qty,int UPrice)
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
