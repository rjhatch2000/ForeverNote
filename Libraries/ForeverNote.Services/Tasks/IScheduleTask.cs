using System.Threading.Tasks;

namespace ForeverNote.Services.Tasks
{
    public interface IScheduleTask
    {
        Task Execute();
    }
}
