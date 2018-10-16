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
    /// YesOrNo.xaml 的互動邏輯
    /// </summary>
    public partial class YesOrNo : Window
    {
        public YesOrNo(/*object tag1, object tag2, object tag4*/)
        {
            InitializeComponent();
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.Show();
            this.Close();
        }
    }
}
