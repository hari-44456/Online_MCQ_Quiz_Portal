
namespace WCE.Models  
{  
    public class Test
    {  
        private TestStoreContext context;

        public int test_id { get; set; }
  
        public int teacher_id { get;set; }

        public string test_title { get;set; }

        public int total_points { get; set; }

        public float duration { get; set; }

        public string test_password { get; set; }

        public string isActive { get; set; }
    }  
}