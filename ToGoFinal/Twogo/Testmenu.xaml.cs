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
    /// Testmenu.xaml 的互動邏輯
    /// </summary>
    public partial class Testmenu : Window
    {
        public Testmenu()
        {
            InitializeComponent();


            Thickness marginnuber = new Thickness(5, 5, 5, 5);

            for (int i = 1; i < 13; i++)
            {
                Button btn = new Button();
                btn.Content = "HotelID__" + i;
                btn.Height = 40;
                btn.Width = 120;
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.VerticalAlignment = VerticalAlignment.Top;
                btn.Margin = marginnuber;
                btn.Click += Btn_Click;
                btn.Tag = i ;  //HotelID




                grid1.Children.Add(btn);
                marginnuber.Top += 30;
            }

        }

        
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            RoomMenu rp = new RoomMenu(((Button)sender).Tag,SDate.SelectedDate.Value,EDate.SelectedDate.Value,People.Text);
            
            rp.Show();

        }

        global::ToGo.ToGoEntities dbcontent = new ToGoEntities();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var q = from n in this.dbcontent.Orders
                    from p in n.Comments
                    where n.HotelID == 2
                    select new { p.CPStars, p.LocationPoint, p.CleanPoint, p.FacilityPoint, p.ServicePoint };

            decimal a = 0;
            int i = 0;
            foreach (var n in q)
            {
                //a += (int)n.CPStars + (int)n.CleanPoint + (int)n.FacilityPoint + (int)n.LocationPoint + (int)n.ServicePoint;
                a += n.CPStars + n.CleanPoint + n.FacilityPoint + n.LocationPoint + n.ServicePoint;
                i += 5;
            }
            評論.Content = a / i;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string Name = "Tokyo";

            var q1 = (from n in this.dbcontent.Countries
                     from p in n.Cities
                     from t in n.Hotels
                     from r in this.dbcontent.RoomInformations
                     where p.CityENName == Name || n.CountryENName == Name
                     select new { t.HotelID ,t.HotelNameEN,r.UnitPrice }).OrderByDescending(x=>x.UnitPrice).Take(1);

            string output = "";
            foreach (var n in q1)
            {
                output += n + "  ";
            }
            評論.Content = output;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //insert i = new insert();
            //i.Show();
        }
    }
}
