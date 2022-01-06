using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Option : BaseEntity
    {
        public string Text { get; set; }
        public int QuestionId { get; set; }

        //NAVIGATION PROPERTIES
        public Question Question { get; set; }
    }
}
