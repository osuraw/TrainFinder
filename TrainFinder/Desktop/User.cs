using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DEVPRO;

namespace Desktop
{
    public class User
    {
        private byte id;
        private User user;
        private string username, password, cpassword;
        private List<User> usersss;

        public User()
        {
        }

        public User(byte id, string name, string username, string password, string cpassword)
        {
            this.id = id;
            this.name = name;
            this.username = username;
            this.password = password;
            this.cpassword = cpassword;
        }

        public string name { get; private set; }

        public bool Update()
        {
            var command = new SqlCommand
                {CommandText = "update [user] set [Name]=@name, [Uname]=@uname, [Password]=@pa where [UID]=@id"};
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@uname", username);
            command.Parameters.AddWithValue("@pa", password);
            command.Parameters.AddWithValue("@id", id);
            return DB.update(command) > 0 ? true : false;
        }

        public bool Login(string username, string password)
        {
            //var rr = new proDB();
            //var users = rr.users.ToList();
            var command = new SqlCommand
                {CommandText = "select * from [user] where [Uname]=@uname and [Password]=@pass"};
            command.Parameters.AddWithValue("uname", username);
            command.Parameters.AddWithValue("pass", password);
            try
            {
                usersss = datareader(DB.GetData(command));
                DB.Close_connection();
            }
            catch (Exception e)
            {
                return false;
            }

            logininfor.user = usersss[0];
            logininfor.logtime = DateTime.Now;
            return true;
        }

        public List<User> datareader(SqlDataReader re)
        {
            usersss = new List<User>();
            if (re == null) throw new Exception("User Name and Password Invalid");
            while (re.Read())
            {
                user = new User
                {
                    id = re.GetByte(0),
                    name = re.GetString(1),
                    username = re.GetString(2),
                    password = re.GetString(3)
                };
                usersss.Add(user);
            }

            return usersss;
        }
    }
}