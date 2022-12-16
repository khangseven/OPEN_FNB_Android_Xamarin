using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using MySqlConnector;
using System.IO;
using OPEN_FNB_Android.Model;
using ZXing.Net.Mobile.Forms;

namespace OPEN_FNB_Android.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public TcpClient client;
        public static MySqlConnection conn;
        public static User user;

        public string code="192.168.1.0";

        public Login()
        {
            InitializeComponent();
            ConnectMySQL();
        }

        protected override void OnAppearing()
        {
            user = null;
            tb_name.Text = "";
            tb_pass.Text = "";
            base.OnAppearing();
        }

        bool  ConnectMySQL()
        {
            conn = Client.MySQLConnection.getMySQLConnection(code, "3306", "openfab", "quantrivien", "123");
            conn.StateChange += Conn_StateChange;
            try
            {
                Console.WriteLine("Database connecting...");
                conn.Open();
                Console.WriteLine("Database connection successfully!");
                DisplayAlert("Xong", "Kết nối thành công đến máy chủ", "OK");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                DisplayAlert("Lỗi","Không thể kết nối đến máy chủ","OK");
                return false;
            }

            
        }

        private void Conn_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            Console.WriteLine("THAI DOI TRANG THAI");
            if (e.CurrentState == System.Data.ConnectionState.Broken || e.CurrentState == System.Data.ConnectionState.Closed)
            {
                Console.WriteLine("lost");
                Navigation.PopToRootAsync();
            }
        }

        void ConnectServer()
        {
            client = new TcpClient();
            client.Connect("192.168.1.9", 5000);
            if (client.Connected)
            {
                DisplayAlert("", "Đã kết nối", "OK");
            }
            else
            {
                DisplayAlert("Lỗi", "Không thể kết nối tới máy chủ", "OK");
            }
        }

        

        private async void btn_login_Clicked(object sender, EventArgs e)
        {
            if(conn.State==System.Data.ConnectionState.Closed)
            {
                bool rs = await DisplayAlert("Chú ý", "Nhập thông tin máy chủ", "Thủ công", "Mã QR");
                if (rs)
                {
                    string result = await DisplayPromptAsync("Chú ý", "Nhập vào IP: ");
                    code = result;
                    ConnectMySQL();
                }
                else
                {
                    var scan = new ZXingScannerPage();
                    await Navigation.PushAsync(scan);
                    scan.OnScanResult += ZXingScannerView_OnScanResult;
                }
                return;
            }

            user = new User();
            var check = User.userLogin(null, out user, tb_name.Text, tb_pass.Text);
            if (check)
            {
                await DisplayAlert(tb_name.Text, "Đăng nhập thành công", "OK");
                
                await Navigation.PushAsync(new Home(user));
            }
            else
            {
                await DisplayAlert(tb_name.Text, "Đăng nhập thất bại", "OK");
            }

        }


        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread( async () => { 
                 await Navigation.PopAsync();
                this.code = result.Text;
                Console.WriteLine(code);
                ConnectMySQL();
            });
        }
    }
}