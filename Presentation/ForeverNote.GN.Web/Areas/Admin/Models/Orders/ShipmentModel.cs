using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class ShipmentModel : BaseForeverNoteEntityModel
    {
        public ShipmentModel()
        {
            ShipmentStatusEvents = new List<ShipmentStatusEventModel>();
            Items = new List<ShipmentItemModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ID")]
        public override string Id { get; set; }
        public int ShipmentNumber { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.OrderID")]
        public string OrderId { get; set; }
        public int OrderNumber { get; set; }
        public string OrderCode { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.TotalWeight")]
        public string TotalWeight { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.TrackingNumber")]
        public string TrackingNumber { get; set; }
        public string TrackingNumberUrl { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShippedDate")]
        public DateTime? ShippedDate { get; set; }
        public bool CanShip { get; set; }
        public DateTime? ShippedDateUtc { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.DeliveryDate")]
        public DateTime? DeliveryDate { get; set; }
        public bool CanDeliver { get; set; }
        public DateTime? DeliveryDateUtc { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.AdminComment")]
        public string AdminComment { get; set; }

        public List<ShipmentItemModel> Items { get; set; }

        public IList<ShipmentStatusEventModel> ShipmentStatusEvents { get; set; }

        //shipment notes
        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.DisplayToCustomer")]
        public bool AddShipmentNoteDisplayToCustomer { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.Note")]

        public string AddShipmentNoteMessage { get; set; }
        public bool AddShipmentNoteHasDownload { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.Download")]
        [UIHint("Download")]
        public string AddShipmentNoteDownloadId { get; set; }


        #region Nested classes

        public partial class ShipmentItemModel : BaseForeverNoteEntityModel
        {
            public ShipmentItemModel()
            {
                AvailableWarehouses = new List<WarehouseInfo>();
            }

            public string OrderItemId { get; set; }
            public string ProductId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.Products.ProductName")]
            public string ProductName { get; set; }
            public string Sku { get; set; }
            public string AttributeInfo { get; set; }
            public string RentalInfo { get; set; }
            public bool ShipSeparately { get; set; }

            //weight of one item (product)
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.Products.ItemWeight")]
            public string ItemWeight { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.Products.ItemDimensions")]
            public string ItemDimensions { get; set; }

            public int QuantityToAdd { get; set; }
            public int QuantityOrdered { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.Products.QtyShipped")]
            public int QuantityInThisShipment { get; set; }
            public int QuantityInAllShipments { get; set; }

            public string ShippedFromWarehouse { get; set; }
            public bool AllowToChooseWarehouse { get; set; }
            //used before a shipment is created
            public List<WarehouseInfo> AvailableWarehouses { get; set; }
            public string WarehouseId { get; set; }

            #region Nested Classes
            public class WarehouseInfo : BaseForeverNoteModel
            {
                public string WarehouseId { get; set; }
                public string WarehouseName { get; set; }
                public int StockQuantity { get; set; }
                public int ReservedQuantity { get; set; }
                public int PlannedQuantity { get; set; }
            }

            #endregion
        }

        public partial class ShipmentNote : BaseForeverNoteEntityModel
        {
            public string ShipmentId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.DisplayToCustomer")]
            public bool DisplayToCustomer { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.Note")]
            public string Note { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.Download")]
            public string DownloadId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.Download")]
            public Guid DownloadGuid { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.CreatedOn")]
            public DateTime CreatedOn { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Orders.Shipments.ShipmentNotes.Fields.CreatedByCustomer")]
            public bool CreatedByCustomer { get; set; }
        }

        public partial class ShipmentStatusEventModel : BaseForeverNoteModel
        {
            public string EventName { get; set; }
            public string Location { get; set; }
            public string Country { get; set; }
            public DateTime? Date { get; set; }
        }

        #endregion
    }
}