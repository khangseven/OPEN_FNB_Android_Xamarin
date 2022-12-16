
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    public class BillDiscount
    {
        public int id { get; set; }
        public bool isDefault { get; set; }
        public string name { get; set; }
        public int value { get; set; }
        public string info  { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int applied  { get; set; }
        public int limit { get; set; }
        public bool isAvailable     { get; set; }

        public string dathem { get; set; }

        public BillDiscount(int id, bool isDefault, string name, int value, string info, DateTime start, DateTime end, int applied, int limit, bool isAvailable)
        {
            this.id = id;
            this.isDefault = isDefault;
            this.name = name;
            this.value = value;
            this.info = info;
            this.start = start;
            this.end = end;
            this.applied = applied;
            this.limit = limit;
            this.isAvailable = isAvailable;
        }

        public BillDiscount(bool isDefault, string name, int value, string info, DateTime start, DateTime end)
        {
            this.isDefault = isDefault;
            this.name = name;
            this.value = value;
            this.info = info;
            this.start = start;
            this.end = end;
        }


        public BillDiscount() { }

        //Get Update Save Delete
        public static List<BillDiscount> getAll(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from bill_discount", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi đơn hàng nào cả!");
                    return null;
                }
                List<BillDiscount> list  = new List<BillDiscount>();
                while (rs.Read())
                {
                    list.Add(new BillDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4), rs.GetDateTime(5), rs.GetDateTime(6), rs.GetInt16(7), rs.GetInt16(8), rs.GetBoolean(9)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi đơn hàng tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<BillDiscount> getAllAvailable(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from bill_discount bd where bd.start < CURRENT_DATE && bd.end>CURRENT_DATE && bd.isAvailable=true ", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi đơn hàng nào cả!");
                    return null;
                }
                List<BillDiscount> list = new List<BillDiscount>();
                while (rs.Read())
                {
                    list.Add(new BillDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4), rs.GetDateTime(5), rs.GetDateTime(6), rs.GetInt16(7), rs.GetInt16(8), rs.GetBoolean(9)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi đơn hàng tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<BillDiscount> getAllByName(MySqlConnection conn,string name)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from bill_discount where name like '%{name}%'", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi đơn hàng nào cả!");
                    return null;
                }
                List<BillDiscount> list = new List<BillDiscount>();
                while (rs.Read())
                {
                    list.Add(new BillDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4), rs.GetDateTime(5), rs.GetDateTime(6), rs.GetInt16(7), rs.GetInt16(8), rs.GetBoolean(9)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi đơn hàng tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<BillDiscount> getAllByIsDefault(MySqlConnection conn,bool isDefault)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select * from bill_discount where isDefault = {isDefault}", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi đơn hàng nào cả!");
                    return null;
                }
                List<BillDiscount> list = new List<BillDiscount>();
                while (rs.Read())
                {
                    list.Add(new BillDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4), rs.GetDateTime(5), rs.GetDateTime(6), rs.GetInt16(7), rs.GetInt16(8), rs.GetBoolean(9)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi đơn hàng tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<BillDiscount> getAllByTime(MySqlConnection conn, DateTime start, DateTime end)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM `bill_discount` WHERE start >= '{start.ToString("yyyy-MM-dd HH:mm:ss")}' && end <= '{end.ToString("yyyy-MM-dd HH:mm:ss")}';", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy khuyến mãi đơn hàng nào cả!");
                    return null;
                }
                List<BillDiscount> list = new List<BillDiscount>();
                while (rs.Read())
                {
                    list.Add(new BillDiscount(rs.GetInt16(0), rs.GetBoolean(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4), rs.GetDateTime(5), rs.GetDateTime(6), rs.GetInt16(7), rs.GetInt16(8), rs.GetBoolean(9)));
                }
                rs.Close();
                //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} khuyến mãi đơn hàng tương ứng!");
                return list;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi tìm khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public bool saveBillDiscount(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `bill_discount`(`isDefault`, `name`, `value`, `info`, `start`, `end`,`applied`, `litmit`, `isAvailable`) VALUES ({this.isDefault},'{this.name}',{this.value},'{this.info}','{this.start.ToString("yyyy-MM-dd HH:mm:ss")}','{this.end.ToString("yyyy-MM-dd HH:mm:ss")}', {this.applied}, {this.limit}, {this.isAvailable})",conn);
                var rs =cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm khuyến mãi đơn hàng thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Thêm khuyến mãi đơn hàng không thành công!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi thêm khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool updateBillDiscount(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"UPDATE `bill_discount` SET `isDefault`={this.isDefault},`name`='{this.name}',`value`={this.value},`info`='{this.info}',`start`='{this.start.ToString("yyyy-MM-dd HH:mm:ss")}',`end`='{this.end.ToString("yyyy-MM-dd HH:mm:ss")}', `applied`={this.applied},`litmit`={this.limit},`isAvailable`={this.isAvailable} WHERE id = {this.id};", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật khuyến mãi đơn hàng thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Cập nhật khuyến mãi đơn hàng không thành công!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi Cập nhật khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool deleteBillDiscount(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `bill_discount` WHERE id = {this.id};", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa khuyến mãi đơn hàng thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Xóa nhật khuyến mãi đơn hàng không thành công!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Lỗi khi Xóa khuyến mãi đơn hàng!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }
    }
}
