using System.Collections.Generic;

namespace ForeverNote.Services.Logging
{
    public partial interface IActivityKeywordsProvider
    {
        IList<string> GetNotebookSystemKeywords();
        IList<string> GetNoteSystemKeywords();
        IList<string> GetManufacturerSystemKeywords();
        IList<string> GetKnowledgebaseNotebookSystemKeywords();
        IList<string> GetKnowledgebaseArticleSystemKeywords();
    }
}
