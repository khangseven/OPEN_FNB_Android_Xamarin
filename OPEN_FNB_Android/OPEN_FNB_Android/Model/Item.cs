using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
//using Android.Media;
//using Android.Graphics;
using System.Drawing;
using MySqlConnector;
using Xamarin.Forms;
using Android.Graphics;

namespace OPEN_FNB_Android.Model
{
    public class Item
    {
        //public static Bitmap defaultImage = new Bitmap(0,0); 
        
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public Image image { get; set; }
        public bool isAvailable { get; set; }
        public bool isProcessed { get; set; }
        public int type { get; set; }
        public string type_name { get; set; }
        public string unit { get; set; }


        public Item(int id, string name, int price, Image image, bool isAvailable, bool isProcessed, int type, string type_name, string unit)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.image = image;
            this.isAvailable = isAvailable;
            this.isProcessed = isProcessed;
            this.type = type;
            this.type_name = type_name;
            this.unit = unit;
        }

        public Item()
        {
        }

        public string getPriceVND()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            return this.price.ToString("C0",cul.NumberFormat);
        }

        public static string intToVnd(int vnd)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            return vnd.ToString("C0", cul.NumberFormat);
        }

        public static List<Item> getAllWithCondition(MySqlConnection conn,string condition)
        {
            if (conn == null) conn = View.Login.conn;
            List<Item> list = new List<Item>();
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit from item i join item_type it on i.item_type = it.id {condition}", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy mặt hàng nào hết!");
                    rs.Close();
                    return null;
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
                        StreamReader reader = new StreamReader(rs.GetStream(3));
                        string text = reader.ReadToEnd();
                        im.Source = ImageSource.FromStream(() => rs.GetStream(3));
                    }

                    list.Add(new Item(rs.GetInt32(0), rs.GetString(1), rs.GetInt32(2),im , rs.GetBoolean(4), rs.GetBoolean(5), rs.GetInt16(6), rs.GetString(7), rs.GetString(8)));
                    
                }
                rs.Close();
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xãy ra lỗi khi lấy dữ liệu về mặt hàng{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
            //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} mặt hàng tương ứng!");
            return list;
        }
        
        public static List<Item> getAll(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            List<Item> list = new List<Item>();
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit from item i join item_type it on i.item_type = it.id",conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy mặt hàng nào hết!");
                    rs.Close();
                    return null;
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
                        im.Source = ImageSource.FromStream(() => rs.GetStream(3));
                    }//rs.IsDBNull(3) ? ImageSource.FromFile("unknow.png") : ImageSource.FromStream(() => rs.GetStream(3))
                    list.Add(new Item (rs.GetInt32(0),rs.GetString(1), rs.GetInt32(2), im, rs.GetBoolean(4), rs.GetBoolean(5),rs.GetInt16(6),rs.GetString(7),rs.GetString(8)));
                }
                rs.Close();
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xãy ra lỗi khi lấy dữ liệu về mặt hàng{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
            //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} mặt hàng tương ứng!");
            return list;
        }

        public static List<Item> getAllByName(MySqlConnection conn,string name)
        {
            if (conn == null) conn = View.Login.conn;
            List<Item> list = new List<Item>();
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit from item i join item_type it on i.item_type = it.id where i.name like '%{name}%'", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    //Controller.ToastController.addToast(3, $"Không tìm thấy mặt hàng nào hết!");
                    rs.Close();
                    return null;
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
                        im.Source = ImageSource.FromStream(() => rs.GetStream(3));
                    }
                    list.Add(new Item(rs.GetInt32(0), rs.GetString(1), rs.GetInt32(2), im, rs.GetBoolean(4), rs.GetBoolean(5), rs.GetInt16(6), rs.GetString(7), rs.GetString(8)));
                }
                rs.Close();
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xãy ra lỗi khi lấy dữ liệu về mặt hàng{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
            //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} mặt hàng tương ứng!");
            return list;
        }
        public static List<Item> getAllByType(MySqlConnection conn,int typeid)
        {
            if (conn == null) conn = View.Login.conn;
            List<Item> list = new List<Item>();
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit from item i join item_type it on i.item_type = it.id where i.item_type = {typeid};", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy mặt hàng nào hết!");
                    return null;
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
                        im.Source = ImageSource.FromStream(() => rs.GetStream(3));
                    }
                    list.Add(new Item(rs.GetInt32(0), rs.GetString(1), rs.GetInt32(2), im, rs.GetBoolean(4), rs.GetBoolean(5), rs.GetInt16(6), rs.GetString(7), rs.GetString(8)));
                }
                rs.Close();
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xãy ra lỗi khi lấy dữ liệu về mặt hàng{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
            //Controller.ToastController.addToast(1, $"Tìm thấy {list.Count} mặt hàng tương ứng!");
            return list;
        }

       

        public static Item getItemByName(MySqlConnection conn,string name)
        {
            if (conn == null) conn = View.Login.conn;
            Item item = new Item();
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit from item i join item_type it on i.item_type = it.id where i.name like '%{name}%'", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy mặt hàng tương ứng!");
                    return null;
                }

                rs.Read();
                Image im = new Image();
                if (rs.IsDBNull(3))
                {
                    im.Source = ImageSource.FromFile("unknow.png");
                }
                else
                {
                    im.Source = ImageSource.FromStream(() => rs.GetStream(3));
                }
                item = new Item(rs.GetInt32(0), rs.GetString(1), rs.GetInt32(2), im, rs.GetBoolean(4), rs.GetBoolean(5), rs.GetInt16(6), rs.GetString(7), rs.GetString(8));
                rs.Close();
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xãy ra lỗi khi lấy dữ liệu về mặt hàng{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
            //Controller.ToastController.addToast(1, $"Tìm thấy mặt hàng có tên tương ứng!");
            return item;
        }
        
        public static Item getItemById(MySqlConnection conn,int id)
        {
            if (conn == null) conn = View.Login.conn;
            Item item = new Item();
            try
            {
                MySqlCommand cmd = new MySqlCommand($"select i.id,i.name,i.price,i.image,i.isAvailable,i.isProcessed,i.item_type,it.name,it.unit from item i join item_type it on i.item_type = it.id where i.id = {id}", conn);
                var rs = cmd.ExecuteReader();
                if (!rs.HasRows)
                {
                    rs.Close();
                    //Controller.ToastController.addToast(3, $"Không tìm thấy mặt hàng tương ứng!");
                    return null;
                }
                //new Bitmap(rs.GetStream(3))
                rs.Read();
                Image im = new Image();
                if (rs.IsDBNull(3))
                {
                    im.Source = ImageSource.FromFile("unknow.png");
                }
                else
                {
                    im.Source = ImageSource.FromStream(() => rs.GetStream(3));
                }
                item = new Item(rs.GetInt32(0), rs.GetString(1), rs.GetInt32(2), im, rs.GetBoolean(4), rs.GetBoolean(5), rs.GetInt16(6), rs.GetString(7), rs.GetString(8));
                rs.Close();
            }
            catch (Exception ex)
            {
                //Controller.ToastController.addToast(4, $"Đã xãy ra lỗi khi lấy dữ liệu về mặt hàng{System.Environment.NewLine}Chi tiết: {ex.Message}");
                return null;
            }
            //Controller.ToastController.addToast(1, $"Tìm thấy mặt hàng có id={id} tương ứng!");
            //Console.WriteLine(item.isAvailable);
            return item;
        }

        
    }
}
