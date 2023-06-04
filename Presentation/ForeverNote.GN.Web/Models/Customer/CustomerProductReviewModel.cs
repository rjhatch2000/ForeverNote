﻿using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Customer
{
    public partial class CustomerProductReviewModel : BaseForeverNoteEntityModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSeName { get; set; }
        public string StoreName { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public string ReplyText { get; set; }
        public string Signature { get; set; }
        public int Rating { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CustomerProductReviewsModel : BaseForeverNoteEntityModel
    {
        public string CustomerId { get; set; }

        public string CustomerInfo { get; set; }
        public IList<CustomerProductReviewModel> Reviews { get; set; }

        public CustomerProductReviewsModel()
        {
            Reviews = new List<CustomerProductReviewModel>();
        }
    }
}