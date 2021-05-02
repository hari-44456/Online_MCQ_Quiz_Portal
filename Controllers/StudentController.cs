
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using WCE.Models;

namespace WCE.Controllers  
{  
    public class StudentController : Controller  
    {  
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public void Login(string username,string password)
        {
            StudentStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.StudentStoreContext)) as StudentStoreContext;
            var id=context.Login(username,password);
            Response.Redirect($"/Student/Profile/{id}");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(string prn,string name,string year,string branch,string username,string password)
        {
            StudentStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.StudentStoreContext)) as StudentStoreContext;
            context.AddStudent(prn,name,year,branch,username,password);     
            Response.Redirect("/Student/Login");
        }

        [HttpGet]
        public IActionResult Invalid()
        {
            return View();
        }

        [HttpPost]
        public void Invalid(string username,string password)
        {
            StudentStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.StudentStoreContext)) as StudentStoreContext;
            var id=context.Login(username,password);
            Response.Redirect("/Student/"+id);
        }

        [HttpGet]
        public IActionResult Profile(int id)
        {
            StudentStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.StudentStoreContext)) as StudentStoreContext;
            TestAttemptsStoreContext context2 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestAttemptsStoreContext)) as TestAttemptsStoreContext;

            ViewData["student_id"]=id;
            dynamic mymodel = new ExpandoObject();
            mymodel.Student = context1.GetStudentDetails(id);
            mymodel.TestAttempts = context2.GetTestAttemptsForStudent(id);
            return View(mymodel);
        }

        [HttpPost]
        public void Profile(int id,string name)
        {
            Response.Redirect($"/Student/EnterTest/{id}");
        }

        [HttpGet]
        public IActionResult EnterTest(int id)
        {
            TestStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            dynamic mymodel = new ExpandoObject();
            mymodel.Test = context.GetAllTests();
            ViewData["id"]=id;
            return View(mymodel);
        }

        [HttpPost]
        public IActionResult EnterTest(string test_title, string test_password, int student_id){
            TestStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            int test_id=context.GetTestDetails(test_title,test_password);
            if(test_id==-1){
                dynamic mymodel = new ExpandoObject();
                mymodel.Test = context.GetAllTests();
                ViewData["id"]=student_id;
                ViewData["error"]="Wrong Test Credentails";
                return View(mymodel);
            }else{
                Response.Redirect($"/Student/AttemptTest?test_id={test_id}&student_id={student_id}&question_index=1");
                return new EmptyResult();
            }
        }

        [HttpGet]
        public IActionResult AttemptTest(){
            TestStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            QuestionStoreContext context2 = HttpContext.RequestServices.GetService(typeof(WCE.Models.QuestionStoreContext)) as QuestionStoreContext;

            int test_id = Int32.Parse(HttpContext.Request.Query["test_id"]);
            int student_id = Int32.Parse(HttpContext.Request.Query["student_id"]);
            int question_index = Int32.Parse(HttpContext.Request.Query["question_index"]);
            int last_question_index = context2.GetLastQuestionIndex(test_id);

            dynamic mymodel = new ExpandoObject();
            mymodel.Test=context1.GetTestDetails(test_id);
            mymodel.Question=context2.GetQuestion(test_id,question_index);

            ViewData["student_id"]=student_id;
            ViewData["test_id"]=test_id;
            ViewData["last_question_index"]=last_question_index;

            return View(mymodel);
        }

        [HttpPost]
        public void AttemptTest(string radio, int question_id, int test_id, int student_id, string ans, int points, int question_index, int last_question_index){
            AnswerStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.AnswerStoreContext)) as AnswerStoreContext;

            int next_question_index=context1.SubmitAnswer(radio,question_id,test_id,student_id,ans,points,question_index);

            if(question_index!=last_question_index){
                Response.Redirect($"/Student/AttemptTest?test_id={test_id}&student_id={student_id}&question_index={next_question_index}");
            }else{
                Response.Redirect($"/Student/EndTest?test_id={test_id}&student_id={student_id}");
            }
        }

        [HttpGet]
        public IActionResult EndTest(){
            TestStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            int test_id = Int32.Parse(HttpContext.Request.Query["test_id"]);
            int student_id = Int32.Parse(HttpContext.Request.Query["student_id"]);

            dynamic mymodel = new ExpandoObject();
            mymodel.Test=context1.GetTestDetails(test_id);

            ViewData["student_id"]=student_id;
            ViewData["test_id"]=test_id;

            return View(mymodel);
        }

        [HttpPost]
        public void EndTest(int student_id,int test_id){
            TestAttemptsStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestAttemptsStoreContext)) as TestAttemptsStoreContext;
            TestStoreContext context2 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            StudentStoreContext context3 = HttpContext.RequestServices.GetService(typeof(WCE.Models.StudentStoreContext)) as StudentStoreContext;

            string test_title=context2.GetTestTitle(test_id);
            int total=context2.GetTotalMarks(test_id);
            string student_name=context3.GetStudentName(student_id);
            context1.EnterAttempt(student_id,test_id,total,test_title,student_name);

            Response.Redirect($"/Student/Profile/{student_id}");
        }
    }    
}