using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Media;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial class InstallationService
    {
        protected virtual async Task InstallNotes(string defaultUserEmail)
        {
            var pictureService = _serviceProvider.GetRequiredService<IPictureService>();
            var downloadService = _serviceProvider.GetRequiredService<IDownloadService>();

            //default user/user
            var defaultUser = _userRepository.Table.FirstOrDefault(x => x.Email == defaultUserEmail);
            if (defaultUser == null)
                throw new Exception("Cannot load default user");

            //pictures
            var sampleImagesPath = GetSamplesPath();

            //downloads
            var sampleDownloadsPath = GetSamplesPath();

            //notes
            var allNotes = new List<Note>();

            #region Computers

            //TODO: Finish this...
            var noteBuildComputer = new Note
            {
                Name = "Build your own computer",
                ShortDescription = "Build it",
                FullDescription = "<p>Fight back against cluttered workspaces with the stylish DELL Inspiron desktop PC, featuring powerful computing resources and a stunning 20.1-inch widescreen display with stunning XBRITE-HiColor LCD technology. The black IBM zBC12 has a built-in microphone and MOTION EYE camera with face-tracking technology that allows for easy communication with friends and family. And it has a built-in DVD burner and Sony's Movie Store software so you can create a digital entertainment library for personal viewing at your convenience. Easy to setup and even easier to use, this JS-series All-in-One includes an elegantly designed keyboard and a USB mouse.</p>",
                ShowOnHomePage = true,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                NoteNotebooks =
                {
                    new NoteNotebook
                    {
                        NotebookId = _notebookRepository.Table.Single(c => c.Name == "Inbox").Id,
                        DisplayOrder = 1,
                    }
                }
            };
            allNotes.Add(noteBuildComputer);

            await _noteRepository.InsertAsync(noteBuildComputer);

            #endregion

        }
    }
}
