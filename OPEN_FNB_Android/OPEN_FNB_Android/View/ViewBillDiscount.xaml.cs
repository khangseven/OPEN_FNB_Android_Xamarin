using OPEN_FNB_Android.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OPEN_FNB_Android.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewBillDiscount : ContentPage
    {
        public ViewBillDiscount()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            loadBillDiscount();
            base.OnAppearing();
        }
        void loadBillDiscount()
        {
            var list = BillDiscount.getAllAvailable(null);
            if (list == null)
            {
                DisplayAlert("Rỗng", "Không tìm thấy khuyến mãi nào", "OKay");
                return;
            }
            lb_title.Text = $"Tìm được {list.Count} khuyến mãi";
            BindableLayout.SetItemsSource(itemDiscount, list);
        }
    }
}