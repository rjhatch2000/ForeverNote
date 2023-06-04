using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Knowledgebase;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Knowledgebase;
using System.Collections.Generic;
using System.Linq;

namespace ForeverNote.Web.Areas.Admin.Validators.Knowledgebase
{
    public class KnowledgebaseCategoryModelValidator : BaseForeverNoteValidator<KnowledgebaseCategoryModel>
    {
        public KnowledgebaseCategoryModelValidator(
            IEnumerable<IValidatorConsumer<KnowledgebaseCategoryModel>> validators,
            ILocalizationService localizationService, IKnowledgebaseService knowledgebaseService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.Name.Required"));
            RuleFor(x => x.ParentCategoryId).MustAsync(async (x, y, context) =>
            {
                var category = await knowledgebaseService.GetKnowledgebaseCategory(x.ParentCategoryId);
                if (category != null || string.IsNullOrEmpty(x.ParentCategoryId))
                {
                    return true;
                }
                else
                {
                    var categories = await knowledgebaseService.GetKnowledgebaseCategories();
                    if (!categories.Any())
                    {
                        return true;
                    }
                }

                return false;
            }).WithMessage(localizationService.GetResource("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.ParentCategoryId.MustExist"));
        }
    }
}
