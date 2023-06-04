﻿using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Customer
{
    public class CustomerNotesModel : BaseForeverNoteModel
    {
        public CustomerNotesModel()
        {
            CustomerNoteList = new List<CustomerNote>();
        }

        public List<CustomerNote> CustomerNoteList { get; set; }
        public string CustomerId { get; set; }
    }

    public class CustomerNote
    {
        public string NoteId { get; set; }
        public string DownloadId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}