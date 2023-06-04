using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Knowledgebase;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Knowledgebase
{
    [Validator(typeof(KnowledgebaseArticleValidator))]
    public class KnowledgebaseArticleModel : BaseForeverNoteEntityModel
    {
        public KnowledgebaseArticleModel()
        {
            RelatedArticles = new List<KnowledgebaseArticleModel>();
            CategoryBreadcrumb = new List<KnowledgebaseCategoryModel>();
            Comments = new List<KnowledgebaseArticleCommentModel>();
            AddNewComment = new AddKnowledgebaseArticleCommentModel();
        }

        public string Name { get; set; }

        public string Content { get; set; }

        public string ParentCategoryId { get; set; }

        public string SeName { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public bool AllowComments { get; set; }

        public IList<KnowledgebaseArticleModel> RelatedArticles { get; set; }

        public List<KnowledgebaseCategoryModel> CategoryBreadcrumb { get; set; }

        public AddKnowledgebaseArticleCommentModel AddNewComment { get; set; }

        public IList<KnowledgebaseArticleCommentModel> Comments { get; set; }
    }
}
