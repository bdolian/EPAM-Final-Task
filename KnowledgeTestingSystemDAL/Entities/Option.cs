using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Option : BaseEntity
    {
        public string Text { get; set; }
        public ICollection<QuestionOption> Questions { get; set; }
    }
}
