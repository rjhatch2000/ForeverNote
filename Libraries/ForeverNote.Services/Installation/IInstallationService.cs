using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial interface IInstallationService
    {
        Task InstallData(string defaultUserEmail, string defaultUserPassword, string collation);
    }
}
