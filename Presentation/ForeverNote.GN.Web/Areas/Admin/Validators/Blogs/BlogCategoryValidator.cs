using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Blogs;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Blogs
{
    public class BlogCategoryValidator : BaseForeverNoteValidator<BlogCategoryModel>
    {
        public BlogCategoryValidator(IEnumerable<IValidatorConsumer<BlogCategoryModel>> validators, 
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogCategory.Fields.Name.Required"));
        }
    }
}