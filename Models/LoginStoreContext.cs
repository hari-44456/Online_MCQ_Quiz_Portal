using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WCE.Models {

    public class LoginStoreContext {
    
        public string ConnectionString {get;set;}

        public LoginStoreContext()  
        {  
            this.ConnectionString=null;  
        }   

        public LoginStoreContext(string connectionString) {
            this.ConnectionString=connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(ConnectionString);
        }

        public bool AuthorizeAdmin(string username,string password) {

            MySqlConnection connection = GetConnection();
            connection.Open();
            string query = "select * from admin where username=@par1 and password=@par2";
            MySqlCommand cmd= new MySqlCommand(query,connection);

            cmd.Parameters.AddWithValue(@"par1",username);
            cmd.Parameters.AddWithValue("@par2",password);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            return reader.HasRows;
        }

        public string AuthorizeTeacher(string username,string password) {
            
            MySqlConnection connection = GetConnection();
            connection.Open();
            string query = "select * from teacher where username=@par1";
            MySqlCommand cmd= new MySqlCommand(query,connection);

            cmd.Parameters.AddWithValue("@par1",username);
            
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();
            string name="Invalid";
            while(reader.Read())
                name=reader["username"].ToString();    
            return name;
        }

        public string AuthorizeStudent(string username,string password) {
            
            MySqlConnection connection = GetConnection();
            connection.Open();
            string query = "select * from login where username=@par1 and password=@par2";
            MySqlCommand cmd= new MySqlCommand(query,connection);

            cmd.Parameters.AddWithValue("@par1",username);
            cmd.Parameters.AddWithValue("@par2",password);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();
            string name="Invalid";
            while(reader.Read())
                name=reader["username"].ToString();
            return name;
        }
    }
}