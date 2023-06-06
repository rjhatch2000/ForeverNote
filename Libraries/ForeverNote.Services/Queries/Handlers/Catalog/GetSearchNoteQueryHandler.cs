using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Queries.Models.Catalog;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Queries.Handlers.Catalog
{
    public class GetSearchNotesQueryHandler : IRequestHandler<GetSearchNotesQuery, (IPagedList<Note> notes, IList<string> filterableSpecificationAttributeOptionIds)>
    {

        private readonly IRepository<Note> _noteRepository;

        private readonly CommonSettings _commonSettings;

        public GetSearchNotesQueryHandler(
            IRepository<Note> noteRepository,
            CommonSettings commonSettings
        )
        {
            _noteRepository = noteRepository;

            _commonSettings = commonSettings;
        }

        public async Task<(IPagedList<Note> notes, IList<string> filterableSpecificationAttributeOptionIds)>
            Handle(GetSearchNotesQuery request, CancellationToken cancellationToken)
        {
            var filterableSpecificationAttributeOptionIds = new List<string>();

            //validate "notebookIds" parameter
            if (request.NotebookIds != null && request.NotebookIds.Contains(""))
                _ = request.NotebookIds.Remove("");

            #region Search notes

            //notes
            var builder = Builders<Note>.Filter;
            var filter = FilterDefinition<Note>.Empty;

            //notebook filtering
            if (request.NotebookIds != null && request.NotebookIds.Any())
            {

                if (request.FeaturedNotes.HasValue)
                {
                    filter &= builder.Where(x => x.NoteNotebooks.Any(y => request.NotebookIds.Contains(y.NotebookId)
                        && y.IsFeaturedNote == request.FeaturedNotes));
                }
                else
                {
                    filter &= builder.Where(x => x.NoteNotebooks.Any(y => request.NotebookIds.Contains(y.NotebookId)));
                }
            }

            //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
            //That's why we pass the date value
            var nowUtc = DateTime.UtcNow;
            if (request.MarkedAsNewOnly)
            {
                filter &= builder.Where(p => p.MarkAsNew);
                filter &= builder.Where(p =>
                    (!p.MarkAsNewStartDateTimeUtc.HasValue || p.MarkAsNewStartDateTimeUtc.Value < nowUtc) &&
                    (!p.MarkAsNewEndDateTimeUtc.HasValue || p.MarkAsNewEndDateTimeUtc.Value > nowUtc));
            }

            //searching by keyword
            if (!string.IsNullOrWhiteSpace(request.Keywords))
            {
                if (_commonSettings.UseFullTextSearch)
                {
                    request.Keywords = "\"" + request.Keywords + "\"";
                    request.Keywords = request.Keywords.Replace("+", "\" \"");
                    request.Keywords = request.Keywords.Replace(" ", "\" \"");
                    filter &= builder.Text(request.Keywords);
                }
                else
                {
                    if (!request.SearchDescriptions)
                        filter &= builder.Where(p =>
                            p.Name.ToLower().Contains(request.Keywords.ToLower())
                            ||
                            p.Locales.Any(x => x.LocaleKey == "Name" && x.LocaleValue != null && x.LocaleValue.ToLower().Contains(request.Keywords.ToLower()))
                            );
                    else
                    {
                        filter &= builder.Where(p =>
                                (p.Name != null && p.Name.ToLower().Contains(request.Keywords.ToLower()))
                                ||
                                (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(request.Keywords.ToLower()))
                                ||
                                (p.FullDescription != null && p.FullDescription.ToLower().Contains(request.Keywords.ToLower()))
                                ||
                                (p.Locales.Any(x => x.LocaleValue != null && x.LocaleValue.ToLower().Contains(request.Keywords.ToLower())))
                                );
                    }
                }
            }

            //tag filtering
            if (!string.IsNullOrEmpty(request.NoteTag))
            {
                filter &= builder.Where(x => x.NoteTags.Any(y => y == request.NoteTag));
            }

            var builderSort = Builders<Note>.Sort.Descending(x => x.CreatedOnUtc);

            if (request.OrderBy == NoteSortingEnum.Position && request.NotebookIds != null && request.NotebookIds.Any())
            {
                //notebook position
                builderSort = Builders<Note>.Sort.Ascending(x => x.DisplayOrderNotebook);
            }
            else if (request.OrderBy == NoteSortingEnum.Position)
            {
                //otherwise sort by name
                builderSort = Builders<Note>.Sort.Ascending(x => x.Name);
            }
            else if (request.OrderBy == NoteSortingEnum.NameAsc)
            {
                //Name: A to Z
                builderSort = Builders<Note>.Sort.Ascending(x => x.Name);
            }
            else if (request.OrderBy == NoteSortingEnum.NameDesc)
            {
                //Name: Z to A
                builderSort = Builders<Note>.Sort.Descending(x => x.Name);
            }
            else if (request.OrderBy == NoteSortingEnum.CreatedOn)
            {
                //creation date
                builderSort = Builders<Note>.Sort.Ascending(x => x.CreatedOnUtc);
            }
            else if (request.OrderBy == NoteSortingEnum.OnSale)
            {
                //on sale
                builderSort = Builders<Note>.Sort.Descending(x => x.OnSale);
            }
            else if (request.OrderBy == NoteSortingEnum.MostViewed)
            {
                //most viewed
                builderSort = Builders<Note>.Sort.Descending(x => x.Viewed);
            }

            var notes = await PagedList<Note>.Create(_noteRepository.Collection, filter, builderSort, request.PageIndex, request.PageSize);

            return (notes, filterableSpecificationAttributeOptionIds);

            #endregion
        }
    }
}
