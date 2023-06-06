using MediatR;

namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// User logged-in event
    /// </summary>
    public class UserLoggedinEvent : INotification
    {
        public UserLoggedinEvent(User user)
        {
            this.User = user;
        }

        /// <summary>
        /// User
        /// </summary>
        public User User
        {
            get; private set;
        }
    }

    /// <summary>
    /// User registered event
    /// </summary>
    public class UserRegisteredEvent : INotification
    {
        public UserRegisteredEvent(User user)
        {
            this.User = user;
        }

        /// <summary>
        /// User
        /// </summary>
        public User User
        {
            get; private set;
        }
    }

}