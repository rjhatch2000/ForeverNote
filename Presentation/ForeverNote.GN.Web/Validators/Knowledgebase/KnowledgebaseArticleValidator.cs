using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Knowledgebase;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Knowledgebase
{
    public class KnowledgebaseArticleValidator : BaseForeverNoteValidator<KnowledgebaseArticleModel>
    {
        public KnowledgebaseArticleValidator(
            IEnumerable<IValidatorConsumer<KnowledgebaseArticleModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.AddNewComment.CommentText).NotEmpty().WithMessage(localizationService.GetResource("ForeverNote.knowledgebase.addarticlecomment.result")).When(x => x.AddNewComment != null);
        }
    }
}