using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OPEN_FNB_Android.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OPEN_FNB_Android.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bill : ContentPage
    {
        public int billid { get; set; }
        Model.Bill bill;
        public Bill(int billid)
        {
            InitializeComponent();
            this.billid = billid;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing(); 
            loadBill();
        }

        void loadBill()
        {
            this.bill = Model.Bill.getBillByID(null, this.billid);
            
            Console.WriteLine("da layb xong bill");
            var billDetails = BillDetails.getAllByBillId(null, bill.id);
            Console.WriteLine("da layb xong bill 2");

            lb_table.Text = bill.table_name;
            lb_username.Text = bill.username;
            lb_time.Text = bill.checkin.ToString("dd/MM/yyyy HH:mm");
            lb_total.Text =Item.intToVnd(bill.total);
            //stack.BindingContext = billDetails;
            BindableLayout.SetItemsSource(stack, billDetails);
            BindableLayout.SetItemsSource(billDiscount, bill.discounts);

            var sl = 0;
            if (billDetails != null) sl = billDetails.Count;

            Title = $"Chi tiết hóa đơn | {sl} SP";
        }

        public async void DeleteBillDetails(object sender, EventArgs e)
        {
            BillDetails billDetail = (BillDetails)(sender as Button).BindingContext;
            bool rs = await DisplayAlert("Chú ý", "Xóa sản phẩm này khỏi hóa đơn?", "Xóa","Hủy");
            if (rs)
            {
                bool check = billDetail.deteleBillDetails(null, billDetail.id);
                if (check)
                {
                    Model.Bill.updateTotalBill(null, this.billid);
                    loadBill();
                    await DisplayAlert("Xong", "Đã xóa sản phẩm khỏi hóa đơn", "Tôi đã hiểu");
                }
                else await DisplayAlert("Lỗi", "Xóa sản phẩm thất bại", "Tôi đã hiểu");
            }
        }

        public async void  ChangeBillDetails(object sender, EventArgs e)
        {
            BillDetails bd = (BillDetails)(sender as Button).BindingContext;

            string result = await DisplayPromptAsync("Sửa số lượng", "Nhập vào số lượng", maxLength: 10, keyboard: Keyboard.Numeric);
           
            try
            {
                int value = Int16.Parse(result);
                if (value == 0)
                {
                    await DisplayAlert("Lỗi", "Số lượng phải là số nguyên dương > 0", "Tôi đã hiểu");
                    return;
                }

                bd.amount = value;

                bool check = bd.updateBillDetails(null);
                if (check)
                {
                    Model.Bill.updateTotalBill(null, this.billid);
                    await DisplayAlert("Xong", "Cập nhật số lượng thành công", "Tôi đã hiểu");
                    loadBill();
                }else
                    await DisplayAlert("Lỗi", "Cập nhật số lượng thất bại", "Tôi đã hiểu");

            }
            catch(Exception ex)
            {
                await DisplayAlert("Lỗi", "Số lượng phải là số nguyên dương > 0", "Tôi đã hiểu");
            }
        }

        public async void ChangeBillDetailsDiscount(object sender, EventArgs e)
        {
            BillDetails bd = (BillDetails)(sender as Button).BindingContext;
            await Navigation.PushAsync(new ChangeItemDiscount(bd));
        }

        public async void AddItemToBill(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddItem(billid));
            Model.Bill.updateTotalBill(null, billid);
        }

        public async void UpdateBillDiscount(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangeBillDiscount(this.bill));

        }

        public async void DoPayment(object sender, EventArgs e)
        {
            bool b = await DisplayAlert("Chú ý", "Thanh toán hóa đơn này?", "OKay", "Hủy");
            if ( b== false)
            {
                return;
            }
            if (bill == null)
            {
                await DisplayAlert("Lỗi", "Thanh toán thất bại", "OKay");
                await Navigation.PopAsync();
                return;
            }
            bill.complete = true;
            var check= bill.updateBill(null);
            if (check)
            {
                await DisplayAlert("Xong", "Thanh toán thành công", "OKay");
                await Navigation.PopAsync();

            }
            else
            {
                await DisplayAlert("Lỗi", "Thanh toán thất bại", "OKay");
            }
        }
        public async void ChangeTable(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Chuyển bàn", "Nhập ID bàn đích: ", maxLength: 10, initialValue: "1", keyboard: Keyboard.Numeric);

            try
            {
                int amount = Int16.Parse(result);
                if (amount == 0)
                {
                    await DisplayAlert("Lỗi", "Số ID bàn là số nguyên dương > 0", "Tôi đã hiểu");
                    return;
                }
                int tableID = amount;
                //Check if table does not exists;
                var table = Table.getTableById(null, tableID);
                if (table != null)
                {
                    //check if table not free
                    string condition = $"t.id = {tableID} && b.complete=0";
                    var bills = Model.Bill.getAllWithCondition(null, condition);
                    if (bills == null)
                    {
                        //change table
                        bill.table_id = tableID;
                        var check = bill.updateBill(null);
                        if (check)
                        {
                            await DisplayAlert("Xong", $"Đã chuyển sang bàn ID: {tableID}", "Tôi đã hiểu");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Lỗi", "Chuyển bàn thất bại", "Tôi đã hiểu");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Lỗi", "Bàn đã có người ngồi", "Tôi đã hiểu");
                    }

                }
                else
                {
                    await DisplayAlert("Lỗi", "Bàn không tồn tại", "Tôi đã hiểu");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Số lượng phải là số nguyên dương > 0", "Tôi đã hiểu");
            }

        }
        public async void AddExtraFee(object sender, EventArgs e)
        {
            
            string reason = await DisplayPromptAsync("Phụ thu", "Nhập lý do: ");
            if(reason != null && reason.Length > 0)
            {
                string price = await DisplayPromptAsync("Phụ thu", "Nhập phí: ", maxLength: 10, initialValue: "1000", keyboard: Keyboard.Numeric);
                try
                {
                    int amount = Int16.Parse(price);
                    if (amount < 1000)
                    {
                        await DisplayAlert("Lỗi", "Phí phải là số nguyên dương > 1000", "Tôi đã hiểu");
                        return;
                    }

                    BillDetails bd = new BillDetails(bill.id, -1, amount, 1, amount, reason, true, 1);
                    bool temp = bd.saveBillDetails(null);
                    if (temp)
                    {
                        Model.Bill.updateTotalBill(null, bill.id);
                        loadBill();
                        await DisplayAlert("Xong", "Thêm phụ thu thành công", "Tôi đã hiểu");
                        
                    }
                    else
                    {
                        await DisplayAlert("Lỗi", "Thêm phụ thu thất bại", "Tôi đã hiểu");
                    }
                    return;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Phí phải là số nguyên dương > 1000", "Tôi đã hiểu");
                    return;
                }
            }
            else
            {
                await DisplayAlert("Lỗi", "Lý do không hợp lệ", "Tôi đã hiểu");
                return;
            }

        }
        public async void CancelBill(object sender, EventArgs e)
        {
            bool b = await DisplayAlert("Chú ý", "Hủy hóa đơn này?", "Hủy", "Không Hủy");
            if (!b)
            {
                return;
            }
            bill.deleteALlDiscountofBill(null, bill.id);
            var billDetails = BillDetails.getAllByBillId(null, bill.id);
            if (billDetails != null)
                foreach (BillDetails bd in billDetails)
                {
                    bd.deteleBillDetails(null, bd.id);
                }
            if (bill.deleteBill(null))
            {
                await DisplayAlert("Xong", $"Hủy đơn thành công!", "Tôi đã hiểu");
                await Navigation.PopAsync();
            }
        }
    }
}