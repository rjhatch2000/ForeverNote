using System.Threading.Tasks;

namespace ForeverNote.Core.Caching.Message
{
    public interface IMessageSubscriber
    {
        Task SubscribeAsync();
    }
}
