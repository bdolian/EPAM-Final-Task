using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Option : BaseEntity
    {
        public string Text { get; set; }

        //NAVIGATION PROPERTIES
        public ICollection<QuestionOption> Questions { get; set; }
    }
}
