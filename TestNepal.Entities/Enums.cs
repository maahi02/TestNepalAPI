using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestNepal.Entities
{
    public enum ApplicationType
    {
        JavaScript = 0,
        NativeConfidential = 1
    };

    public class Enums
    {
        /// <summary>
        /// Visibility for different
        /// kinds of Users:
        /// BeforeLogin,AfterLogin,AfterSubscription
        /// </summary>
        enum ItemVisibility
        {
            BeforeLogin = 1,
            AfterLogin = 2,
            AfterSubscription = 3
        };

        public enum ModulesEnum
        {
            ModelSets = 1,
            CurrentAffairs = 2,
            Exam = 3,
            QuestionModule = 4,
            QuizModule = 5,
            OnlineExam = 6
        };

        public enum TestNepalTypeEnum
        {
            ExamType = 1,
            QuestionType = 2,
            QuizStatusType = 3,
            ModelSetStatusType = 4,
            OnlineExamStatusType = 5,
        };

    }

}