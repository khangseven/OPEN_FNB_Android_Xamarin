using System;
using System.Collections.Generic;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    public class Table
    {
        public int id { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public int type { get; set; }
        public string type_name { get; set; }
        public int bill { get; set; }
        public string isActive1 { get; set; }
        public string isActive2 { get; set; }

        public string bill_info { get; set; }
        public string isTAW1 { get; set; }
        public string isTAW2 { get; set; }


        public Table(int id, string name, string info, int type, string type_name)
        {
            this.id = id;
            this.name = name;
            this.info = info;
            this.type = type;
            this.type_name = type_name;
        }

        public Table(int id, string name, string info, int type, string type_name, int bill)
        {
            this.id = id;
            this.name = name;
            this.info = info;
            this.type = type;
            this.type_name = type_name;
            this.bill = bill;
            if (bill == -1)
            {
                this.isActive1 = "false";
                this.isActive2 = "true";
            }
            else
            {
                this.isActive2 = "false";
                this.isActive1 = "true";
            }
            if (id == 0)
            {
                this.isTAW1 = "true";
                this.isTAW2 = "false";
            }
            else
            {
                this.isTAW1 = "false";
                this.isTAW2 = "true";
            }
        }

        public Table(string name, string info, int type, string type_name)
        {
            this.name = name;
            this.info = info;
            this.type = type;
            this.type_name = type_name;
        }

        public Table(string name,  int type)
        {
            this.name = name;
            this.type = type;
        }

        public Table() { }

        ///<summary>
        /// b.id is not null | order by [t.name,b.id]
        ///</summary>
        public static List<Table> getAllWithCondition(MySqlConnection conn,string condition)
        {
            if (conn == null) conn = View.Login.conn;
            List<Table> tables = new List<Table>();

            try
            {
                MySqlCommand cmd = new MySqlCommand($"select t.id,t.name,t.info,tt.id as typeid, tt.name as typename,b.id as bill,b.customer_reviews as bill_info from tables t join table_type tt on t.table_type = tt.id left join (select id,table_id,customer_reviews from bill where complete=false ) b on b.table_id = t.id where t.name like '%%'" + condition, conn);

                Console.WriteLine($"select t.id,t.name,t.info,tt.id as typeid, tt.name as typename,b.id as bill from tables t join table_type tt on t.table_type = tt.id left join (select id,table_id from bill where complete=false ) b on b.table_id = t.id where t.name like '%%'" + condition);
                
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    return null;
                }

                while (rs.Read())
                {
                    var tb = new Table(rs.GetInt16(0), rs.GetString(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4), rs.IsDBNull(5) ? -1 : rs.GetInt32(5));
                    tb.bill_info = rs.IsDBNull(6) ? "" : rs.GetString(6);
                    tables.Add(tb);
                }
                rs.Close();
            }
            catch (Exception e)
            {
                //Controller.ToastController.addToast(4, e.Message);
                Console.WriteLine(e.Message);
                return tables;
            }

            return tables;
        }

        public static List<Table> getAll(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            List<Table> tables = new List<Table>();

            try
            {
                MySqlCommand cmd = new MySqlCommand("select t.id,t.name,t.info,tt.id as typeid, tt.name as typename from tables t join table_type tt on t.table_type = tt.id;", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    return null;
                }

                while (rs.Read())
                {
                    tables.Add(new Table(rs.GetInt16(0), rs.GetString(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4)));
                }
                rs.Close();
            }
            catch (Exception e)
            {
                //Controller.ToastController.addToast(4, e.Message);
                Console.WriteLine(e.Message);
                return tables;
            }

            return tables;
        }

        public static Table getTableById(MySqlConnection conn, int id)
        {
            if (conn == null) conn = View.Login.conn;

            MySqlCommand cmd = new MySqlCommand($"select t.id,t.name,t.info,tt.id as typeid, tt.name as typename,b.id as bill from tables t join table_type tt on t.table_type = tt.id left join (select id,table_id from bill where complete=false ) b on b.table_id = t.id where t.name like '%%' && t.id = {id} ;",conn);
            var rs = cmd.ExecuteReader();

            if (!rs.HasRows)
            {
                rs.Close();
                return null;
            }

            rs.Read();
            Table table = new Table(rs.GetInt16(0), rs.GetString(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4), rs.IsDBNull(5) ? -1 : rs.GetInt32(5));
            rs.Close();

            return table;
        }
        public static Table getTableByName(MySqlConnection conn , string name)
        {
            if (conn == null) conn = View.Login.conn;

            MySqlCommand cmd = new MySqlCommand($"select t.id,t.name,t.info,tt.id as typeid, tt.name as typename from tables t join table_type tt on t.table_type = tt.id where t.name like '%{name}%' ;", conn);
            var rs = cmd.ExecuteReader();

            if (!rs.HasRows)
            {
                rs.Close();
                return null;
            }

            rs.Read();
            Table table = new Table(rs.GetInt16(0), rs.GetString(1), rs.GetString(2), rs.GetInt16(3), rs.GetString(4));
            rs.Close();

            return table;
        }

        public bool updateTable(MySqlConnection conn = null , Table table = null)
        {
            if (conn == null) conn = View.Login.conn;
            if (table == null) table = this;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"update tables set name = '{table.name}', table_type = {table.type} where id = {table.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật bàn có id={table.id} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Cập nhật bàn có id={table.id} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Cập nhật bàn có id={table.id} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };
        }

        public bool saveTable(MySqlConnection conn = null)
        {
            if (conn == null) conn = View.Login.conn;

            try
            {
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `tables`(`table_type`, `name`) VALUES ({this.type},'{this.name}')", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm bàn {this.name} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Thêm bàn {this.name} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Thêm bàn {this.name} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };

        }

        public bool deleteTable(MySqlConnection conn = null)
        {
            if (conn == null) conn = View.Login.conn;

            try
            {
                MySqlCommand cmd = new MySqlCommand($"DELETE FROM `tables` WHERE id={this.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Xóa bàn {this.name} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Xóa bàn {this.name} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Xóa bàn {this.name} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };

        }


    }
}
