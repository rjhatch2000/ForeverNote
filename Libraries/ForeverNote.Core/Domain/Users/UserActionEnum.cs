namespace ForeverNote.Core.Domain.Users
{
    public enum UserActionTypeEnum
    {
        AddToCart = 1,
        AddOrder = 2,
        Viewed = 3,
        Url = 4,
        Registration = 5,
        PaidOrder = 6
    }
    public enum UserActionConditionEnum
    {
        OneOfThem = 0,
        AllOfThem = 1,
    }

    public enum UserActionConditionTypeEnum
    {
        Note = 1,
        Notebook = 2,
        UserTag = 8,
        UserRegisterField = 9,
        CustomUserAttribute = 10,
        UrlReferrer = 11,
        UrlCurrent = 12
    }

    public enum UserReactionTypeEnum
    {
        Banner = 1,
        Email = 2,
        AssignToUserTag = 4,
        InteractiveForm = 5,
    }
}
