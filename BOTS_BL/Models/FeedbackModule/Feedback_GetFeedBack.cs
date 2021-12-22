using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.FeedbackModule
{
  public class Feedback_GetFeedBack
  {
        public string GroupId { get; set; }
        public string ImagePath { get; set; }
        public string HomeHeading1 { get; set; }
        public string HomeHeading2 { get; set; }
        public string HomeHeading3 { get; set; }
        public string HomeRepresentative { get; set; }
        public string QuestionsHeading1 { get; set; }
        public string QuestionsHeading2 { get; set; }      
        public string OtherInfoHeading1 { get; set; }
        public string OtherInfoHeading2 { get; set; }
        public string FeedbackQuestion1 { get; set; }
        public string FeedbackQuestion2 { get; set; }
        public string FeedbackQuestion3 { get; set; }
        public string FeedbackQuestion4 { get; set; }
        public string OtherInfoQuestion1 { get; set; }
        public string OtherInfoQuestion2 { get; set; }
        public string OtherInfoQuestion3 { get; set; }
        public string OtherInfoQuestion4 { get; set; }
        public bool IsOtherInfoEnabled { get; set; }
    }
}
