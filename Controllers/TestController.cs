using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WCE.Models;

namespace WCE.Controllers  
{  
    public class TestController : Controller  
    {  
        [HttpGet]
        public IActionResult Create(int id) {
            TestStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            int teacher_id = Int32.Parse(HttpContext.Request.Query["teacher_id"]);
            
            ViewData["Message"] = "Test";
            ViewData["test_id"] = id;
            ViewData["teacher_id"] = teacher_id;

            dynamic mymodel = new ExpandoObject();  
            return View(mymodel);
        }

        [HttpPost]
        public void Create(int id, string test_title, string test_password, string duration, int teacher_id) {
            TestStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            int test_id = context.SaveTestDetails(id,test_title,test_password,duration);
            if(id!=-1)
                Response.Redirect($"/Question/Create/{id}?teacher_id={teacher_id}");
        }

        public Test GetTestDetails(int test_id){
            TestStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            Test t = context.GetTestDetails(test_id);
            return t;
        }
    }    
}