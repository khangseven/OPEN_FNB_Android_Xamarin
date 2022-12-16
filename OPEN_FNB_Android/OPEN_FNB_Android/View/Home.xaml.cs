using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OPEN_FNB_Android.Model;

namespace OPEN_FNB_Android.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public User user { get; set; }
        public Home()
        {
            InitializeComponent();
        }

        public Home(User u)
        {
            InitializeComponent();
            this.user = u;
            username.Text = "Tên người dùng: " + u.name;
            
        }


        private void btn_bd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewBillDiscount());
        }

        private void btn_table_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Tables(0));
        }

        private void btn_taw_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Tables(1));
        }

        private void btn_id_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewItemDiscount());
        }
    }
}