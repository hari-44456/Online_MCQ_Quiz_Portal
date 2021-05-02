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
    public class TeacherController : Controller  
    {  
        [HttpGet]
        public IActionResult Profile(int id) {
            TeacherStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TeacherStoreContext)) as TeacherStoreContext;
            dynamic mymodel = new ExpandoObject();
            mymodel.Teacher = context.getTeacherDetails(id);
            mymodel.Test= context.getTests(id);
            return View(mymodel);
        }

        [HttpPost]
        public IActionResult Profile(string type, int test_id, int teacher_id) {
            TeacherStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TeacherStoreContext)) as TeacherStoreContext;
            TestStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;

            if(type=="edit"){
                var id=test_id;
                Response.Redirect($"/Question/Create/{test_id}?teacher_id={teacher_id}");
            }else{
                if(type=="delete"){
                    var id=teacher_id;
                    context.DeleteTest(test_id);
                    Response.Redirect($"/Teacher/Profile/{teacher_id}");
                }else{
                    if(type=="attempts"){
                        Response.Redirect($"/Teacher/TestAttempts?teacher_id={teacher_id}&test_id={test_id}");
                    }
                    else{
                        context1.SetTestInactive(test_id);
                        Response.Redirect($"/Teacher/Profile/{teacher_id}");
                    }
                }
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.Teacher = context.getTeacherDetails(test_id);
            mymodel.Test= context.getTests(test_id);
            return View(mymodel);
        }

        [HttpGet]
        public IActionResult TestAttempts(){
            TestStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            TestAttemptsStoreContext context2 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestAttemptsStoreContext)) as TestAttemptsStoreContext;
            dynamic mymodel = new ExpandoObject();

            int teacher_id = Int32.Parse(HttpContext.Request.Query["teacher_id"]);
            int test_id = Int32.Parse(HttpContext.Request.Query["test_id"]);

            mymodel.Test = context1.GetTestDetails(test_id);
            mymodel.TestAttempts = context2.GetTestAttemptsForTeacher(test_id);

            return View(mymodel);
        }

        [HttpGet]
        public void Create(int teacher_id) {
            TeacherStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TeacherStoreContext)) as TeacherStoreContext;
            int id=(int)context.CreateTest(teacher_id);
            Response.Redirect($"/Test/Create/{id}?teacher_id={teacher_id}");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public void Login(string username,string password)
        {
            TeacherStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TeacherStoreContext)) as TeacherStoreContext;
            int id=(int)context.Login(username,password);
            if(id!=-1)
                Response.Redirect($"/Teacher/Profile/{id}");
            else
                ViewData["Message"]="Invalid Username or Password";
        }

        [HttpGet]
        public IActionResult Invalid()
        {
            return View();
        }

        [HttpPost]
        public void Invalid(string username,string password)
        {
            TeacherStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TeacherStoreContext)) as TeacherStoreContext;
            var id=context.Login(username,password);     
            Response.Redirect("/Teacher/"+id);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(string name,string username,string password)
        {
            TeacherStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TeacherStoreContext)) as TeacherStoreContext;
            context.AddTeacher(name,username,password);     
            Response.Redirect("/Teacher/Login");
        }
    }    
}