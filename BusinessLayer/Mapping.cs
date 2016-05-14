using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTO;
using DAL.Entity;


namespace BusinessLayer
{
    class Mapping
    {
        public static IMapper Mapper { get; }
        static Mapping()
        {
            var config = new MapperConfiguration(c =>
            {                
                c.CreateMap<AnswerDTO, Answer>().ReverseMap();
                c.CreateMap<TeacherDTO, Teacher>().ReverseMap();
                c.CreateMap<AreaDTO, Area>().ReverseMap();
                c.CreateMap<QuestionDTO, Question>().ReverseMap();
                c.CreateMap<SolutionDTO, Solution>().ReverseMap();
                c.CreateMap<SolutionAnswerDTO, SolutionAnswer>().ReverseMap();
                c.CreateMap<StudentDTO, Student>().ReverseMap();
                c.CreateMap<StudentGroupDTO, StudentGroup>().ReverseMap();
                c.CreateMap<TestPatternDTO, TestPattern>().ReverseMap();
            });
            Mapper = config.CreateMapper();
        }
    }
}
