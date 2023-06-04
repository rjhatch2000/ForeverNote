using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Common;

namespace ForeverNote.Web.Areas.Admin.Models.Common
{
    [Validator(typeof(ReviewValidator))]
    public partial class ReviewModel : BaseForeverNoteEntityModel
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.Reviews.Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the review text
        /// </summary>
        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.Reviews.Review")]
        public string ReviewText { get; set; }
    }
}