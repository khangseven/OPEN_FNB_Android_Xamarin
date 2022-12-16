using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OPEN_FNB_Android.Model;

namespace OPEN_FNB_Android.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tables : ContentPage
    {
        public ObservableCollection<Table> listTable { get; set; }

        int type = 0;
        public Tables(int type) 
        {
            InitializeComponent();
            this.type=type;
        }

        protected override void OnAppearing()
        {
            loadTable();
            base.OnAppearing();
            
        }

        void loadTable()
        {
            var condition = " && t.id!=0 Order by t.name ASC ";
            if (type == 1)
            {
                btn.IsVisible = true;
                condition = " && t.id=0  && b.id is not null Order by t.name ASC ";
                Title = "Danh sách mang về";
            }
            else
            {
                btn.IsVisible = false;
            }

            List<Table> a = Table.getAllWithCondition(null, condition);
            Console.WriteLine("Show");
            if (a == null)
            {
                DisplayAlert("Rỗng", "Không tìm thấy bàn", "OKay");
                
            }
            collectionView.ItemsSource = a;
        }

        private async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //await Navigation.PushAsync(new Bill());
            Table tb = e.CurrentSelection[0] as Table;
            if (tb != null)
            {
                if (tb.bill != -1)
                {
                    await Navigation.PushAsync(new Bill(tb.bill));
                }
                else
                {
                    bool rs = await DisplayAlert("Thêm mới?", "Tạo hóa đơn cho " + tb.name, "Tạo", "Hủy");
                    if (rs)
                    {
                        Model.Bill b = new Model.Bill(tb.id, Login.user.id, DateTime.Now, new DateTime(), 0, false, "");
                        rs = b.saveBill(null);
                        if (rs)
                        {
                            await DisplayAlert("Xong", "Thêm hóa đơn mới thành công", "OKay");
                            int billID = Table.getTableById(null, tb.id).bill;
                            await Navigation.PushAsync(new Bill(billID));
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                    else
                    {

                    }
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string reason = await DisplayPromptAsync("Thêm mang về", "Nhập Thông Tin: ");
            if(reason!=null && reason.Length > 0)
            {
                Model.Bill b = new Model.Bill(0, Login.user.id, DateTime.Now, new DateTime(), 0, false, reason);

                var check = b.saveBill(null);
                if (check)
                {
                    await DisplayAlert("Xong", "Thêm hóa đơn mang về thành công", "OKay");
                    loadTable();
                }
                else
                {
                    await DisplayAlert("Lỗi", "Thêm hóa đơn mang về thất bại", "OKay");
                }
            }
            else
            {
                await DisplayAlert("Lỗi", "Thông tin nhập vào không hợp lệ", "OKay");
            }

            
        }
    }
}
