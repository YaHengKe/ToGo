using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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


namespace ToGo
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.CalendarSearch.DisplayDateStart = DateTime.Today;
            this.CalendarSearch.DisplayDateEnd = DateTime.Today.AddMonths(3);
            var q = dbContext.Cities.Select(x => x.CityCHName).ToList();
            //CityComboBox.ItemsSource = q;
            //將各個城市加入ComboBox(City)
            var q1 = dbContext.Cities.Select(x => new { x.Country.CountryCHName, x.CityCHName });
            List<string> tempList = new List<string>();
            
            foreach (var item in q1)
            {
                tempList.Add(item.CountryCHName + " " + item.CityCHName);//將國家和城市同時加入ComboBox，並以空白區分
            }
            CityComboBox.ItemsSource = tempList;

        }

        public static string LoginFirstName; // 登入成功後存取名字
        public static string LoginLastName;
        public static string LoginEmail;
        public static string LoginPhone;
        public static string OwnerLoginEmail;
        //MemberID加入
        public static int _MemberNumber = 0;
        //MemberNumber 等於0 表示非會員
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemberLogin w = new MemberLogin();
            w.Owner = this;
            this.Background = new SolidColorBrush(Colors.Gray);
            w.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewMember w = new NewMember();
            w.Owner = this;
            this.Background = new SolidColorBrush(Colors.Gray);
            w.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OwnerLogin w = new OwnerLogin();
            w.Owner = this;
            this.Background = new SolidColorBrush(Colors.Gray);
            w.ShowDialog();
        }

        global::ToGo.ToGoEntities dbContext =new ToGoEntities();

        public static DateTime _StartDate;
        public static DateTime _EndDate;
        public static string _City;
        public static int _RoomType;

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (CityComboBox.Text == "" || CityComboBox.Text == "請輸入城市")/*TextBox_City.Text == "")*/
            {
                City_Require.Opacity = 1;
            }
            else if (SearchSDate.Content.ToString()=="請選擇")
            {
                Date_Require.Opacity = 1;
            }
            else if(TextBox_Room.Text=="")
            {
                Room_Require.Opacity = 1;
            }
            else if(TxtRoomType.Text=="")
            {
                People_Require.Opacity = 1;
            }
            else
            {
                List<int> RoomIdCollection = new List<int>();
                List<int> IsOrderedRoom = new List<int>();

                _StartDate = CalendarSearch.SelectedDates[0]; //(DateTime)SearchSDate.Content;
                _EndDate = CalendarSearch.SelectedDates[CalendarSearch.SelectedDates.Count - 1]; //(DateTime)SearchEDate.Content;
                                                                                                 //_City = TextBox_City.Text;               
                                                                                                 //CityComboBox.Text.
                _City = ReturnCityCnName(); //呼叫新定義的方法

                //_City = CityComboBox.Text;
                //把TextBox換成ComboBox
                _RoomType = int.Parse(TxtRoomType.Text);

                using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=ToGo;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        cmd.CommandText = "select  r1.CityCHName,r1.HotelNameCN,r1.GoogleMapUri,r1.HotelID,min(r1.UnitPrice) from V_Room r1 where r1.CityCHName=@CityCHName and  not exists (select 1 from( select r.CityCHName,r.HotelID, r.RoomID from V_Room r left join v_order o on r.HotelID= o.HotelID and r.RoomID=o.RoomID where r.CityCHName=@CityCHName and r.RoomType=@RoomType and  @SearchStartDate  between ISNULL( o.StartDate, '2099-11-05') and ISNULL( dateadd( day,-1,o.EndDate),'2099-11-05')or  @SearchEndDate   between ISNULL( o.StartDate, '2099-11-05') and ISNULL(  o.EndDate,'2099-11-05')or o.StartDate between @SearchStartDate and @SearchEndDate or DATEADD(day,-1 ,o.EndDate) between @SearchStartDate and @SearchEndDate) r2 where r1.CityCHName =r2.CityCHName and r1.HotelID =r2.HotelID and r1.RoomID  =r2.RoomID)group by r1.CityCHName,r1.HotelNameCN,r1.GoogleMapUri,r1.HotelID";
                        cmd.Parameters.Add("@RoomType", SqlDbType.Int).Value = _RoomType;
                        cmd.Parameters.Add("@CityCHName", SqlDbType.NVarChar).Value = _City;
                        cmd.Parameters.Add("@SearchStartDate", SqlDbType.Date).Value = _StartDate;
                        cmd.Parameters.Add("@SearchEndDate", SqlDbType.Date).Value = _EndDate;

                        conn.Open();
                        Hotel_Search ww = new Hotel_Search();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            int a = 0;//搜尋出的資料筆數
                            while (dr.Read())
                            {
                                Window_HotelButton xx = new Window_HotelButton((int)dr["HotelID"]); //利用建構子方法 把HotelID 帶入                                
                                xx.City = dr["CityCHName"];//CityCHName
                                xx.DESC = dr["HotelNameCN"];//HotelNameCN
                                xx.TempUrl = dr["GoogleMapUri"].ToString(); //URL
                                xx.price = dr[4];//UnitPrice                                
                                ww.StackPanel_ShowHotel.Children.Add(xx);
                                a++;
                            }
                            ww.Label_HotelCount.Content = a;
                        }
                        this.Close();
                        ww.Show();
                    }
                }
            }
        }

        

        public static DateTime _SearchSDate;
        public static DateTime _SearchEDate;
        
        private void searchDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SearchSDate.Content = this.CalendarSearch.SelectedDates[0].ToString("yyyy/MM/dd");
            _SearchSDate= this.CalendarSearch.SelectedDates[0];
            this.SearchEDate.Content = this.CalendarSearch.SelectedDates[CalendarSearch.SelectedDates.Count - 1].ToString("yyyy/MM/dd");
            _SearchEDate= this.CalendarSearch.SelectedDates[CalendarSearch.SelectedDates.Count - 1];
        }

        private void TextBox_City_KeyUp(object sender, KeyEventArgs e)
        {
            exp1.IsExpanded = true;
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e) //允許使用者輸入欲搜尋的城市，並將符合條件的城市秀出來
        {
            ComboBox cmb = (ComboBox)sender;
            CollectionView itemsViewOriginal = (CollectionView)CollectionViewSource.GetDefaultView(cmb.ItemsSource);

            itemsViewOriginal.Filter = ((o) =>
            {
                if (string.IsNullOrEmpty(cmb.Text)) return false;
                else
                {
                    if (((string)o).Contains(cmb.Text)) return true;
                    else return false;
                }
            });

            cmb.IsDropDownOpen = true;
            itemsViewOriginal.Refresh();

        }
        
        private string ReturnCityCnName() //這個方法用來把ComboBox(City)裡面的資料「僅傳回城市」
        {
            string s = "";
            bool _Space = false;
            foreach (var item in CityComboBox.Text.ToString())
            {
                if (item == ' ' || _Space == true) //遇到空白就才加入
                {
                    _Space = true;  //遇到空白就把值改為true 這樣下一圈迴圈進來以後就會繼續執行

                    s += item; 
                }
            }
            return s.Trim();   //去掉空白後傳回
        }
    }
}
