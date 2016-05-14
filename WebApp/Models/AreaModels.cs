using BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class AreaModel
    {
        public string Name { get; set; }
        public List<QuestionDTO> Questions { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
    }
}