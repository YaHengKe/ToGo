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

        global::ToGo.ToGoEntities dbContext = new ToGoEntities();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            var q = this.dbContext.Members.Where(x => x.MemberNumber == MainWindow._MemberNumber);


            if (MainWindow._MemberNumber != 0) //判斷登入者是否為會員或訪客，若為登入者 則加入其個人資料
            {
                foreach (var item in q)
                {
                    orderWindow.FName.Text = item.FirstName;
                    orderWindow.LName.Text = item.LastName;
                    orderWindow.Email.Text = item.Email;
                    orderWindow.PNumber.Text = item.PhoneNumber;
                    orderWindow.FName.Text = item.FirstName;
                    orderWindow.ComBoxCountry.Text = item.Country.CountryCHName;
                }
                orderWindow.Show();
            }
            else
            {
                orderWindow.Show();
            }


            
            this.Close();
        }
    }
}
