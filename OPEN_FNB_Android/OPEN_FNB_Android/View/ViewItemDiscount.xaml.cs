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
    public partial class ViewItemDiscount : ContentPage
    {
        public ViewItemDiscount()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            loadItemlDiscount();
            base.OnAppearing();
        }

        void loadItemlDiscount()
        {
            List<ItemDiscount> list = ItemDiscount.getAllAvailable(null);
            if (list == null)
            {
                DisplayAlert("Rỗng", "Không tìm thấy khuyến mãi nào", "OKay");
                return;
            }
            lb_title.Text = $"Tìm được {list.Count} khuyến mãi";
            BindableLayout.SetItemsSource(itemDiscount, list);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ItemDiscount id = btn.BindingContext as ItemDiscount;

            List<ItemDiscountDetails> list = ItemDiscountDetails.getAllItemOf1Discount(null,id.id);

            string s = "";
            list.ForEach(x=> {
                s += x.item.name + " (" + x.value + "%) \n";
            });


            await DisplayAlert("Danh sách sản phẩm", s, "OKay");
        }
    }
}