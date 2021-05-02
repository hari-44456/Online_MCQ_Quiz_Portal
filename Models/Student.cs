
namespace WCE.Models  
{  
    public class Student
    {  
        private StudentStoreContext context;

        public int stuId { get; set; }
  
        public string prn { get; set; }

        public string name { get;set; }

        public string year { get;set; }

        public string branch { get;set; }

        public string username { get;set; }

        public string password { get; set; }        
    }  
}