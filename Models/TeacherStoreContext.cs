
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WCE.Models {
    public class TeacherStoreContext {
        public string ConnectionString {get;set;}
        public TeacherStoreContext(string connectionString) {
            this.ConnectionString=connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(ConnectionString);
        }

        public Teacher getTeacherDetails(int id){
            Teacher t = new Teacher();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from teacher where teacher_id=@par",conn);

            cmd.Parameters.AddWithValue("@par",id);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            if(reader.Read()){
                t.teacher_id = (int)reader["teacher_id"];
                t.name = (string)reader["name"];
            }

            return t;
        }
        
        public int Login(string username, string password){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from teacher where username=@par1 and password=@par2",conn);

            cmd.Parameters.AddWithValue("@par1",username);
            cmd.Parameters.AddWithValue("@par2",password);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();
            int name=-1;
            while(reader.Read())
                name=(int)reader["teacher_id"];
            
            conn.Close();
            return name;
        }

        public void AddTeacher(string name,string username,string password) {
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("insert into teacher(name,username,password) values(@p1,@p2,@p3)",conn);

            cmd.Parameters.AddWithValue("@p1",name);
            cmd.Parameters.AddWithValue("@p2",username);
            cmd.Parameters.AddWithValue("@p3",password);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public List<Test> getTests(int teacher_id){
            List<Test> l = new List<Test>();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from test where teacher_id=@par",conn);

            cmd.Parameters.AddWithValue("@par",teacher_id);
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

        public int CreateTest(int id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("insert into test(teacher_id) values (@par1)",conn);

            cmd.Parameters.AddWithValue("@par1",id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            MySqlCommand cmd1=new MySqlCommand("select max(test_id) as test_id from test where teacher_id=@par",conn);

            cmd1.Parameters.AddWithValue("@par",id);
            cmd1.Prepare();

            MySqlDataReader reader = cmd1.ExecuteReader();

            int test_id=-1;
            while(reader.Read()) {
                test_id=(int)reader["test_id"];
            }

            conn.Close();

            return test_id;
        }

        public void DeleteTest(int test_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("delete from question where test_id=@par1;",conn);

            cmd.Parameters.AddWithValue("@par1",test_id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            MySqlCommand cmd1=new MySqlCommand("delete from test where test_id=@par2",conn);

            cmd1.Parameters.AddWithValue("@par2",test_id);
            cmd1.Prepare();
            cmd1.ExecuteNonQuery();
        }
    }
}