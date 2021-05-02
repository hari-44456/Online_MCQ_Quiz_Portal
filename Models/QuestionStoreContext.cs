
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web;

namespace WCE.Models {
    public class QuestionStoreContext {
        public string ConnectionString {get;set;}
        public QuestionStoreContext(string connectionString) {
            this.ConnectionString=connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(ConnectionString);
        }

        public List<Question> AddQuestion(int test_id, string que, string option1, string option2, string option3, string option4, string ans, int points ){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd1=new MySqlCommand("select max(question_index) as question_index from question where test_id=@par",conn);

            cmd1.Parameters.AddWithValue("@par",test_id);
            cmd1.Prepare();

            MySqlDataReader reader = cmd1.ExecuteReader();

            int question_index=0;
            while(reader.Read()) {
                if(reader["question_index"]!=DBNull.Value)
                question_index=(int)reader["question_index"];
            }
            reader.Close();

            question_index=question_index+1;

            MySqlCommand cmd=new MySqlCommand("insert into question (test_id, que, option1, option2, option3, option4, ans, points, question_index) values (@par1, @par2, @par3, @par4, @par5, @par6, @par7, @par8, @par9)",conn);

            cmd.Parameters.AddWithValue("@par1",test_id);
            cmd.Parameters.AddWithValue("@par2",que);
            cmd.Parameters.AddWithValue("@par3",option1);
            cmd.Parameters.AddWithValue("@par4",option2);
            cmd.Parameters.AddWithValue("@par5",option3);
            cmd.Parameters.AddWithValue("@par6",option4);
            cmd.Parameters.AddWithValue("@par7",ans);
            cmd.Parameters.AddWithValue("@par8",points);
            cmd.Parameters.AddWithValue("@par9",question_index);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            return GetAllQuestions(test_id);
        }

        public void EditQuestion(int test_id, int question_id, string que, string option1, string option2, string option3, string option4, string ans, int points){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("update question set que=@par2, option1=@par3, option2=@par4, option3=@par5, option4=@par6, ans=@par7, points=@par8 where question_id=@par9 and test_id=@par1",conn);

            cmd.Parameters.AddWithValue("@par1",test_id);
            cmd.Parameters.AddWithValue("@par2",que);
            cmd.Parameters.AddWithValue("@par3",option1);
            cmd.Parameters.AddWithValue("@par4",option2);
            cmd.Parameters.AddWithValue("@par5",option3);
            cmd.Parameters.AddWithValue("@par6",option4);
            cmd.Parameters.AddWithValue("@par7",ans);
            cmd.Parameters.AddWithValue("@par8",points);
            cmd.Parameters.AddWithValue("@par9",question_id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public List<Question> GetAllQuestions(int test_id){
            List<Question> l = new List<Question>();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from question where test_id=@par",conn);

            cmd.Parameters.AddWithValue("@par",test_id);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read()) {
                l.Add(new Question(){
                    question_id=(int)reader["question_id"],
                    que=(string)reader["que"],
                    option1=(string)reader["option1"],
                    option2=(string)reader["option2"],
                    option3=(string)reader["option3"],
                    option4=(string)reader["option4"],
                    ans=(string)reader["ans"],
                    points=(int)reader["points"],
                    question_index=(int)reader["question_index"],
                });
            }

            return l;
        }

        public void DeleteQuestion(int test_id, int question_id, int question_index){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("delete from question where test_id=@var1 and question_id=@var2",conn);

            cmd.Parameters.AddWithValue("@var1",test_id);
            cmd.Parameters.AddWithValue("@var2",question_id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();

            MySqlCommand cmd1=new MySqlCommand("select max(question_index) as question_index from question where test_id=@par",conn);

            cmd1.Parameters.AddWithValue("@par",test_id);
            cmd1.Prepare();

            MySqlDataReader reader = cmd1.ExecuteReader();

            int max_index=0;
            while(reader.Read()) {
                if(reader["question_index"]!=DBNull.Value)
                max_index=(int)reader["question_index"];
            }
            reader.Close();

            while(question_index<max_index){
                MySqlCommand cmd2=new MySqlCommand("update question set question_index=@par1 where test_id=@par2 and question_index=@par3",conn);

                cmd2.Parameters.AddWithValue("@par1",question_index);
                cmd2.Parameters.AddWithValue("@par2",test_id);
                cmd2.Parameters.AddWithValue("@par3",question_index+1);
                cmd2.Prepare();

                cmd2.ExecuteNonQuery();
                question_index=question_index+1;
            }
        }

        public Question GetQuestionDetails(int test_id, int question_id){
            Question q = new Question();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from question where test_id=@par1 and question_id=@par2",conn);

            cmd.Parameters.AddWithValue("@par1",test_id);
            cmd.Parameters.AddWithValue("@par2",question_id);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read()) {
                q.question_id=(int)reader["question_id"];
                q.que=(string)reader["que"];
                q.option1=(string)reader["option1"];
                q.option2=(string)reader["option2"];
                q.option3=(string)reader["option3"];
                q.option4=(string)reader["option4"];
                q.ans=(string)reader["ans"];
                q.points=(int)reader["points"];
                q.question_index=(int)reader["question_index"];
            }

            return q;
        }

        public Question GetQuestion(int test_id, int question_index){
            Question q = new Question();

            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select * from question where test_id=@par1 and question_index=@par2",conn);

            cmd.Parameters.AddWithValue("@par1",test_id);
            cmd.Parameters.AddWithValue("@par2",question_index);
            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read()) {
                q.question_id=(int)reader["question_id"];
                q.que=(string)reader["que"];
                q.option1=(string)reader["option1"];
                q.option2=(string)reader["option2"];
                q.option3=(string)reader["option3"];
                q.option4=(string)reader["option4"];
                q.ans=(string)reader["ans"];
                q.points=(int)reader["points"];
                q.question_index=(int)reader["question_index"];
            }

            return q;
        }

        public int GetLastQuestionIndex(int test_id){
            MySqlConnection conn=GetConnection();
            conn.Open();

            MySqlCommand cmd=new MySqlCommand("select MAX(question_index) as last_index from question where test_id=@par1",conn);

            cmd.Parameters.AddWithValue("@par1",test_id);
            
            MySqlDataReader reader = cmd.ExecuteReader();

            int last_question_index=-1;
            while(reader.Read()){
                last_question_index=(int)reader["last_index"];
            }
            return last_question_index;
        }
    }
}