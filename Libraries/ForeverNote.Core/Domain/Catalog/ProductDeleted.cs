using System;

namespace ForeverNote.Core.Domain.Catalog
{
    public partial class ProductDeleted: Product
    {
        public DateTime DeletedOnUtc { get; set; }
    }
}
