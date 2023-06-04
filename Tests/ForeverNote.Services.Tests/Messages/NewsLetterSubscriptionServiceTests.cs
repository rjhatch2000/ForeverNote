using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Events;
using ForeverNote.Services.Common;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Events;
using ForeverNote.Services.Tests;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages.Tests
{
    [TestClass()]
    public class NewsLetterSubscriptionServiceTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IRepository<NewsLetterSubscription>> _subscriptionRepository;
        private Mock<IHistoryService> _historyServiceMock;
        private INewsLetterSubscriptionService _newsLetterSubscriptionService;

        [TestInitialize()]
        public void TestInitialize()
        {
            _mediatorMock = new Mock<IMediator>();
            _subscriptionRepository = new Mock<IRepository<NewsLetterSubscription>>();
            _historyServiceMock = new Mock<IHistoryService>();
            _newsLetterSubscriptionService = new NewsLetterSubscriptionService(_subscriptionRepository.Object, _mediatorMock.Object, _historyServiceMock.Object);
        }


        [TestMethod]
        public void InsertNewsLetterSubscription_InvalidEmail_ThrowException()
        {
            var email = "NotValidEmail";
            var newsLetterSubscription = new NewsLetterSubscription();
            newsLetterSubscription.Email = email;
            Assert.ThrowsExceptionAsync<ForeverNoteException>(async () => await _newsLetterSubscriptionService.InsertNewsLetterSubscription(newsLetterSubscription));
        }

        [TestMethod]
        public async Task InsertNewsLetterSubscription_ActiveSubcription_InvokeRepositoryAndPublishSubscriptionEvent()
        {
            var email = "johny@gmail.com";
            var newsLetterSubscription = new NewsLetterSubscription() { Email=email,Active=true};
            await _newsLetterSubscriptionService.InsertNewsLetterSubscription(newsLetterSubscription);
            _subscriptionRepository.Verify(r => r.InsertAsync(newsLetterSubscription), Times.Once);
            _historyServiceMock.Verify(h => h.SaveObject(It.IsAny<BaseEntity>()), Times.Once);
            _mediatorMock.Verify(m => m.Publish<EmailSubscribedEvent>(It.IsAny<EmailSubscribedEvent>(), default(CancellationToken)), Times.Once);
            _mediatorMock.Verify(c => c.Publish(It.IsAny<EntityInserted<NewsLetterSubscription>>(), default(CancellationToken)), Times.Once);
        }

        [TestMethod]
        public async Task InsertNewsLetterSubscription_InactiveSubcription_InvokeRepository()
        {
            var email = "johny@gmail.com";
            var newsLetterSubscription = new NewsLetterSubscription() { Email = email, Active = false };
            await _newsLetterSubscriptionService.InsertNewsLetterSubscription(newsLetterSubscription);
            _subscriptionRepository.Verify(r => r.InsertAsync(newsLetterSubscription), Times.Once);
            _historyServiceMock.Verify(h => h.SaveObject(It.IsAny<BaseEntity>()), Times.Once);
            _mediatorMock.Verify(m => m.Publish<EmailSubscribedEvent>(It.IsAny<EmailSubscribedEvent>(), default(CancellationToken)), Times.Never);
            _mediatorMock.Verify(c => c.Publish(It.IsAny<EntityInserted<NewsLetterSubscription>>(), default(CancellationToken)), Times.Once);
        }

        [TestMethod]
        public async Task UpdateNewsLetterSubscription_InvokeRepository()
        {
            var email = "johny@gmail.com";
            var newsLetterSubscription = new NewsLetterSubscription() { Email = email, Active = false };
            await _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsLetterSubscription);
            _subscriptionRepository.Verify(r => r.UpdateAsync(newsLetterSubscription), Times.Once);
            _historyServiceMock.Verify(h => h.SaveObject(It.IsAny<BaseEntity>()), Times.Once);
            _mediatorMock.Verify(c => c.Publish(It.IsAny<EntityUpdated<NewsLetterSubscription>>(), default(CancellationToken)), Times.Once);
        }

        [TestMethod]
        public async Task DeleteNewsLetterSubscription_InvokeRepositoryAndEmailUnsubscribedEvent()
        {
            var email = "johny@gmail.com";
            var newsLetterSubscription = new NewsLetterSubscription() { Email = email, Active = false };
            await _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsLetterSubscription);
            _subscriptionRepository.Verify(r => r.DeleteAsync(newsLetterSubscription), Times.Once);
            _mediatorMock.Verify(m => m.Publish<EmailUnsubscribedEvent>(It.IsAny<EmailUnsubscribedEvent>(), default(CancellationToken)), Times.Once);
            _mediatorMock.Verify(c => c.Publish(It.IsAny<EntityDeleted<NewsLetterSubscription>>(), default(CancellationToken)), Times.Once);
        }
    }
}