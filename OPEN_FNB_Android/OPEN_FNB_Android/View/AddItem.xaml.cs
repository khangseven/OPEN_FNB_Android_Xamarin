using MySqlConnector;
using OPEN_FNB_Android.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OPEN_FNB_Android.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItem : ContentPage
    {
        public int billid { get; set; }
        public string itemName { get; set; }

        public AddItem(int b)
        {
            InitializeComponent();
            this.billid = b;
            name.BindingContext = itemName;
            itemName = "";
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadItem();
        }

        async void loadItem()
        {
            
            string condition = $"where i.name like '%{name.Text}%' && i.isAvailable=true";
            //var list = Item.getAllWithCondition(null, condition);

            var conn = View.Login.conn;
            var list = new List<Item>();
            
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit from item i join item_type it on i.item_type = it.id {condition}", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy mặt hàng nào hết!");
                    rs.Close();
                    return;
                }

                while (rs.Read())
                {
                    Image im = new Image();
                    if (rs.IsDBNull(3))
                    {
                        im.Source = ImageSource.FromFile("unknow.png");

                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream();
                        rs.GetStream(3).CopyTo(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        im.Source = ImageSource.FromStream(() => ms);

                    }
                    list.Add(new Item(rs.GetInt32(0), rs.GetString(1), rs.GetInt32(2), im, rs.GetBoolean(4), rs.GetBoolean(5), rs.GetInt16(6), rs.GetString(7), rs.GetString(8)));
                    
                }
                

                rs.Close();


            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xãy ra lỗi khi lấy dữ liệu về mặt hàng{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return;
            }
            if (list == null)
            {
                await DisplayAlert("Rỗng", "Không tìm thấy sản phẩm nào", "OKay");
                return;
            }
            listItem.ItemsSource = list;

            Title = "Thêm sản phẩm";
        }

        void addItem()
        {
            //BillDetails bd = new BillDetails(selectedTable.Bill, item.id, item.price * amount, amount, item.price * amount, "", true, 1);
            //bool temp = bd.saveBillDetails(null);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            loadItem();
        }

        private async void listItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item item = e.CurrentSelection[0] as Item;
            if (item != null)
            {
                string result = await DisplayPromptAsync("Sửa số lượng", "Nhập vào số lượng", maxLength: 10, initialValue: "1",keyboard: Keyboard.Numeric);

                try
                {
                    int amount = Int16.Parse(result);
                    if (amount == 0)
                    {
                        await DisplayAlert("Lỗi", "Số lượng phải là số nguyên dương > 0", "Tôi đã hiểu");
                        return;
                    }

                    BillDetails bd = new BillDetails(billid, item.id, item.price * amount, amount, item.price * amount, "", true,1);
                    var check = bd.saveBillDetails(null);
                    if (check)
                    {
                        await DisplayAlert("Xong", "Thêm sản phẩm thành công", "Okay");
                        OnBackButtonPressed();
                    }
                    else
                    {
                        await DisplayAlert("Lỗi", "Thêm sản phẩm thất bại", "Okay");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Số lượng phải là số nguyên dương > 0", "Tôi đã hiểu");
                }
            }
        }
    }
}