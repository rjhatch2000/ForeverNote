using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Notes;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Queries.Models.Catalog
{
    public class GetSearchNotesQuery : IRequest<(IPagedList<Note> notes, IList<string> filterableSpecificationAttributeOptionIds)>
    {
        public User User { get; set; }

        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = int.MaxValue;
        public IList<string> NotebookIds { get; set; } = null;
        public bool MarkedAsNewOnly { get; set; } = false;
        public bool? FeaturedNotes { get; set; } = null;
        public string NoteTag { get; set; } = "";
        public string Keywords { get; set; } = null;
        public bool SearchDescriptions { get; set; } = false;
        public bool SearchNoteTags { get; set; } = false;
        public string LanguageId { get; set; } = "";
        public NoteSortingEnum OrderBy { get; set; } = NoteSortingEnum.Position;
    }
}
