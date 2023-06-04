using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial interface IUpgradeService
    {
        string DatabaseVersion();
        Task UpgradeData(string fromversion, string toversion);
    }
}
