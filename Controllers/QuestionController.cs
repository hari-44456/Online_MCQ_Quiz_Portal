using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using WCE.Models;

namespace WCE.Controllers  
{  
    public class QuestionController : Controller  
    {
        [HttpGet]
        public IActionResult Create(int id){
            QuestionStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.QuestionStoreContext)) as QuestionStoreContext;
            TestStoreContext context2 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;
            
            int teacher_id = Int32.Parse(HttpContext.Request.Query["teacher_id"]);

            dynamic mymodel = new ExpandoObject();
            mymodel.Test = context2.GetTestDetails(id);
            mymodel.Question= context1.GetAllQuestions(id);

            ViewData["Test_Id"]=id;
            ViewData["teacher_id"]=teacher_id;
            return View(mymodel);
        }

        [HttpPost]
        public void Create(string type,int test_id, int question_id, int teacher_id, int question_index){
            QuestionStoreContext context1 = HttpContext.RequestServices.GetService(typeof(WCE.Models.QuestionStoreContext)) as QuestionStoreContext;
            TestStoreContext context2 = HttpContext.RequestServices.GetService(typeof(WCE.Models.TestStoreContext)) as TestStoreContext;

            if(type=="delete"){
                context1.DeleteQuestion(test_id,question_id,question_index);

                dynamic mymodel = new ExpandoObject();
                mymodel.Test = context2.GetTestDetails(test_id);
                mymodel.Question= context1.GetAllQuestions(test_id);

                Response.Redirect($"/Question/Create/{test_id}?teacher_id={teacher_id}");
            }else{
                if(type=="edit"){
                    Response.Redirect($"/Question/Edit/{teacher_id}?test_id={test_id}&question_id={question_id}");
                }else{
                    context2.SetTotalMarks(test_id);
                    context2.SetTestActive(test_id);
                    Response.Redirect($"/Teacher/Profile/{teacher_id}");
                }
            }
        }

        [HttpGet]
        public IActionResult Edit(int id){
            QuestionStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.QuestionStoreContext)) as QuestionStoreContext;

            int test_id = Int32.Parse(HttpContext.Request.Query["test_id"]);
            int question_id = Int32.Parse(HttpContext.Request.Query["question_id"]);

            ViewData["Test_Id"]=test_id;
            Question q=context.GetQuestionDetails(test_id,question_id);
            ViewData["question"]=(string)q.que;
            ViewData["option1"]=(string)q.option1;
            ViewData["option2"]=(string)q.option2;
            ViewData["option3"]=(string)q.option3;
            ViewData["option4"]=(string)q.option4;
            ViewData["ans"]=(string)q.ans;
            ViewData["points"]=(int)q.points;
            ViewData["question_id"]=(int)q.question_id;
            ViewData["teacher_id"]=(int)id;
            
            return View();
        }

        [HttpPost]
        public void Edit(int test_id, int question_id, string que, string option1, string option2, string option3, string option4, string ans, int points, IFormFile image, int teacher_id){
            
            QuestionStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.QuestionStoreContext)) as QuestionStoreContext;

            string fileName = Path.GetFileName(image.FileName);

            Console.WriteLine(fileName);
            
            context.EditQuestion(test_id,question_id,que,option1,option2,option3,option4,ans,points);

            Response.Redirect($"/Question/Create/{test_id}?teacher_id={teacher_id}");
        }

        [HttpGet]
        public IActionResult AddQuestion(int id){
            ViewData["Test_Id"]=id;
            return View();
        }

        [HttpPost]
        public void AddQuestion(int id, string que, string option1, string option2, string option3, string option4, string ans, int points, IFormFile image){
            QuestionStoreContext context = HttpContext.RequestServices.GetService(typeof(WCE.Models.QuestionStoreContext)) as QuestionStoreContext;
            int teacher_id = Int32.Parse(HttpContext.Request.Query["teacher_id"]);

            string fileName = Path.GetFileName(image.FileName);

            Console.WriteLine(fileName);

            context.AddQuestion(id,que,option1,option2,option3,option4,ans,points);

            Response.Redirect($"/Question/Create/{id}?teacher_id={teacher_id}");
        }
    }    
}