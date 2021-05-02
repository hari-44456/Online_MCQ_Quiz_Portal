using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WCE.Models {
    public class AnswerStoreContext {
        public string ConnectionString {get;set;}
        public AnswerStoreContext(string connectionString) {
            this.ConnectionString=connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(ConnectionString);
        }

        public int SubmitAnswer(string answer, int question_id, int test_id, int student_id, string ans, int points, int question_index){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd1=new MySqlCommand("select COUNT(*) as cnt from answer where question_id=@par1 and test_id=@par2 and student_id=@par3",conn);

            cmd1.Parameters.AddWithValue("@par1",question_id);
            cmd1.Parameters.AddWithValue("@par2",test_id);
            cmd1.Parameters.AddWithValue("@par3",student_id);
            cmd1.Prepare();
            MySqlDataReader reader = cmd1.ExecuteReader();

            int present=0;
            while(reader.Read()) {
                if(Convert.ToInt32(reader["cnt"])==1){
                    present=1;
                }
            }

            reader.Close();

            if(answer!=ans)    points=0;

            if(present==1){
                MySqlCommand cmd2=new MySqlCommand("update answer set selected=@par4, points=@par5 where test_id=@par6 and student_id=@par7 and question_id=@par8",conn);

                cmd2.Parameters.AddWithValue("@par4",answer);
                cmd2.Parameters.AddWithValue("@par5",points);
                cmd2.Parameters.AddWithValue("@par6",test_id);
                cmd2.Parameters.AddWithValue("@par7",student_id);
                cmd2.Parameters.AddWithValue("@par8",question_id);

                cmd2.Prepare();

                cmd2.ExecuteNonQuery();
            }else{
                MySqlCommand cmd2=new MySqlCommand("insert into answer values (@par4,@par5,@par6,@par7,@par8)",conn);

                cmd2.Parameters.AddWithValue("@par4",student_id);
                cmd2.Parameters.AddWithValue("@par5",test_id);
                cmd2.Parameters.AddWithValue("@par6",question_id);
                cmd2.Parameters.AddWithValue("@par7",answer);
                cmd2.Parameters.AddWithValue("@par8",points);
                cmd2.Prepare();

                cmd2.ExecuteNonQuery();
            }

            return question_index+1;
        }
    }
}