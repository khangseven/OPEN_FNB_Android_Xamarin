using Android.Widget;
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
    public partial class ChangeItemDiscount : ContentPage
    {
        public BillDetails billDetail { get; set; }

        bool loading = false;

        public ChangeItemDiscount(BillDetails billDetails)
        {
            InitializeComponent();
            this.billDetail = billDetails;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadItemDiscount();
        }

        void loadItemDiscount()
        {
            loading = true;
            var list = ItemDiscountDetails.getAllDiscountOfItem(null, this.billDetail.item);
            if (list == null)
            {
                DisplayAlert("Rỗng", "Không tìm thấy khuyến mãi nào", "OKay");
                return;
            }
            foreach(ItemDiscountDetails idd in list)
            {
                idd.dathem = "false";
                if (billDetail.discounts != null)
                    billDetail.discounts.ForEach(x =>
                    {
                        if (x.id == idd.id)
                        {
                            idd.dathem = "true";
                        }
                    });
            }
            lb_title.Text = $"Tìm được {list.Count} khuyến mãi";
            BindableLayout.SetItemsSource(itemDiscount, list);
            loading = false;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (loading) return;
            var apply = (Xamarin.Forms.CheckBox)sender;
            ItemDiscountDetails idd = (ItemDiscountDetails)apply.BindingContext;
            Console.WriteLine(idd.dathem);
            if (apply.IsChecked)
            {
                bool check = billDetail.addDiscountToBillDetails(null, idd.id, billDetail.id);
                if (check)
                {
                    DisplayAlert("Xong", "Thêm khuyến mãi thành công", "OKay");
                    BillDetails.updateLastPriceBillDetails(null, billDetail.id);
                    Model.Bill.updateTotalBill(null, billDetail.bill);
                    loadItemDiscount();
                }
                else
                {
                    DisplayAlert("Lỗi", "Thêm khuyến mãi thất bại", "OKay");
                    BillDetails.updateLastPriceBillDetails(null, billDetail.id);
                    Model.Bill.updateTotalBill(null, billDetail.bill);
                    loadItemDiscount();
                }
            }
            else
            {
                bool check = billDetail.deleteDiscountToBillDetails(null, idd.id, billDetail.id);
                if (check)
                {
                    DisplayAlert("Xong", "Xóa khuyến mãi thành công", "OKay");
                    BillDetails.updateLastPriceBillDetails(null, billDetail.id);
                    Model.Bill.updateTotalBill(null, billDetail.bill);
                    loadItemDiscount();
                }
                else
                {
                    DisplayAlert("Lỗi", "Xóa khuyến mãi thất bại", "OKay");
                    BillDetails.updateLastPriceBillDetails(null, billDetail.id);
                    Model.Bill.updateTotalBill(null, billDetail.bill);
                    loadItemDiscount();
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}