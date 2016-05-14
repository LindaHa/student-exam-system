using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.DTO;
using BusinessLayer.Facades;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            AreaDTO area = new AreaDTO();
            area.Name = "java";
            AreaFacade areaFacade = new AreaFacade();
            areaFacade.CreateArea(area);

            var question = new QuestionDTO();
            question.Text = "is it true?";
            question.PointsForCorrectAnswer = 2;
            question.TypeOfQuestion = "single";
            question.Area = area;
            QuestionFacade questionFacade = new QuestionFacade();
            questionFacade.CreateQuestion(question);

            AnswerDTO answer = new AnswerDTO();
            answer.Text = "programovani je cool";
            answer.IsCorrect = true;
            answer.Question = question;
            AnswerFacade Answerfacade = new AnswerFacade();
            Answerfacade.CreateAnswer(answer);


            question.Answers.Add(answer);
            area.Questions.Add(question);
            answer.Question = question;


            Console.ReadKey();
        }
    }
}
