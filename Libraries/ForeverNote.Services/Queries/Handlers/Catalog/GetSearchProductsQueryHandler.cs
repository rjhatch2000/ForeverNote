using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Queries.Models.Catalog;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Queries.Handlers.Catalog
{
    public class GetSearchProductsQueryHandler : IRequestHandler<GetSearchProductsQuery, (IPagedList<Product> products, IList<string> filterableSpecificationAttributeOptionIds)>
    {

        private readonly IRepository<Product> _productRepository;

        private readonly CatalogSettings _catalogSettings;
        private readonly CommonSettings _commonSettings;

        public GetSearchProductsQueryHandler(
            IRepository<Product> productRepository,
            CatalogSettings catalogSettings,
            CommonSettings commonSettings
        )
        {
            _productRepository = productRepository;

            _catalogSettings = catalogSettings;
            _commonSettings = commonSettings;
        }

        public async Task<(IPagedList<Product> products, IList<string> filterableSpecificationAttributeOptionIds)>
            Handle(GetSearchProductsQuery request, CancellationToken cancellationToken)
        {
            var filterableSpecificationAttributeOptionIds = new List<string>();

            //validate "categoryIds" parameter
            if (request.CategoryIds != null && request.CategoryIds.Contains(""))
                request.CategoryIds.Remove("");

            //Access control list. Allowed customer roles
            var allowedCustomerRolesIds = request.Customer.GetCustomerRoleIds();

            #region Search products

            //products
            var builder = Builders<Product>.Filter;
            var filter = FilterDefinition<Product>.Empty;

            //category filtering
            if (request.CategoryIds != null && request.CategoryIds.Any())
            {

                if (request.FeaturedProducts.HasValue)
                {
                    filter = filter & builder.Where(x => x.ProductCategories.Any(y => request.CategoryIds.Contains(y.CategoryId)
                        && y.IsFeaturedProduct == request.FeaturedProducts));
                }
                else
                {
                    filter = filter & builder.Where(x => x.ProductCategories.Any(y => request.CategoryIds.Contains(y.CategoryId)));
                }
            }

            //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
            //That's why we pass the date value
            var nowUtc = DateTime.UtcNow;
            if (request.MarkedAsNewOnly)
            {
                filter = filter & builder.Where(p => p.MarkAsNew);
                filter = filter & builder.Where(p =>
                    (!p.MarkAsNewStartDateTimeUtc.HasValue || p.MarkAsNewStartDateTimeUtc.Value < nowUtc) &&
                    (!p.MarkAsNewEndDateTimeUtc.HasValue || p.MarkAsNewEndDateTimeUtc.Value > nowUtc));
            }

            //searching by keyword
            if (!String.IsNullOrWhiteSpace(request.Keywords))
            {
                if (_commonSettings.UseFullTextSearch)
                {
                    request.Keywords = "\"" + request.Keywords + "\"";
                    request.Keywords = request.Keywords.Replace("+", "\" \"");
                    request.Keywords = request.Keywords.Replace(" ", "\" \"");
                    filter = filter & builder.Text(request.Keywords);
                }
                else
                {
                    if (!request.SearchDescriptions)
                        filter = filter & builder.Where(p =>
                            p.Name.ToLower().Contains(request.Keywords.ToLower())
                            ||
                            p.Locales.Any(x => x.LocaleKey == "Name" && x.LocaleValue != null && x.LocaleValue.ToLower().Contains(request.Keywords.ToLower()))
                            );
                    else
                    {
                        filter = filter & builder.Where(p =>
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
            if (!string.IsNullOrEmpty(request.ProductTag))
            {
                filter = filter & builder.Where(x => x.ProductTags.Any(y => y == request.ProductTag));
            }

            var builderSort = Builders<Product>.Sort.Descending(x => x.CreatedOnUtc);

            if (request.OrderBy == ProductSortingEnum.Position && request.CategoryIds != null && request.CategoryIds.Any())
            {
                //category position
                builderSort = Builders<Product>.Sort.Ascending(x => x.DisplayOrderCategory);
            }
            else if (request.OrderBy == ProductSortingEnum.Position)
            {
                //otherwise sort by name
                builderSort = Builders<Product>.Sort.Ascending(x => x.Name);
            }
            else if (request.OrderBy == ProductSortingEnum.NameAsc)
            {
                //Name: A to Z
                builderSort = Builders<Product>.Sort.Ascending(x => x.Name);
            }
            else if (request.OrderBy == ProductSortingEnum.NameDesc)
            {
                //Name: Z to A
                builderSort = Builders<Product>.Sort.Descending(x => x.Name);
            }
            else if (request.OrderBy == ProductSortingEnum.CreatedOn)
            {
                //creation date
                builderSort = Builders<Product>.Sort.Ascending(x => x.CreatedOnUtc);
            }
            else if (request.OrderBy == ProductSortingEnum.OnSale)
            {
                //on sale
                builderSort = Builders<Product>.Sort.Descending(x => x.OnSale);
            }
            else if (request.OrderBy == ProductSortingEnum.MostViewed)
            {
                //most viewed
                builderSort = Builders<Product>.Sort.Descending(x => x.Viewed);
            }
            else if (request.OrderBy == ProductSortingEnum.BestSellers)
            {
                //best seller
                builderSort = Builders<Product>.Sort.Descending(x => x.Sold);
            }

            var products = await PagedList<Product>.Create(_productRepository.Collection, filter, builderSort, request.PageIndex, request.PageSize);

            return (products, filterableSpecificationAttributeOptionIds);

            #endregion
        }
    }
}
