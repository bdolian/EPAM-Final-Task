using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; set; }
        public ICollection<TestQuestion> Tests { get; set; }
        public ICollection<QuestionOption> Options { get; set; }
    }
}
