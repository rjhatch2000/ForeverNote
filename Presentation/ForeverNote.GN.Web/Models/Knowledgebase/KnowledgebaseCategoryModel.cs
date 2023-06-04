using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Knowledgebase
{
    public class KnowledgebaseCategoryModel : BaseForeverNoteEntityModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsCurrent { get; set; }

        public List<KnowledgebaseCategoryModel> Children { get; set; }

        public KnowledgebaseCategoryModel Parent { get; set; }

        public string SeName { get; set; }
    }
}
