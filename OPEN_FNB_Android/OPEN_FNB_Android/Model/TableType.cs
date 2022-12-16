
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    public class TableType
    {
        public int id { get; set; }
        public string name { get; set; }

        public TableType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public TableType(string name)
        {
            this.name = name;
        }

        public static List<TableType> getAll(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            List<TableType> tables = new List<TableType>();

            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from table_type where id!=3;", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows) return null;

                while (rs.Read())
                {
                    tables.Add(new TableType(rs.GetInt16(0), rs.GetString(1)));
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

        public bool updateTableType(MySqlConnection conn = null)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"UPDATE `table_type` SET `name`='{this.name}' WHERE id = {this.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật loại bàn {this.name} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Cập nhật loại bàn {this.name} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Cập nhật loại bàn {this.name} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };
        }

        public bool saveTableType(MySqlConnection conn = null)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `table_type`(`name`) VALUES ('{this.name}')", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm loại bàn {this.name} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Thêm loại bàn {this.name} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Thêm loại bàn {this.name} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };

        }

        public bool deleteTableType(MySqlConnection conn = null)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"DELETE FROM `table_type` WHERE id = {this.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm loại bàn {this.name} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Thêm loại bàn {this.name} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Thêm loại bàn {this.name} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };
        }
    }
}
