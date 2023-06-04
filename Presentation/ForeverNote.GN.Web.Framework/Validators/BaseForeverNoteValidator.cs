using FluentValidation;
using System.Collections.Generic;

namespace ForeverNote.Web.Framework.Validators
{
    public abstract class BaseForeverNoteValidator<T> : AbstractValidator<T> where T : class
    {

        protected BaseForeverNoteValidator(IEnumerable<IValidatorConsumer<T>> validators)
        {
            PostInitialize(validators);
        }

        protected virtual void PostInitialize(IEnumerable<IValidatorConsumer<T>> validators)
        {
            foreach (var item in validators)
            {
                item.AddRules(this);
            }

        }

    }


}