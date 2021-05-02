
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WCE.Models {
    public class StudentStoreContext {
        public string ConnectionString {get;set;}
        public StudentStoreContext(string connectionString) {
            this.ConnectionString=connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(ConnectionString);
        }

        public Student GetStudentDetails(int student_id) {
            Student s=new Student();
            MySqlConnection conn= GetConnection();
            conn.Open();
            string query = "select * from student where stuId=@par1";
            MySqlCommand cmd= new MySqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@par1",student_id);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()) {
                s.prn=(string)reader["prn"];
                s.name=(string)reader["name"];
                s.branch=(string)reader["branch"];
                s.year=(string)reader["year"];
            }
            return s;
        }

        public string Login(string username, string password){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from student where username=@par1 and password=@par2",conn);

            cmd.Parameters.AddWithValue("@par1",username);
            cmd.Parameters.AddWithValue("@par2",password);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();
            string name="Invalid";
            while(reader.Read())
                name=reader["stuId"].ToString();
            return name;
        }

        public void AddStudent(string prn,string name,string year,string branch,string username,string password) {
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("insert into student(prn,name,year,branch,username,password) values(@p1,@p2,@p3,@p4,@p5,@p6)",conn);

            cmd.Parameters.AddWithValue("@p1",prn);
            cmd.Parameters.AddWithValue("@p2",name);
            cmd.Parameters.AddWithValue("@p3",year);
            cmd.Parameters.AddWithValue("@p4",branch);
            cmd.Parameters.AddWithValue("@p5",username);
            cmd.Parameters.AddWithValue("@p6",password);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public string GetStudentName(int student_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select name from student where stuId=@par",conn);

            cmd.Parameters.AddWithValue("@par",student_id);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            string student_name="";
            while(reader.Read()) {
                student_name=(string)reader["name"];
            }

            return student_name;
        }
    }
}