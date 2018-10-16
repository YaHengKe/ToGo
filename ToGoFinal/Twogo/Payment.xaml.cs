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
using System.Windows.Shapes;

namespace ToGo
{
    /// <summary>
    /// Payment.xaml 的互動邏輯
    /// </summary>
    public partial class Payment : Window
    {
        public Payment()
        {
            InitializeComponent();            
            
        }
        public static string _CardHolderName;
        public static string _CardNumber;
        public DateTime _OrderDate;


        //訂單所需資料 orderDate,CardNumber,CardHolderName,Request,Ispay
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _CardHolderName = this.TxtName.Text;
            _CardNumber = this.TxtCardNumber.Text;
            _OrderDate = DateTime.Now;

            MessageBox.Show("訂購成功!");

            
            OrderSelectableRoom();
            CreateOrderDetail();
            
            this.Close();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            this.Close(); //記得要把orderDate,CardNumber,CardHolderName,Request,Ispay 清空
        }

        //加入靜態變數　OrderID
        public static int _OrderID;

        //MemberID家道訂單裡

        //加入一個方法 該方法能產生一筆訂單(訂單使用連線)
        public void OrderSelectableRoom()
        {
            try //尚缺 OrderCode,TotalPrice,Request,MemberNumber,IsPaied,OrderState
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=ToGo;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into [dbo].[Order] (HotelID,HotelNameCN,RoomNameCN,OrderDate,FirstName,LastName,PhoneNumber,Email,CountryID,CardNumber,CardHolderName,StartDate,EndDate,MemberNumber) values " +
                                                                    "(@HotelID,@HotelNameCN,@RoomNameCN,@OrderDate,@FirstName,@LastName,@PhoneNumber,@Email,@CountryID,@CardNumber,@CardHolderName,@StartDate,@EndDate,@MemberNumber);" +
                                                                    "select orderId from [dbo].[Order] where OrderId=Scope_Identity()";    //回傳orderID資料
                        cmd.Parameters.Add("@HotelID", SqlDbType.Int).Value = RoomMenu._HotelID;
                        cmd.Parameters.Add("@HotelNameCN", SqlDbType.NVarChar).Value = OrderWindow._HotelName;
                        cmd.Parameters.Add("@RoomNameCN", SqlDbType.NVarChar).Value = OrderWindow._RoomName;
                        cmd.Parameters.Add("@OrderDate", SqlDbType.Date).Value = DateTime.Today;
                        cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = OrderWindow._FirstName;
                        cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = OrderWindow._LastName;
                        cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value = OrderWindow._PhoneNumber;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = OrderWindow._Email;
                        cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = OrderWindow._CountryID;
                        cmd.Parameters.Add("@CardNumber", SqlDbType.VarChar).Value = Payment._CardNumber;
                        cmd.Parameters.Add("@CardHolderName", SqlDbType.NVarChar).Value = Payment._CardHolderName;
                        cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = MainWindow._SearchSDate;
                        cmd.Parameters.Add("@EndDate",SqlDbType.Date).Value=MainWindow._SearchEDate;
                        
                        cmd.Parameters.Add("@MemberNumber", SqlDbType.Int).Value = MainWindow._MemberNumber;
                        //MemberNumber 等於0 表示非會員 但依舊會加入一筆訂單
                        conn.Open();
                        _OrderID = (int)cmd.ExecuteScalar();


                        MessageBox.Show("已成功加入一筆Order " + _OrderID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public static int RoomNameType;
        //加入訂購房型數量 (每個不同房型都各算一個)

        //加入多筆 OrderDetail 訂單的每一間房間都各算一筆
        public void CreateOrderDetail()
        {

            try //尚缺 OrderCode,TotalPrice,Request,MemberNumber,IsPaied,OrderState
            {
                                
                int k = 0;
                for (int i = 0; i < RoomMenu.HN_RN_Qty_UnP.Count; i++)
                {
                    
                    for (int j = 0; j <RoomMenu.HN_RN_Qty_UnP[i].Quantity ; j++)
                    {
                        

                        using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=ToGo;Integrated Security=True"))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = "insert into [dbo].[OrderDetail] (OrderID,RoomId,UnitPrice) values(@OrderID,@RoomId,@UnitPrice)";
                                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = Payment._OrderID;
                                cmd.Parameters.Add("@RoomId", SqlDbType.Int).Value = RoomMenu.RoomIDCollection[k];  //k 是RoomIDCollection的其中一筆 很難參透
                                cmd.Parameters.Add("@UnitPrice", SqlDbType.Int).Value = RoomMenu.HN_RN_Qty_UnP[i].UnitPrice; //單價從集合的第 i 筆中取出
                                conn.Open();
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("已成功加入一筆OrderDetail");
                            }
                        }
                        k += 1; //k 是RoomIDCollection的其中一筆
                    }
                    
                }             

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
