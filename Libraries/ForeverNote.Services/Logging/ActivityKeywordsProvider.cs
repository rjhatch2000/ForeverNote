using System.Collections.Generic;

namespace ForeverNote.Services.Logging
{
    public class ActivityKeywordsProvider: IActivityKeywordsProvider
    {
        public virtual IList<string> GetNotebookSystemKeywords()
        {
            var tokens = new List<string>
            {
                "PublicStore.ViewNotebook",
                "EditNotebook",
                "AddNewNotebook",
            };
            return tokens;
        }
        public virtual IList<string> GetNoteSystemKeywords()
        {
            var tokens = new List<string>
            {
                "PublicStore.ViewNote",
                "EditNote",
                "AddNewNote",
            };
            return tokens;
        }
        public virtual IList<string> GetManufacturerSystemKeywords()
        {
            var tokens = new List<string>
            {
                "PublicStore.ViewManufacturer",
                "EditManufacturer",
                "AddNewManufacturer"
            };
            return tokens;
        }

        public IList<string> GetKnowledgebaseNotebookSystemKeywords()
        {
            var tokens = new List<string>
            {
                "CreateKnowledgebaseNotebook",
                "UpdateKnowledgebaseNotebook",
                "DeleteKnowledgebaseNotebook"
            };
            return tokens;
        }


        public IList<string> GetKnowledgebaseArticleSystemKeywords()
        {
            var tokens = new List<string>
            {
                "CreateKnowledgebaseArticle",
                "UpdateKnowledgebaseArticle",
                "DeleteKnowledgebaseArticle",
            };
            return tokens;
        }
    }
}
