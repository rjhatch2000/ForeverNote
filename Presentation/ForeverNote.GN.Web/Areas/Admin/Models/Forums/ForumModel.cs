using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Forums;
using System;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Forums
{
    [Validator(typeof(ForumValidator))]
    public partial class ForumModel : BaseForeverNoteEntityModel
    {
        public ForumModel()
        {
            ForumGroups = new List<ForumGroupModel>();
        }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.ForumGroupId")]
        public string ForumGroupId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.Description")]
        
        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Forums.Forum.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        public List<ForumGroupModel> ForumGroups { get; set; }
    }
}