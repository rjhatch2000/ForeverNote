using FluentValidation;

namespace ForeverNote.Web.Framework.Validators
{
    public interface IValidatorConsumer<T> where T : class
    {
        void AddRules(BaseForeverNoteValidator<T> validator);
    }
}
