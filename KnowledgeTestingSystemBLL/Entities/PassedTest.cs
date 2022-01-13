using System.Collections.Generic;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class PassedTest
    {
        public int TestId { get; set; }
        public Dictionary<int, int> QuestionAnswerKeyValue { get; set; }
    }
}
