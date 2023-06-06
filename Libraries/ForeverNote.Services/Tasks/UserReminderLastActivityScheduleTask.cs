using ForeverNote.Services.Users;
using System.Threading.Tasks;

namespace ForeverNote.Services.Tasks
{
    public partial class UserReminderLastActivityScheduleTask : IScheduleTask
    {
        private readonly IUserReminderService _userReminderService;
        public UserReminderLastActivityScheduleTask(IUserReminderService userReminderService)
        {
            _userReminderService = userReminderService;
        }

        public async Task Execute()
        {
            await _userReminderService.Task_LastActivity();
        }
    }
}
