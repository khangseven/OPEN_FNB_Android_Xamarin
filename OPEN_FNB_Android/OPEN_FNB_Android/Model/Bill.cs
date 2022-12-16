using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    public class Bill
    {

		public int id { get; set; }
		public int table_id { get; set; }
		public int bill_creator { get; set; }
		public DateTime checkin { get; set; }
		public DateTime checkout { get; set; }
		public int total { get; set; }
		public bool complete { get; set; }
		public string customer_reviews { get; set; }
        public string username  { get; set; }
        public int role { get; set; }
        public string table_name { get; set; }
        public string table_type_name { get; set; }

        public List<BillDiscount> discounts { get; set; }    

        public Bill(int id, int table_id, int bill_creator, DateTime checkin, DateTime checkout, int total, bool complete, string customer_reviews, string username, int role, string table_name, string table_type_name)
        {
            this.id = id;
            this.table_id = table_id;
            this.bill_creator = bill_creator;
            this.checkin = checkin;
            this.checkout = checkout;
            this.total = total;
            this.complete = complete;
            this.customer_reviews = customer_reviews;
            this.username = username;
            this.role = role;
            this.table_name = table_name;
            this.table_type_name = table_type_name;
            this.discounts = new List<BillDiscount>();
            this.discounts = getAllDiscountOfBill(null,this.id);
        }

        public Bill(int table_id, int bill_creator, DateTime checkin, DateTime checkout, int total, bool complete, string customer_reviews)
        {
            this.table_id = table_id;
            this.bill_creator = bill_creator;
            this.checkin = checkin;
            this.checkout = checkout;
            this.total = total;
            this.complete = complete;
            this.customer_reviews = customer_reviews;
        }

        public Bill() { }

        public static Bill getBillByID(MySqlConnection conn,int id)
        {
            if (conn == null) conn = View.Login.conn;
            Bill bill = null;
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where b.id = {id};", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    /*rs.Read();
                    bill = new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11));
                    rs.Close();*/
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    var row = dt.Rows[0];
                    bill=new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]);
                    return bill;



                }else
                    rs.Close();
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAllWithCondition(MySqlConnection conn, string condition)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where  {condition};", conn);
                Console.WriteLine("hello123312");
                var rs = cmd.ExecuteReader();
                Console.WriteLine("hello");
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7]==null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));
                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");
                    
                    return list;
                }
                else
                {
                    rs.Close();
                }
                
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAll(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>(); 
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type;", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    /*while (rs.Read())
                    {
                        list.Add(new Bill(rs.GetInt16(0),rs.GetInt16(1),rs.GetInt16(2),rs.GetDateTime(3),rs.GetDateTime(4),rs.GetInt32(5),rs.GetBoolean(6),rs.IsDBNull(7) ? "" : rs.GetString(7),rs.GetString(8),rs.GetInt16(9),rs.GetString(10),rs.GetString(11)));
                    }
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");
                    return list;*/

                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3,$"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAllByTime(MySqlConnection conn, DateTime from, DateTime to)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join " +
                    $"table_type tp on tp.id = t.table_type where b.checkin > '{from.ToString("yyyy-MM-dd HH:mm:ss")}' && b.checkout < '{to.ToString("yyyy-MM-dd HH:mm:ss")}' ;", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAllByDay(MySqlConnection conn, DateTime date)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where b.checkin like '{date.ToString("yyyy-MM-dd")}%';", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAllByMonth(MySqlConnection conn, DateTime month)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where b.checkin like '{month.ToString("yyyy-MM-")}%';", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAllByYear(MySqlConnection conn, DateTime year)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where b.checkin like '{year.ToString("yyyy-")}%';", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAllByMaxValue(MySqlConnection conn, int value)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where {value} > b.total ;", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }
        
        public static List<Bill> getAllByMinValue(MySqlConnection conn, int value)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where {value} < b.total ;", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }

        public static List<Bill> getAllBetweenValue(MySqlConnection conn, int max, int min)
        {
            if (conn == null) conn = View.Login.conn;
            List<Bill> list = new List<Bill>();
            try
            {
                var cmd = new MySqlCommand($"select b.*,u.username,u.role,t.name,tp.name from bill b join user u on b.bill_creator = u.id join tables t on t.id = b.table_id join table_type tp on tp.id = t.table_type where {max} > b.total  &&  {min} < b.total;", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(rs);
                    rs.Close();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        //list.Add(new Bill(rs.GetInt16(0), rs.GetInt16(1), rs.GetInt16(2), rs.GetDateTime(3), rs.IsDBNull(4) ? new DateTime() : rs.GetDateTime(4), rs.GetInt32(5), rs.GetBoolean(6), rs.IsDBNull(7) ? "" : rs.GetString(7), rs.GetString(8), rs.GetInt16(9), rs.GetString(10), rs.GetString(11)));
                        list.Add(new Bill((int)row[0], (int)row[1], (int)row[2], (DateTime)row[3], (DateTime)row[4], (int)row[5], (bool)row[6], row[7] == null ? "" : (string)row[7], (string)row[8], (int)row[9], (string)row[10], (string)row[11]));

                    }
                    //Controller.ToastController.addToast(3, $"Tìm thấy {list.Count} hóa đơn tương ứng!");

                    return list;
                }
                else
                {
                    rs.Close();
                }
                //Controller.ToastController.addToast(3, $"Không tìm thấy hóa đơn nào hết");
                return null;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi trong hóa trình tìm hóa đơn{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
        }
    
    
        public bool saveBill(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `bill`(`table_id`, `bill_creator`, `checkin`, `checkout`, `total`, `complete`, `customer_reviews`) " +
                                            $"VALUES ({this.table_id},{this.bill_creator},'{this.checkin.ToString("yyyy-MM-dd HH:mm:ss")}','{this.checkout.ToString("yyyy-MM-dd HH:mm:ss")}',{this.total},{this.complete},'{this.customer_reviews}')",conn);
                var rs = cmd.ExecuteNonQuery();
                if(rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm hóa đơn thành công");
                    return true;
                }
                //Controller.ToastController.addToast(2, $"Thêm hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi thêm hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }
        
        public bool updateBill(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"UPDATE `bill` SET `table_id`={this.table_id},`bill_creator`={this.bill_creator},`checkin`='{this.checkin.ToString("yyyy-MM-dd HH:mm:ss")}',`checkout`='{this.checkout.ToString("yyyy-MM-dd HH:mm:ss")}',`total`={this.total},`complete`={this.complete},`customer_reviews`='{this.customer_reviews}' where id = {this.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật hóa đơn thành công");
                    return true;
                }
                //Controller.ToastController.addToast(2, $"Cập nhật hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi Cập nhật hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool deleteBill(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `bill` WHERE id= {this.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa hóa đơn thành công");
                    return true;
                }
                //Controller.ToastController.addToast(2, $"Xóa hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xảy ra lỗi trong khi Xóa hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }



        public List<BillDiscount> getAllDiscountOfBill(MySqlConnection conn, int billid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select bd.* from bill b join list_bill_discount lbd on b.id = lbd.bill join bill_discount bd on bd.id = lbd.bill_discount where b.id ={billid}", conn);
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
    
        public bool addDiscountToBill(MySqlConnection conn, int discountid ,int billid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `list_bill_discount`(`bill`, `bill_discount`) VALUES ({billid},{discountid})", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm khuyến mãi vào hóa đơn thành công!");
                    this.discounts = getAllDiscountOfBill(null, this.id);
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Thêm khuyến mãi vào hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi thêm khuyến mãi vào hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }
        
        public bool deleteDiscountToBill(MySqlConnection conn, int discountid, int billid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `list_bill_discount` WHERE bill = {billid} && bill_discount = {discountid}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa khuyến mãi vào hóa đơn thành công!");
                    this.discounts = getAllDiscountOfBill(null, this.id);
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Xóa khuyến mãi vào hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi xóa khuyến mãi vào hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public bool deleteALlDiscountofBill(MySqlConnection conn,int billid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `list_bill_discount` WHERE bill = {billid}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa Tất cả khuyến mãi vào hóa đơn thành công!");
                    this.discounts = getAllDiscountOfBill(null, this.id);
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Xóa Tất cả khuyến mãi vào hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi xóa Tất cả khuyến mãi vào hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }

        public static bool updateTotalBill(MySqlConnection conn, int billid)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                //var cmd = new MySqlCommand($"update bill set total = round(((select sum(bd.last_price) as cal1 from bill b join bill_details bd on b.id=bd.bill where b.id={billid})-(select sum(bd.last_price) as cal1 from bill b join bill_details bd on b.id=bd.bill where b.id={billid})*((select if(COALESCE(sum(bd.value),0)>100,100,COALESCE(sum(bd.value),0)) as cal2 from bill b join list_bill_discount lbd on b.id = lbd.bill join bill_discount bd on bd.id = lbd.bill_discount where b.id={billid})/100)),-3) where id={billid};", conn);
                var cmd = new MySqlCommand($"update bill set total =  round((COALESCE((select sum(bd.last_price) as cal1 from bill b join bill_details bd on b.id=bd.bill where b.id={billid} && bd.item!=-1),0)-COALESCE((select sum(bd.last_price) as cal1 from bill b join bill_details bd on b.id=bd.bill where b.id={billid} && bd.item!=-1),0)*((select if(COALESCE(sum(bd.value),0)>100,100,COALESCE(sum(bd.value),0)) as cal2 from bill b join list_bill_discount lbd on b.id = lbd.bill join bill_discount bd on bd.id = lbd.bill_discount where b.id={billid})/100)) + COALESCE((select sum(bd.last_price) as cal1 from bill b join bill_details bd on b.id=bd.bill where b.id={billid} && bd.item=-1),0),-3) where id={billid};", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Tính tổng hóa đơn thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(3, $"Tính tổng hóa đơn thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã có lỗi xảy ra khi Tính tổng hóa đơn!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            }
        }
    }
}
