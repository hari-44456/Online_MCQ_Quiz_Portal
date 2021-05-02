namespace WCE.Models  
{  
    public class Answer
    {
        private QuestionStoreContext context;  
  
        public int student_id { get; set; }

        public int test_id { get;set; }

        public int question_id { get; set; }

        public string selected { get; set; }

        public int points { get; set; }

    }  
}