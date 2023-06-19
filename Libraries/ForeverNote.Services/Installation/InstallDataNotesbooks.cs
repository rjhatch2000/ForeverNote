using ForeverNote.Core.Domain.Notebooks;
using ForeverNote.Services.Media;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial class InstallationService
    {
        protected virtual async Task InstallNotebooks()
        {
            var pictureService = _serviceProvider.GetRequiredService<IPictureService>();

            //sample pictures
            var sampleImagesPath = GetSamplesPath();

            //notebooks
            var allNotebooks = new List<Notebook>();
            var notebookComputers = new Notebook
            {
                Name = "Inbox",
                PageSize = 6,
                ParentNotebookId = "",
                Flag = "New",
                FlagStyle = "badge-danger",
                DisplayOrder = 100,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };

            notebookComputers.PictureId = (await pictureService.InsertPicture(
                File.ReadAllBytes(sampleImagesPath + "notebook_computers.jpeg"),
                "image/jpeg",
                ////pictureService.GetPictureSeName("Computers"),
                "Inbox", //TODO: Delete this field ?
                reference: ForeverNote.Core.Domain.Common.Reference.Notebook,
                objectId: notebookComputers.Id)).Id;
            allNotebooks.Add(notebookComputers);
        }
    }
}
