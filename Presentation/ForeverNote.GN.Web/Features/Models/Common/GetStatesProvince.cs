using ForeverNote.Web.Models.Common;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Common
{
    public class GetStatesProvince : IRequest<IList<StateProvinceModel>>
    {
        public string CountryId { get; set; }
        public bool AddSelectStateItem { get; set; }
    }
}
