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
    /// ToGoMAP.xaml 的互動邏輯
    /// </summary>
    public partial class ToGoMAP : Window
    {
        public ToGoMAP()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
                Uri uri = new Uri("https://www.google.com.tw/maps/place/Hotel+sardonyx+ueno/@35.7087171,139.7738073,17z/data=!3m1!4b1!4m7!3m6!1s0x60010784ddec2dcd:0x2db93a4e015c9ccd!5m1!1s2018-10-31!8m2!3d35.7087128!4d139.775996?hl=zh-TW&authuser=0");
                wbbrower1.Source = (uri);
            
        }
    }
}
