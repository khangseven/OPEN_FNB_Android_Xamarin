using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OPEN_FNB_Android.Model
{
    [Serializable]
    public class User
    {
        public int id {get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string info { get; set; }
        public int role { get; set; }

        public User(int id, string name, string password, string info, int role)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.info = info;
            this.role = role;
        }

        public User( string name, string password, string info, int role)
        {
            this.name = name;
            this.password = password;
            this.info = info;
            this.role = role;
        }

        public User() { }

        public static bool checkUserByID(MySqlConnection conn, int userID)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"SELECT id FROM user WHERE id={userID};", conn);
                var rs = cmd.ExecuteNonQuery();
                Console.WriteLine(rs);
                if (rs == 1)
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static bool checkUserByName(MySqlConnection conn, string name)
        {
            if (conn == null) conn = View.Login.conn;
            try
            {
                var cmd = new MySqlCommand($"SELECT id FROM user WHERE username={name};", conn);
                var rs = cmd.ExecuteNonQuery();
                Console.WriteLine(rs);
                if (rs == 1)
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static User getUser(MySqlConnection conn, int id)
        {
            if (conn == null) conn = View.Login.conn;
            User user = new User();
            try
            {
                var cmd = new MySqlCommand($"SELECT * FROM user WHERE id={id};", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    Console.WriteLine("lay user");
                    rs.Read();
                    user = new User(rs.GetInt32(0),rs.GetString(1),rs.GetString(2),rs.GetString(3),rs.GetInt32(4));
                    rs.Close();
                    Console.WriteLine(user.name);
                }
                else
                {
                    user = null;
                }
            }
            catch (Exception e)
            {  
                Console.WriteLine(e.Message);
                return null;
            }
            return user;
        }

        public static List<User> getAllNoAd(MySqlConnection conn)
        {
            if (conn == null) conn = View.Login.conn;
            var list = new List<User>();
            try
            {
                var cmd = new MySqlCommand($"SELECT * FROM user WHERE id!=1;", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    while (rs.Read())
                    {
                        var user = new User(rs.GetInt32(0), rs.GetString(1), rs.GetString(2), rs.GetString(3), rs.GetInt32(4));
                        list.Add(user);
                    }
                    
                    rs.Close();
                    return list;
                }
                else
                {
                    rs.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                //Controller.ToastController.addToast(4, $"Lối khi tìm người dùng!{System.Environment.NewLine}Chi tiết: {e.Message}");
                return null;
            }
        }

        public static bool userLogin(MySqlConnection conn ,out User user, string username, string password)
        {
            if (conn == null) conn = View.Login.conn;
            user = new User();
            try
            {
                var cmd = new MySqlCommand($"SELECT * FROM user WHERE username='{username}' && password = '{password}';", conn);
                var rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    user = new User(rs.GetInt32(0), rs.GetString(1), rs.GetString(2), rs.GetString(3), rs.GetInt32(4));
                    rs.Close();
                    return true;
                }
                else
                {
                    rs.Close();
                    user = null;
                    return false;
                }
            }
            catch (Exception e)
            {
                user = null;
                Console.WriteLine(e.Message);
            }
            return false;

        }

        public bool saveUser(MySqlConnection conn,User user=null)
        {
            if (conn == null) conn = View.Login.conn;
            if (user == null) user = this;

            

            try
            {
                var cmd = new MySqlCommand($"INSERT INTO `user`(`username`, `password`, `info`, `role`) VALUES " +
                    $"('{user.name}','{user.password}','{user.info}',{user.role});", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addSureToast(2, "Thêm người dùng thành công");
                    return true;
                    
                }
                    
                else
                {
                    //Controller.ToastController.addSureToast(3, "Thêm người dùng thất bại");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Controller.ToastController.addSureToast(3, $"Thêm người dùng thất bại! {Environment.NewLine} {ex.Message.ToString()}");
                return false;
            }

        }

        public bool deleteUser(MySqlConnection conn, User user = null)
        {
            if (conn == null) conn = View.Login.conn;
            if (user == null) user = this;



            try
            {
                var cmd = new MySqlCommand($"DELETE FROM `user` WHERE id = {user.id};", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addSureToast(2, "Xóa người dùng thành công");
                    return true;

                }

                else
                {
                    //Controller.ToastController.addSureToast(3, "Xóa người dùng thất bại");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Controller.ToastController.addSureToast(3, $"Xóa người dùng thất bại! {Environment.NewLine} {ex.Message.ToString()}");
                return false;
            }

        }

        public bool updateUser(MySqlConnection conn, User user = null)
        {
            if (conn == null) conn = View.Login.conn;
            if (user == null) user = this;



            try
            {
                var cmd = new MySqlCommand($"UPDATE `user` SET `username`='{user.name}',`password`='{user.password}',`info`='{user.info}',`role`='{user.role}' WHERE id = {user.id};", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addSureToast(2, "Cập nhật người dùng thành công");
                    return true;

                }

                else
                {
                    //Controller.ToastController.addSureToast(3, "Cập nhật người dùng thất bại");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Controller.ToastController.addSureToast(3, $"Cập nhật người dùng thất bại! {Environment.NewLine} {ex.Message.ToString()}");
                return false;
            }

        }

        public static bool updateAdmin(MySqlConnection conn,string pass)
        {
            if (conn == null) conn = View.Login.conn;



            try
            {
                var cmd = new MySqlCommand($"UPDATE `user` SET `password`='{pass}' WHERE id = 1;", conn);
                var rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                {
                    //Controller.ToastController.addSureToast(2, "Cập nhật admin thành công");
                    return true;

                }

                else
                {
                    //Controller.ToastController.addSureToast(3, "Cập nhật admin thất bại");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Controller.ToastController.addSureToast(3, $"Cập nhật admin thất bại! {Environment.NewLine} {ex.Message.ToString()}");
                return false;
            }

        }

    }
}
