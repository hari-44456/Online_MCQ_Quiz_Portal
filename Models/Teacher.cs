
namespace WCE.Models  
{  
    public class Teacher
    {  
        private TeacherStoreContext context;

        public int teacher_id { get; set; }
  
        public string name { get;set; }

        public string username { get;set; }

        public string password { get; set; }        
    }  
}