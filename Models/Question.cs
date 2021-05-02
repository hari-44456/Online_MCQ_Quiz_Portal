namespace WCE.Models  
{  
    public class Question
    {
        private QuestionStoreContext context;  
  
        public int question_id { get; set; }

        public int test_id { get;set; }

        public string que { get; set; }

        public string option1 { get; set; }

        public string option2 { get; set; }

        public string option3 { get; set; }

        public string option4 { get; set; }

        public string ans { get; set; }

        public int points { get; set; }

        public int question_index { get; set; }
    }  
}