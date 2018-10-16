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
    /// RoomInformationDetail.xaml 的互動邏輯
    /// </summary>
    public partial class RoomInformationDetail : Window
    {
        global::ToGo.ToGoEntities dbContent = new ToGoEntities();
        Thickness Roommarginnuber = new Thickness(5, 5, 5, 5);

        public RoomInformationDetail()
        {
            InitializeComponent();
        }

        internal static List<byte[]> RoomInformationImagelist = new List<byte[]>();
        //private object tag2;
        public RoomInformationDetail(object RoomName, object RoomID)
        {
            RoomInformationImagelist.Clear();

            InitializeComponent();

            RoomInformationDetailRoomName.Content = RoomName.ToString();
            //this.tag1 = RoomName;
            //this.tag2 = RoomID;
            int HotelIDtoint = int.Parse(RoomID.ToString());

            var q = from n in dbContent.ServiceAndFacilities
                    from p in n.RoomServiceAndFacilities
                    where p.HotelID == HotelIDtoint && p.RoomNameCN == RoomName.ToString()
                    select new { n.ServiceFacilityCHName };

            int newlabel = 0;
            foreach (var n in q)
            {
                Label label = new Label();
                label.Width = 180;
                label.Margin = Roommarginnuber;
                label.SetValue(Grid.RowProperty, 1);
                label.Content = "■  " + n.ServiceFacilityCHName;                
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Left;
               


                this.smallgird.Children.Add(label);
                Roommarginnuber.Top += 20;
                newlabel += 1;
                if (newlabel % 10 == 0)
                {
                    Roommarginnuber.Top = 5;
                    Roommarginnuber.Left += 180;
                }

            }
                       

            var q1 = (from n in dbContent.RoomImage2
                     where n.HotelID == HotelIDtoint && n.RoomNameCN == RoomName.ToString()
                     select n).ToList() ;

            foreach (var n1 in q1)
            {
                RoomInformationImagelist.Add(n1.Image);                
            }

            Picture.ImageByteRoomInfotmation = RoomInformationImagelist[0];

        }

    }
}
