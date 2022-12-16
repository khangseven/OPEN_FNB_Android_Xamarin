using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    public class BillDetails
    {
		public int id { get; set; }
		public int bill { get; set; }
		public int item { get; set; }
		public int price { get; set; }
		public int amount { get; set; }
		public int last_price { get; set; }
		public string customer_request { get; set; }
		public bool isAnnounced { get; set; }
		public int status { get; set; }
        public Item item_details { get; set; }

        public List<ItemDiscountDetails> discounts  { get; set; }

    public BillDetails(int id, int bill, int item, int price, int amount, int last_price, string customer_request, bool isAnnounced, int status)
        {
            this.id = id;
            this.bill = bill;
            this.item = item;
            this.price = price;
            this.amount = amount;
            this.last_price = last_price;
            this.customer_request = customer_request;
            this.isAnnounced = isAnnounced;
            this.status = status;

            this.item_details = Item.getItemById(null,item);
            this.discounts =  new List<ItemDiscountDetails>();
            this.discounts = ItemDiscountDetails.getAllDiscountOfBillDetails(null, this.id);   
            
        }

        public BillDetails(int bill, int item, int price, int amount, int last_price, string customer_request, bool isAnnounced, int status)
        {
            this.bill = bill;
            this.item = item;
            this.price = price;
            this.amount = amount;
            this.last_price = last_price;
            this.customer_request = customer_request;
            this.isAnnounced = isAnnounced;
            this.status = status;
        }

        public static List<BillDetails> getAllByBillId(MySqlConnection conn, int billid)
        {
            if (conn == null) conn = View.Login.conn;
            List<BillDetails> list = new List<BillDetails>();
            try
            {
                var cmd = new MySqlCommand($"select * from bill_details where bill = {billid}",conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3,$"Không tìm thấy mặt hàng nào trong hóa đơn này");
                    return null;
                }

                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(rs);
                rs.Close();

                foreach(System.Data.DataRow row in dt.Rows)
                {
                    Console.WriteLine("bat dau load");
                    list.Add(new BillDetails((int)row[0], (int)row[1], (int)row[2], (int)row[3], (int)row[4], (int)row[5], row[6] == null ? "" : (string)row[6], (bool)row[7], (int)row[8]));
                    Console.WriteLine("load billdetails xong");
                }

                /*                while (rs.Read())
                                {
                                    list.Add(new BillDetails(rs.GetInt16(0),rs.GetInt16(1),rs.GetInt16(2),rs.GetInt16(3),rs.GetInt16(4),rs.GetInt16(5),rs.GetString(6),rs.GetBoolean(7),rs.GetInt16(8)));
                                }*/
                rs.Close();
                //Controller.ToastController.addToast(2, $"Tìm thấy {list.Count} mặt hàng nào trong hóa đơn này");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi! {System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public bool saveBillDetails(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `bill_details`(`bill`, `item`, `price`, `amount`, `last_price`, `customer_request`, `isAnnounced`, `status`) " +
                    $"VALUES ({this.bill},{this.item},{this.price},{this.amount},{this.last_price},'{this.customer_request}',{this.isAnnounced},{this.status})", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.Off();
                    Bill.updateTotalBill(null, bill);
                    //Controller.ToastController.On();
                    //Controller.ToastController.addToast(2, $"Cập nhật mặt hàng trong hóa đơn {this.bill} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Cập nhật mặt hàng trong hóa đơn {this.bill} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi lưu mặt hàng mặt hàng trong hóa đơn {this.bill}!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }


        public bool updateBillDetails(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"UPDATE `bill_details` SET `bill`={this.bill},`item`={this.item},`price`={this.price},`amount`={this.amount},`last_price`={this.last_price},`customer_request`='{this.customer_request}',`isAnnounced`={this.isAnnounced},`status`={this.status} WHERE id = {this.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật mặt hàng trong hóa đơn {this.bill} thành công!");
                    updateLastPriceBillDetails(null, this.id);
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Cập nhật mặt hàng trong hóa đơn {this.bill} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi cập nhật mặt hàng mặt hàng trong hóa đơn {this.bill}!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }
        
        public bool deteleBillDetails(MySqlConnection conn, int id)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                if (this.discounts != null)
                {
                    discounts.ForEach(d => { this.deleteDiscountToBillDetails(null, d.id, this.id); });
                }
                var cmd = new MySqlCommand($"DELETE FROM `bill_details` WHERE id = {id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa mặt hàng trong hóa đơn {bill} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Xóa mặt hàng trong hóa đơn thất bại! Do không tìm thấy mặt hàng trong hóa đơn {bill} tương ứng");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi xóa mặt hàng mặt hàng trong hóa đơn {bill}!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }
        
        
        //==============================================================================


        public List<ItemDiscount> getAllItemDiscountOfBillDetails(MySqlConnection conn, int billdetailid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select id.* from bill_details bd join list_item_discount lid on lid.bill_details = bd.id join item_discount id on id.id = lid.item_discount where bd.id = {billdetailid};", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi mặt hàng nào cả!");
                    return null;
                }
                List<ItemDiscount> list = new List<ItemDiscount>();
                while (rs.Read())
                {
                    list.Add(new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi mặt hàng tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi m hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public bool addDiscountToBillDetails(MySqlConnection conn, int itemdiscountdetail, int billdetailid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `list_item_discount`(`bill_details`, `item_discount_details`) VALUES ({this.id},{itemdiscountdetail})", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    
                    //Controller.ToastController.addToast(2, $"Thêm khuyến mãi vào mặt hàng trong hóa đơn thành công!");
                    this.discounts =  ItemDiscountDetails.getAllDiscountOfBillDetails(null, this.id); ;



                    return true;
                }
                //Controller.ToastController.addToast(3, $"Thêm khuyến mãi vào mặt hàng trong hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi thêm khuyến mãi vào mặt hàng trong hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool deleteDiscountToBillDetails(MySqlConnection conn, int itemDiscountDetail, int billdetailid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `list_item_discount` WHERE bill_details = {this.id} && item_discount_details = {itemDiscountDetail}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa khuyến mãi vào mặt hàng trong hóa đơn thành công!");
                    this.discounts = ItemDiscountDetails.getAllDiscountOfBillDetails(null, this.id);
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Xóa khuyến mãi vào mặt hàng trong hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi xóa khuyến mãi vào mặt hàng trong hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public static bool updateLastPriceBillDetails(MySqlConnection conn, int id)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"update bill_details set last_price=round(amount*price - (amount*price*(if((select COALESCE(sum(idd.value),0) as total_discount from bill_details bd join list_item_discount lid on lid.bill_details = bd.id join item_discount_details idd on idd.id = lid.item_discount_details where bd.id={id})>100,100,(select COALESCE(sum(idd.value),0) as total_discount from bill_details bd join list_item_discount lid on lid.bill_details = bd.id join item_discount_details idd on idd.id = lid.item_discount_details where bd.id={id}))/100)),-3) where id={id};",conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật giá cuối của mặt hàng trong hóa đơn thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Cập nhật giá cuối của mặt hàng trong hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Cập nhật giá cuối của mặt hàng trong hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

    }
}
