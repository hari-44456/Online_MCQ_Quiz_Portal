
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WCE.Models {
    public class TestAttemptsStoreContext {
        public string ConnectionString {get;set;}
        public TestAttemptsStoreContext(string connectionString) {
            this.ConnectionString=connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(ConnectionString);
        }

        public void EnterAttempt(int student_id, int test_id, int marks, string test_title,string student_name){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd1=new MySqlCommand("insert into testattempts (student_id,test_id,score,total_score,test_title,name) values (@par1,@par2,@par3,@par6,@par7,@par8);",conn);

            MySqlCommand cmd2=new MySqlCommand("select sum(points) as total_marks from answer where test_id=@par4 and student_id=@par5;",conn);

            Console.WriteLine(student_id);
            Console.WriteLine(test_id);

            cmd1.Parameters.AddWithValue("@par1",student_id);
            cmd1.Parameters.AddWithValue("@par2",test_id);
            cmd2.Parameters.AddWithValue("@par4",test_id);
            cmd2.Parameters.AddWithValue("@par5",student_id);
            cmd1.Parameters.AddWithValue("@par6",marks);
            cmd1.Parameters.AddWithValue("@par7",test_title);
            cmd1.Parameters.AddWithValue("@par8",student_name);

            cmd2.Prepare();
            MySqlDataReader reader = cmd2.ExecuteReader();
            int total_marks=0;

            while(reader.Read()) {
                total_marks=Convert.ToInt32(reader["total_marks"]);
            }
            reader.Close();
            cmd1.Parameters.AddWithValue("@par3",total_marks);
            cmd1.Prepare();
            cmd1.ExecuteNonQuery();
        }

        public List<TestAttempts> GetTestAttemptsForStudent(int student_id){
            List<TestAttempts> l=new List<TestAttempts>();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd1=new MySqlCommand("select * from testattempts where student_id=@par",conn);

            cmd1.Parameters.AddWithValue("@par",student_id);
            
            cmd1.Prepare();
            MySqlDataReader reader = cmd1.ExecuteReader();
            
            while(reader.Read()) {
                l.Add(new TestAttempts(){
                    student_id=(int)reader["student_id"],
                    test_id=(int)reader["test_id"],
                    score=(int)reader["score"],
                    total_score=(int)reader["total_score"],
                    test_title=reader["test_title"].ToString(),
                    name=reader["name"].ToString(),
                });
            }
            return l;
        }

        public List<TestAttempts> GetTestAttemptsForTeacher(int test_id){
            List<TestAttempts> l=new List<TestAttempts>();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd1=new MySqlCommand("select * from testattempts where test_id=@par order by score DESC;",conn);

            cmd1.Parameters.AddWithValue("@par",test_id);
            
            cmd1.Prepare();
            MySqlDataReader reader = cmd1.ExecuteReader();
            
            while(reader.Read()) {
                l.Add(new TestAttempts(){
                    student_id=(int)reader["student_id"],
                    test_id=(int)reader["test_id"],
                    score=(int)reader["score"],
                    total_score=(int)reader["total_score"],
                    test_title=reader["test_title"].ToString(),
                    name=reader["name"].ToString(),
                });
            }
            return l;
        }
    }
}