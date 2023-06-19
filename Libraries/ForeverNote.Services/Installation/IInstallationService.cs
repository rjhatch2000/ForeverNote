using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial interface IInstallationService
    {
        Task InstallData(
            string httpscheme, HostString host,
            string defaultUserEmail, string defaultUserPassword, string collation, bool installSampleData = true,
            string companyName = "", string companyAddress = "", string companyPhoneNumber = "", string companyEmail = "");
    }
}
