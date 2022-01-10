﻿using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; set; }
        public int TestId { get; set; }
        public int NumberOfOptions { get; set; }

        //NAVIGATION PROPERTIES
        public Test Test { get; set; }
        public ICollection<Option> Options { get; set; }
    }
}
