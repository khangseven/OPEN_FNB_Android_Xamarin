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
    public partial class ChangeBillDiscount : ContentPage
    {
       
        public Model.Bill bill { get; set; }

        bool loading = false;

        public ChangeBillDiscount(Model.Bill b)
        {
            InitializeComponent();
            this.bill=b;
            loading = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadBillDiscount();
        }

        void loadBillDiscount()
        {
            loading = true;
            var list = BillDiscount.getAllAvailable(null);
            if (list == null)
            {
                DisplayAlert("Rỗng", "Không tìm thấy khuyến mãi nào", "OKay");
                return;
            }
            foreach (BillDiscount bd in list)
            {
                bd.dathem = "false";
                if (this.bill.discounts != null)
                    bill.discounts.ForEach(x =>
                    {
                        if (x.id == bd.id)
                        {
                            bd.dathem = "true";
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
            BillDiscount bd = (BillDiscount)apply.BindingContext;
            if (apply.IsChecked)
            {
                bool check = bill.addDiscountToBill(null, bd.id, bill.id);
                if (check)
                {
                    DisplayAlert("Xong", "Thêm khuyến mãi thành công", "OKay");
                    Model.Bill.updateTotalBill(null, bill.id);
                    loadBillDiscount();
                }
                else
                {
                    DisplayAlert("Lỗi", "Thêm khuyến mãi thất bại", "OKay");
                    Model.Bill.updateTotalBill(null, bill.id);
                    loadBillDiscount();
                }
            }
            else
            {
                bool check = bill.deleteDiscountToBill(null, bd.id, bill.id);
                if (check)
                {
                    DisplayAlert("Xong", "Xóa khuyến mãi thành công", "OKay");
                    Model.Bill.updateTotalBill(null, bill.id);
                    loadBillDiscount();
                }
                else
                {
                    DisplayAlert("Lỗi", "Xóa khuyến mãi thất bại", "OKay");
                    Model.Bill.updateTotalBill(null, bill.id);
                    loadBillDiscount();
                }
            }
        }
    }
}