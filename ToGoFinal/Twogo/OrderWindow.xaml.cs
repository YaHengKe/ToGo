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
    /// testbuy.xaml 的互動邏輯
    /// </summary>
    public partial class OrderWindow : Window
    {
        
        ToGo.ToGoEntities dbContext = new ToGoEntities();
        public OrderWindow(/*object tag1, object tag2, object tag4*/)
        {
            InitializeComponent();
            
            //加入各個國家到comboBox
            var q = dbContext.Countries.Select(x => x.CountryCHName).ToList();
            this.ComBoxCountry.ItemsSource = q;
            AddOdDetail();

            if (ComBoxCountry.Text != "") //判斷會員的居住國家
            {
                var q2 = dbContext.Countries.Where(x => x.CountryCHName == ComBoxCountry.Text);
                foreach (var item in q2)
                {
                    _CountryID = item.CountryID;
                }
            }



        }
        public void AddOdDetail()
        {
            for (int i = 0; i < RoomMenu.HN_RN_Qty_UnP.Count; i++)
            {
                OrderDetailUC orderDetailUC = new OrderDetailUC();
                orderDetailUC.QtyLabel.Content = RoomMenu.HN_RN_Qty_UnP[i].Quantity;
                orderDetailUC.UnPriceLabel.Content = RoomMenu.HN_RN_Qty_UnP[i].UnitPrice;
                orderDetailUC.RNameLabel.Content = RoomMenu.HN_RN_Qty_UnP[i].RoomName;
                orderDetailUC.TotalPriceLabel.Content = (RoomMenu.HN_RN_Qty_UnP[i].UnitPrice* RoomMenu.HN_RN_Qty_UnP[i].Quantity).ToString();
                orderDetailUC.HorizontalAlignment = HorizontalAlignment.Center;
                orderDetailUC.VerticalAlignment = VerticalAlignment.Center;
                

                this.OdDetailCollection.Children.Add(orderDetailUC);
            }

        }


        //private object Htag;
        //private object RItag;
        //private object Number;

        private void buy_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _FirstName = FName.Text;
            _LastName = LName.Text;
            _Email = Email.Text;
            _PhoneNumber = PNumber.Text;

            if (_FirstName == "First Name")
            {
                MessageBox.Show("First Name 不可空白");
                return;
            }
            if (_LastName == "Last Name")
            {
                MessageBox.Show("Last Name 不可空白");
                return;
            }
            if (_Email == "Email")
            {
                MessageBox.Show("Email 不可空白");
                return;
            }
            if (!_PhoneNumber.StartsWith("0"))
            {
                MessageBox.Show("請輸入正確電話號碼格式");
                return;
            }
            if (_CountryID == 0)
            {
                MessageBox.Show("請選擇居住國家");
                return;
            }
            //CheckoutWindow checkout = new CheckoutWindow();
            //checkout.Show();
            _HotelName = RoomMenu.HN_RN_Qty_UnP[0].HotelName;
            _RoomName = RoomMenu.HN_RN_Qty_UnP[0].RoomName;

            Payment p = new Payment();
            p.Show();
            this.Close();
        }

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCountry = (string)this.ComBoxCountry.SelectedItem;
            var q = dbContext.Countries.Where(x => x.CountryCHName == selectedCountry).Select(x=>x.CountryID);
            foreach (var item in q)
            {
                _CountryID = item;
            }
        }

        //訂單需要的資料(當前頁面的資料): First Name,Last Name,Email,PhoneNumber,Country
        public static int _HotelID;
        public static string _HotelName;
        public static string _RoomName;
        public static string _FirstName;
        public static string _LastName;
        public static string _Email;
        public static string _PhoneNumber;
        public static int _CountryID;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close(); //記得把_FirstName ,_LastName, _Country,_Email,_Email,_PhoneNumber   List<AllData>清空

        }
    }
}
