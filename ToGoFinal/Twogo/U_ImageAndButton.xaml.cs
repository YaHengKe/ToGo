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
    /// ImageAndButton.xaml 的互動邏輯
    /// </summary>
    public partial class ImageAndButton : UserControl
    {
        public ImageAndButton()
        {
            InitializeComponent();

            
            nownumber.Content = 1;
        }

        int imageNumber = 0;
        internal byte[] ImageByteRoomInfotmation
        {
            set
            {                
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(value);
                bi3.StreamSource = ms;
                bi3.EndInit();

                this.showimage.Source = bi3;
                countnumber.Content = RoomInformationDetail.RoomInformationImagelist.Count();
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (imageNumber < RoomInformationDetail.RoomInformationImagelist.Count() - 1)
            {
                imageNumber += 1;                
            }
            else
            {
                imageNumber = 0;               
            }
            this.ImageByteRoomInfotmation = RoomInformationDetail.RoomInformationImagelist[imageNumber];
            nownumber.Content = imageNumber +1;
        }

        private void last_Click(object sender, RoutedEventArgs e)
        {
            if (imageNumber > 0)
            {
                imageNumber -= 1;
            }
            else
            {
                imageNumber = RoomInformationDetail.RoomInformationImagelist.Count() - 1;
            }
            this.ImageByteRoomInfotmation = RoomInformationDetail.RoomInformationImagelist[imageNumber];
            nownumber.Content = imageNumber +1;
        }
    }
}
