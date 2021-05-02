namespace WCE.Models  
{  
    public class TestAttempts
    {
        private TestAttemptsStoreContext context;  
  
        public int student_id { get; set; }

        public int test_id { get;set; }

        public int score { get; set; }

        public int total_score { get; set; }

        public string test_title { get; set; }
        
        public string name { get; set; }
    }  
}