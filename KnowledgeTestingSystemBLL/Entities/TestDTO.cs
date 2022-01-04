using System;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfQuestions { get; set; }
        public DateTime TimeToEnd { get; set; }
    }
}
