using ForeverNote.Services.Users;
using System.Threading.Tasks;

namespace ForeverNote.Services.Tasks
{
    public partial class UserReminderRegisteredUserScheduleTask : IScheduleTask
    {
        private readonly IUserReminderService _userReminderService;
        public UserReminderRegisteredUserScheduleTask(IUserReminderService userReminderService)
        {
            _userReminderService = userReminderService;
        }

        public async Task Execute()
        {
            await _userReminderService.Task_RegisteredUser();
        }
    }
}
