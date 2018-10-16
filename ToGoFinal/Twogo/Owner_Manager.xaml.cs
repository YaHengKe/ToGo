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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToGo;


namespace ToGo
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class Owner_Manager : Window
    {
        public Owner_Manager()
        {
            InitializeComponent();

            var q = dbContext.Hotels.Where(i => i.Owner.Email == "mag12242002@gmail.com").Select(i => i.HotelNameCN).ToList();
            for (int i = 0; i < q.Count; i++)
            {
                this.cbhotelname.Items.Add(q[i]);
            }
        }

        global::ToGo.ToGoEntities dbContext = new ToGo.ToGoEntities();
        global::ToGo.ToGoEntities db = new ToGoEntities();
        //ToGoDBEntities db = new ToGoDBEntities();
        string roomname = "";
        string name = "";
        int x, y, z;
        string dpstar = "";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext.Hotels.ToList();
            dbContext.RoomInformations.ToList();
            dbContext.Orders.ToList();

            System.Windows.Data.CollectionViewSource hotelViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hotelViewSource")));
            // 透過設定 CollectionViewSource.Source 屬性載入資料: 
            // hotelViewSource.Source = [泛用資料來源]
            System.Windows.Data.CollectionViewSource roomInformationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("roomInformationViewSource")));
            // 透過設定 CollectionViewSource.Source 屬性載入資料: 
            // roomInformationViewSource.Source = [泛用資料來源]
            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));

            hotelViewSource.Source = dbContext.Hotels.Where(x => x.Owner.Email == "zj4ru04eji6@yahoo.com.tw").ToList();
            roomInformationViewSource.Source = dbContext.RoomInformations.Where(x => x.Hotel.Owner.Email == "zj4ru04eji6@yahoo.com.tw").ToList();
            orderViewSource.Source = dbContext.Orders.Where(x => x.Hotel.Owner.Email == "zj4ru04eji6@yahoo.com.tw").ToList();
            //cityENNameBox.ItemsSource = dbContext.Cities.Local;

        }


        #region Owner新增 修改 刪除
        private void Button_Click(object sender, RoutedEventArgs e) //Add Hotel
        {
            //var q = from h in dbContext.Hotels
            //        where h.OwnerID ==10
            //        select h;
            //this.hotelDataGrid.DataContext = q.ToList();
            //this.dbContext.Hotels.Local.Add(new ToGoDB.Hotel { CountryID = countryID, HotelNameEN = "" });
            //先彈出對話框，確認ok再新增資料到DB
            Window AH = new Add_Hotel();
            AH.Show();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Update Hotel 更新飯店資料
        {
            //修改的部份顏色變動 
            //如果資料沒有變動…

            this.dbContext.SaveChanges();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //Delete Hotel 刪除飯店資料
        {
            var c = (ToGo.Hotel) this.hotelDataGrid.SelectedItem;
                    
            this.dbContext.Hotels.Remove(c);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) //Add hotel image 加入飯店照片
        {

        }

        //RoomInf Management
        private void Button_Click_4(object sender, RoutedEventArgs e) //Add Room
        {
            this.dbContext.RoomInformations.Local.Add(new ToGo.RoomInformation { RoomNameCN="aaa"});
        }
        private void Button_Click_5(object sender, RoutedEventArgs e) //Update Room 更新房型資料
        {
            //修改的部份顏色變動
            this.dbContext.SaveChanges();
        }
        private void Button_Click_6(object sender, RoutedEventArgs e) //Delete Room 刪除房型資料
        {
            var room = (ToGo.RoomInformation)this.roomDataGrid.SelectedItem;

            this.dbContext.RoomInformations.Remove(room);
        }
        private void Button_Click_7(object sender, RoutedEventArgs e) //Add Room image 加入房間照片
        {

        }
        #endregion

        private void datePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //cd1.BlackoutDates.First().Start = datePicker1.SelectedDate??DateTime.Now;
            dpstar = (datePicker1.SelectedDate ?? DateTime.Now).ToString();
        }

        private void datePicker2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            //cd1.BlackoutDates.First().End = datePicker2.SelectedDate??DateTime.Now;
            this.DataGrid1.DataContext = db.Orders.Where(i => i.OrderDate >= (datePicker1.SelectedDate ?? DateTime.Now) && i.OrderDate <= (datePicker2.SelectedDate ?? DateTime.Now))
                                            .Select(a => new { a.OrderID, a.OrderDate, a.RoomNameCN, a.FirstName, a.LastName, a.StartDate, a.EndDate }).ToList(); ;
        }


        private void cbcityname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cbroom.Items.Clear();
            name = this.cbhotelname.SelectedValue.ToString();

            var b = db.Hotels.Where(i => i.HotelNameCN == name).Select(i => i.HotelID).ToList();
            int k = 0;
            for (int i = 0; i < b.Count; i++)
            {
                k = b[i];
            }

            var q = db.RoomInformations.AsEnumerable().Where(i => i.HotelID == k).Select(i => i.RoomNameCN).ToList();
            foreach (var item in q)
            {
                this.cbroom.Items.Add(item);
            }
            var p = db.Orders.Where(a => a.HotelNameCN == name).Select(a => new { a.OrderID, a.OrderDate, a.RoomNameCN, a.FirstName, a.LastName, a.StartDate, a.EndDate }).ToList();
            this.DataGrid1.DataContext = p;

            totalroom.Content = q.Count();
            mtroom.Content = p.Count();
            x = int.Parse(totalroom.Content.ToString());
            y = int.Parse(mtroom.Content.ToString());
            z = x - y;
            chroom.Content = z.ToString();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (roomname != null && this.cbroom.SelectedValue != null)
            {
                roomname = this.cbroom.SelectedValue.ToString();
            }
            else
            {
                return;
            }

            var roomlist = db.Orders.Where(a => a.HotelNameCN == name && a.RoomNameCN == roomname).Select(a => new { a.OrderID, a.OrderDate, a.RoomNameCN, a.FirstName, a.LastName, a.StartDate, a.EndDate }).ToList();

            this.DataGrid1.DataContext = roomlist;

            for (int i = 0; i < roomlist.Count; i++)
            {
                CalendarDateRange dateRange = new CalendarDateRange();
                dateRange.Start =roomlist[i].StartDate.Value;
                dateRange.End = roomlist[i].EndDate.Value;
                cd1.BlackoutDates.Add(dateRange);
            }
            if (roomlist.Count >= 2)
            {
                cd1.DisplayDate = roomlist[1].StartDate.Value;
            }

        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cd1.SelectedDates.Clear();
            if (DataGrid1.SelectedCells.Count > 0)
            {
                cd1.BlackoutDates.Clear();
                string StartDate = "";
                string EndDate = "";
                var item = DataGrid1.SelectedItem;
                if (item != null)
                {
                    StartDate = (DataGrid1.SelectedCells[12].Column.GetCellContent(item) as TextBlock).Text;
                    EndDate = (DataGrid1.SelectedCells[13].Column.GetCellContent(item) as TextBlock).Text;
                }
                else
                {
                    return;
                }

                cd1.BlackoutDates.Add(new CalendarDateRange()
                {
                    Start = DateTime.Parse(StartDate),
                    End = DateTime.Parse(EndDate)
                });

                cd1.DisplayDate = DateTime.Parse(StartDate);
            }
        }

        #region 新增飯店和房型的服務和設施視窗
        private void Button_Click_8(object sender, RoutedEventArgs e) 
        {
            Room_Service r = new Room_Service();
            r.Show();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            Hotel_Service f = new Hotel_Service();
            f.Show();
        }
        #endregion

        int countryID;
        private void countryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var country = countryComboBox.SelectedValue as Country;
            countryID = country.CountryID;            
        }

    }
}
