namespace ForeverNote.Core.Domain.Users
{
    public enum UserReminderRuleEnum
    {
        RegisteredUser = 2,
        LastPurchase = 3,
        LastActivity = 4,
    }

    public enum UserReminderConditionTypeEnum
    {
        Note = 1,
        Notebook = 2,
        UserTag = 5,
        UserRegisterField = 6,
        CustomUserAttribute = 7,
    }

    public enum UserReminderConditionEnum
    {
        OneOfThem = 0,
        AllOfThem = 1,
    }
    public enum UserReminderHistoryStatusEnum
    {
        Started = 10,
        CompletedReminder = 20,
    }


}
