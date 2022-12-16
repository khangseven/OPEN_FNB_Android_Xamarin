
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    public class ItemDiscount
    {
        public int id { get; set; }
        public bool isDefault { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int applied { get; set; }
        public int limit { get; set; }
        public bool isAvailable { get; set; }


        private List<Point> listItem;

        public ItemDiscount(int id, bool isDefault, string name, string info, DateTime start, DateTime end, int applied, int limit, bool isAvailable)
        {
            this.id = id;
            this.isDefault = isDefault;
            this.name = name;
            this.info = info;
            this.start = start;
            this.end = end;
            this.applied = applied;
            this.limit = limit;
            this.isAvailable = isAvailable;

            /*this.listItem = new List<Point>();
            var conn = View.Login.conn;
            var cmd = new MySqlCommand($"select * from item_discount_details where item_discount = {this.id}", conn);
            var rs = cmd.ExecuteReader();
            while (rs.Read())
            {
                listItem.Add(new Point(rs.GetInt16(1), rs.GetInt16(2)));
            }
            rs.Close();*/
        }

        public ItemDiscount(bool isDefault, string name, string info, DateTime start, DateTime end)
        {
            this.isDefault = isDefault;
            this.name = name;
            this.info = info;
            this.start = start;
            this.end = end;
        }

        public ItemDiscount() { }

        //Get Update Save Delete
        public static List<ItemDiscount> getAll(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from item_discount", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi sản phẩm nào cả!");
                    return null;
                }
                List<ItemDiscount> list = new List<ItemDiscount>();
                while (rs.Read())
                {
                    list.Add(new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5),rs.GetInt16(6),rs.GetInt16(7),rs.GetBoolean(8)));
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

        public static List<ItemDiscount> getAllAvailable(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from item_discount where start < CURRENT_DATE && end > CURRENT_DATE ", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi sản phẩm nào cả!");
                    return null;
                }
                List<ItemDiscount> list = new List<ItemDiscount>();
                while (rs.Read())
                {
                    list.Add(new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8)));
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

        public static List<ItemDiscount> getAllByName(MySqlConnection conn, string name)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from item_discount where name like '%{name}%'", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi sản phẩm nào cả!");
                    return null;
                }
                List<ItemDiscount> list = new List<ItemDiscount>();
                while (rs.Read())
                {
                    list.Add(new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8)));
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

        public static List<ItemDiscount> getAllByIsDefault(MySqlConnection conn, bool isDefault)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from item_discount where isDefault = {isDefault}", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi sản phẩm nào cả!");
                    return null;
                }
                List<ItemDiscount> list = new List<ItemDiscount>();
                while (rs.Read())
                {
                    list.Add(new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8)));
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

        public static List<ItemDiscount> getAllByTime(MySqlConnection conn, DateTime start, DateTime end)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM `item_discount` WHERE start >= '{start.ToString("yyyy-MM-dd HH:mm:ss")}' && end <= '{end.ToString("yyyy-MM-dd HH:mm:ss")}';", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi sản phẩm nào cả!");
                    return null;
                }
                List<ItemDiscount> list = new List<ItemDiscount>();
                while (rs.Read())
                {
                    list.Add(new ItemDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetString(3), rs.GetDateTime(4), rs.GetDateTime(5), rs.GetInt16(6), rs.GetInt16(7), rs.GetBoolean(8)));
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


        public bool saveItemDiscount(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `item_discount`(`isDefault`, `name`, `info`, `start`, `end`) VALUES ({this.isDefault},'{this.name}','{this.info}','{this.start.ToString("yyyy-MM-dd HH:mm:ss")}','{this.end.ToString("yyyy-MM-dd HH:mm:ss")}')", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm khuyến mãi sản phẩm thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Thêm khuyến mãi sản phẩm không thành công!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi thêm khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool updateItemDiscount(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"UPDATE `item_discount` SET `isDefault`={this.isDefault},`name`='{this.name}',`info`='{this.info}',`start`='{this.start.ToString("yyyy-MM-dd HH:mm:ss")}',`end`='{this.end.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE id = {this.id};", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật khuyến mãi sản phẩm thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Cập nhật khuyến mãi sản phẩm không thành công!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi Cập nhật khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool deleteItemDiscount(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `item_discount`  WHERE id = {this.id};", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa khuyến mãi sản phẩm thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Xóa khuyến mãi sản phẩm không thành công!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi Xóa khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool addItemToItemDiscount(MySqlConnection conn, int item, int value)
        {
            if (conn == null) conn = View.Login.conn;
            

            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `item_discount_details`(`item_discount`, `item`, `value`) VALUES ({this.id},{item},{value})", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi thêm sản phẩm vào khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool updateItemDiscountValue(MySqlConnection conn, int discountid, int itemid, int value)
        {
            if (conn == null) conn = View.Login.conn;
            

            try
            {
                var cmd = new MySqlCommand($"UPDATE `item_discount_details` SET `value`={value} WHERE `item_discount`='{discountid}' && `item`='{itemid}';", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi cập nhật sản phẩm từ khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool deleteItemFromItemDiscount(MySqlConnection conn, int item)
        {
            if (conn == null) conn = View.Login.conn;
            
            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `item_discount_details` WHERE item_discount = {this.id} && item = {item}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi xóa sản phẩm khỏi khuyến mãi sản phẩm!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }
    }
}
