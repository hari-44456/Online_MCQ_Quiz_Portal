namespace WCE.Models  
{  
    public class Login
    {  
        private LoginStoreContext context;  
  
        public string username { get; set; }

        public string password { get;set; }
    }  
}