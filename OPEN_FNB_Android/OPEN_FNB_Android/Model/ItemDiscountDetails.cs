using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using MySqlConnector;
using Xamarin.Forms;

namespace OPEN_FNB_Android.Model
{
    public class ItemDiscountDetails
    {
        public int id   { get; set; }
        public ItemDiscount itemDiscount { get; set; }
        public Item item { get; set; }
        public int value { get; set; }
        public string dathem { get; set; }

        public ItemDiscountDetails( ItemDiscount itemDiscount, Item itemId , int id, int value)
        {
            this.id = id;
            this.itemDiscount = itemDiscount;
            this.item = itemId;
            this.value = value;
        }

        public ItemDiscountDetails() { }


        public static List<ItemDiscountDetails> getAllDiscountOfItem(MySqlConnection conn,int itemId)
        {
            
            if (conn == null) conn = View.Login.conn;
            try
            {  

                //&& (id.litmit = 0 || id.applied < id.litmit)
                MySqlCommand cmd = new MySqlCommand($"select id.*,i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit,idd.id,idd.value from item_discount_details idd join item_discount id on idd.item_discount = id.id join item i on i.id = idd.item join item_type it on i.item_type = it.id where id.isAvailable=true && id.start < CURRENT_DATE && id.end > CURRENT_DATE  && idd.item={itemId};", conn);
                //Console.WriteLine($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit,id.*,idd.id,idd.value from item_discount_details idd join item_discount id on idd.item_discount = id.id join item i on i.id = idd.item join item_type it on i.item_type = it.id where id.isAvailable=true && id.start < CURRENT_DATE && id.end > CURRENT_DATE  && idd.item={itemId}");
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi sản phẩm nào cả!");
                    rs.Close();
                    return null;
                }
                List<ItemDiscountDetails> list = new List<ItemDiscountDetails>();
                int n = 9;
                
                while (rs.Read())
                {
                    Console.WriteLine(rs.GetInt16(19));
                    Image im = new Image();
                    im.Source = ImageSource.FromFile("unknow.png");
                   
                    list.Add(new ItemDiscountDetails(
                        new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8))
                        ,new Item(rs.GetInt32(0+n), rs.GetString(1 + n), rs.GetInt32(2 + n), im, rs.GetBoolean(4 + n), rs.GetBoolean(5 + n), rs.GetInt16(6 + n), rs.GetString(7 + n), rs.GetString(8 + n))
                        , rs.GetInt32(8+n+1), rs.GetInt32(8+n+2)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi sản phẩm tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<ItemDiscountDetails> getAllDiscountOfBillDetails(MySqlConnection conn, int itemId)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                //&& (id.litmit = 0 || id.applied < id.litmit)select id.*,i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit,idd.id,idd.value from item_discount_details idd join item_discount id on idd.item_discount = id.id join item i on i.id = idd.item join item_type it on i.item_type = it.id where i.id = 2
                MySqlCommand cmd = new MySqlCommand($"select id.*,i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit,idd.id,idd.value from item_discount_details idd join item_discount id on idd.item_discount = id.id join item i on i.id = idd.item join item_type it on i.item_type = it.id join list_item_discount lid on lid.item_discount_details = idd.id where lid.bill_details={itemId};", conn);
                //Console.WriteLine($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit,id.*,idd.id,idd.value from item_discount_details idd join item_discount id on idd.item_discount = id.id join item i on i.id = idd.item join item_type it on i.item_type = it.id where id.isAvailable=true && id.start < CURRENT_DATE && id.end > CURRENT_DATE  && idd.item={itemId}");
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi sản phẩm nào cả!");
                    rs.Close();
                    return null;
                }
                List<ItemDiscountDetails> list = new List<ItemDiscountDetails>();
                int n = 9;

                while (rs.Read())
                {
                    Console.WriteLine(rs.GetInt16(19));
                    Image im = new Image();
                    im.Source = ImageSource.FromFile("unknow.png");
                    list.Add(new ItemDiscountDetails(
                        new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8))
                        , new Item(rs.GetInt32(0 + n), rs.GetString(1 + n), rs.GetInt32(2 + n), im, rs.GetBoolean(4 + n), rs.GetBoolean(5 + n), rs.GetInt16(6 + n), rs.GetString(7 + n), rs.GetString(8 + n))
                        , rs.GetInt32(8 + n + 1), rs.GetInt32(8 + n + 2)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi sản phẩm tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        //select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit,idd.*,id.* from item i join item_type it on i.item_type = it.id join item_discount_details idd on idd.item=i.id join item_discount id on id.id = idd.item_discount where id.id=1

        public static List<ItemDiscountDetails> getAllItemOf1Discount(MySqlConnection conn, int itemDiscountId)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                //&& (id.litmit = 0 || id.applied < id.litmit)
                MySqlCommand cmd = new MySqlCommand($"select id.*,i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit,idd.id,idd.value from item i join item_type it on i.item_type = it.id join item_discount_details idd on idd.item=i.id join item_discount id on id.id = idd.item_discount where id.id={itemDiscountId};", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy sản phẩm của khuyến mãi tương ứng!");
                    return null;
                }
                List<ItemDiscountDetails> list = new List<ItemDiscountDetails>();
                int n = 9;
                while (rs.Read())
                {
                    Image im = new Image();
                    im.Source = ImageSource.FromFile("unknow.png");
                    list.Add(new ItemDiscountDetails(
                        new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8))
                        , new Item(rs.GetInt32(0 + n), rs.GetString(1 + n), rs.GetInt32(2 + n), im, rs.GetBoolean(4 + n), rs.GetBoolean(5 + n), rs.GetInt16(6 + n), rs.GetString(7 + n), rs.GetString(8 + n))
                        , rs.GetInt32(8 + n + 1), rs.GetInt32(8 + n + 2)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} sản phẩm của khuyến mãi tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }


    }
}
