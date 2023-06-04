using ForeverNote.Core.Domain.Topics;
using ForeverNote.Web.Areas.Admin.Models.Topics;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Interfaces
{
    public interface ITopicViewModelService
    {
        Task<TopicListModel> PrepareTopicListModel();
        Task PrepareTemplatesModel(TopicModel model);
        Task<Topic> InsertTopicModel(TopicModel model);
        Task<Topic> UpdateTopicModel(Topic topic, TopicModel model);
        Task DeleteTopic(Topic topic);
    }
}
