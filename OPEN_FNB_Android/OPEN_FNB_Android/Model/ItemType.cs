
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    internal class ItemType
    {
		public int id { get; set; }
		public string name { get; set; }
		public string unit { get; set; }

		public ItemType(int id_, string name_, string unit_)
		{
			this.id = id_;
			this.name = name_;
			this.unit = unit_;
		}

        public ItemType(string name, string unit)
        {
            this.name = name;
            this.unit = unit;
        }

        public ItemType() { }

        public static List<ItemType> getAll(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            List<ItemType> tables = new List<ItemType>();

            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from item_type;", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows) return null;

                while (rs.Read())
                {
                    tables.Add(new ItemType(rs.GetInt16(0), rs.GetString(1), rs.GetString(2)));
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

        public bool updateItemType(MySqlConnection conn = null)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"UPDATE `item_type` SET `name`='{this.name}',`unit`='{this.unit}' WHERE id = {this.id}", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Cập nhật loại mặt hàng {this.name} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Cập nhật loại mặt hàng {this.name} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Cập nhật loại mặt hàng {this.name} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };
        }

        public bool saveItemType(MySqlConnection conn = null)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `item_type`(`name`, `unit`) VALUES ('{this.name}','{this.unit}')", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addToast(2, $"Thêm loại mặt hàng {this.name} thành công!");
                    return true;
                }
                //Controller.ToastController.addToast(4, $"Thêm loại mặt hàng {this.name} thất bại!");
                return false;
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Thêm loại mặt hàng {this.name} thất bại!{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return false;
            };

        }

    }
}
