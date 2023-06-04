﻿using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Features.Models.Customers;
using ForeverNote.Web.Models.Customer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Customers
{
    public class GetNavigationHandler : IRequestHandler<GetNavigation, CustomerNavigationModel>
    {

        private readonly CustomerSettings _customerSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly ForumSettings _forumSettings;
        private readonly OrderSettings _orderSettings;
        private readonly VendorSettings _vendorSettings;

        public GetNavigationHandler(
            CustomerSettings customerSettings, 
            RewardPointsSettings rewardPointsSettings, 
            ForumSettings forumSettings, 
            OrderSettings orderSettings, 
            VendorSettings vendorSettings)
        {
            _customerSettings = customerSettings;
            _rewardPointsSettings = rewardPointsSettings;
            _forumSettings = forumSettings;
            _orderSettings = orderSettings;
            _vendorSettings = vendorSettings;
        }

        public async Task<CustomerNavigationModel> Handle(GetNavigation request, CancellationToken cancellationToken)
        {
            var model = new CustomerNavigationModel();
            model.HideAvatar = !_customerSettings.AllowCustomersToUploadAvatars;
            model.HideRewardPoints = !_rewardPointsSettings.Enabled;
            model.HideDeleteAccount = !_customerSettings.AllowUsersToDeleteAccount;
            model.HideForumSubscriptions = !_forumSettings.ForumsEnabled || !_forumSettings.AllowCustomersToManageSubscriptions;
            model.HideReturnRequests = !_orderSettings.ReturnRequestsEnabled;
            model.HideDownloadableProducts = _customerSettings.HideDownloadableProductsTab;
            model.HideBackInStockSubscriptions = _customerSettings.HideBackInStockSubscriptionsTab;
            model.HideAuctions = _customerSettings.HideAuctionsTab;
            model.HideNotes = _customerSettings.HideNotesTab;
            model.HideDocuments = _customerSettings.HideDocumentsTab;
            model.HideReviews = _customerSettings.HideReviewsTab;
            model.HideCourses = _customerSettings.HideCoursesTab;

            if (_vendorSettings.AllowVendorsToEditInfo && request.Vendor != null)
            {
                model.ShowVendorInfo = true;
            }
            model.SelectedTab = (CustomerNavigationEnum)request.SelectedTabId;

            return await Task.FromResult(model);
        }
    }
}
