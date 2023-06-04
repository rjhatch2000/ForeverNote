using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Polls
{
    public partial class PollModel : BaseForeverNoteEntityModel
    {
        public PollModel()
        {
            Answers = new List<PollAnswerModel>();
        }

        public string Name { get; set; }

        public bool AlreadyVoted { get; set; }

        public int TotalVotes { get; set; }
        
        public IList<PollAnswerModel> Answers { get; set; }

    }

    public partial class PollAnswerModel : BaseForeverNoteEntityModel
    {
        public string Name { get; set; }
        public string PollId { get; set; }
        public int NumberOfVotes { get; set; }

        public double PercentOfTotalVotes { get; set; }
    }
}