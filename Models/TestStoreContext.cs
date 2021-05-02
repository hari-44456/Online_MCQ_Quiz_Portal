
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WCE.Models {
    public class TestStoreContext {
        public string ConnectionString {get;set;}
        public TestStoreContext(string connectionString) {
            this.ConnectionString=connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(ConnectionString);
        }

        public int SaveTestDetails(int id, string test_title, string test_password, string duration){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("update test set test_title=@par1, test_password=@par2, duration=@par3, isActive=@par5 where test_id=@par4;",conn);

            float dur = float.Parse(duration);
            string active="YES";

            cmd.Parameters.AddWithValue("@par1",test_title);
            cmd.Parameters.AddWithValue("@par2",test_password);
            cmd.Parameters.AddWithValue("@par3",dur);
            cmd.Parameters.AddWithValue("@par4",id);
            cmd.Parameters.AddWithValue("@par5",active);

            cmd.Prepare();
            cmd.ExecuteNonQuery();

            return id;
        }

        public void SetTestActive(int test_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("update test set isActive=@par1 where test_id=@par2;",conn);

            string active="YES";
            
            cmd.Parameters.AddWithValue("@par1",active);
            cmd.Parameters.AddWithValue("@par2",test_id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void SetTestInactive(int test_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("update test set isActive=@par1 where test_id=@par2;",conn);

            string active="NO";
            
            cmd.Parameters.AddWithValue("@par1",active);
            cmd.Parameters.AddWithValue("@par2",test_id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public Test GetTestDetails(int test_id){
            Test t =new Test();
            
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from test where test_id=@var",conn);

            cmd.Parameters.AddWithValue("@var",test_id);

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read()) {
                t.test_title=(string)reader["test_title"];
                t.test_password=(string)reader["test_password"];
                t.duration=(float)reader["duration"];
                if(reader["total_points"]!=DBNull.Value)
                t.total_points=(int)reader["total_points"];
            }

            return t;
        }

        public void SetTotalMarks(int test_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("update test set total_points=(select sum(points) as total from question where test_id=@var) where test_id=@var2",conn);

            cmd.Parameters.AddWithValue("@var",test_id);
            cmd.Parameters.AddWithValue("@var2",test_id);

            cmd.ExecuteNonQuery();
        }

        public int GetTotalMarks(int test_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select sum(points) as total from question where test_id=@var",conn);

            cmd.Parameters.AddWithValue("@var",test_id);

            int total=0;

            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()) {
                total=Convert.ToInt32(reader["total"]);
            }
            return total;
        }

        public List<Test> GetAllTests(){
            List<Test> l = new List<Test>();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from test where isActive=@par",conn);

            string isActive="YES";
            cmd.Parameters.AddWithValue("@par",isActive);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read()) {
                l.Add(new Test(){
                    test_id=(int)reader["test_id"],
                    teacher_id=(int)reader["teacher_id"],
                    test_title=(string)reader["test_title"],
                    total_points=(int)reader["total_points"],
                    duration=(float)reader["duration"],
                    isActive=(string)reader["isActive"]
                });
            }

            return l;
        }

        public int GetTestDetails(string test_title, string test_password){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from test where test_title=@par1 and test_password=@par2",conn);

            cmd.Parameters.AddWithValue("@par1",test_title);
            cmd.Parameters.AddWithValue("@par2",test_password);

            MySqlDataReader reader = cmd.ExecuteReader();

            int id=-1;
            while(reader.Read()){
                id=(int)reader["test_id"];
            }
            return id;
        }

        public string GetTestTitle(int test_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select test_title from test where test_id=@par",conn);

            cmd.Parameters.AddWithValue("@par",test_id);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            string test_title="";
            while(reader.Read()) {
                test_title=(string)reader["test_title"];
            }

            return test_title;
        }
    }
}