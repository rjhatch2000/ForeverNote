namespace ForeverNote.Web.Framework.Mvc.Models
{
    public class DeleteConfirmationModel : BaseForeverNoteEntityModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string WindowId { get; set; }
    }
}