using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Services.Commands.Models.Users;
using ForeverNote.Services.Users;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Users
{
    public class UserActionEventReactionCommandHandler : IRequestHandler<UserActionEventReactionCommand, bool>
    {
        private readonly IRepository<Banner> _bannerRepository;
        private readonly IRepository<InteractiveForm> _interactiveFormRepository;
        private readonly IRepository<UserActionHistory> _userActionHistoryRepository;
        private readonly IWorkContext _workContext;
        private readonly IPopupService _popupService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;

        public UserActionEventReactionCommandHandler(
            IRepository<Banner> bannerRepository,
            IRepository<InteractiveForm> interactiveFormRepository,
            IRepository<UserActionHistory> userActionHistoryRepository,
            IWorkContext workContext,
            IPopupService popupService,
            IServiceProvider serviceProvider,
            IUserService userService)
        {
            _bannerRepository = bannerRepository;
            _interactiveFormRepository = interactiveFormRepository;
            _userActionHistoryRepository = userActionHistoryRepository;
            _workContext = workContext;
            _popupService = popupService;
            _serviceProvider = serviceProvider;
            _userService = userService;
        }

        public async Task<bool> Handle(UserActionEventReactionCommand request, CancellationToken cancellationToken)
        {
            await Reaction(request.UserActionTypes, request.Action, request.UserId);
            return true;
        }

        public async Task Reaction(
            IList<UserActionType> userActionTypes,
            UserAction action,
            string userId
        )
        {
            if (action.ReactionType == UserReactionTypeEnum.Banner)
            {
                var banner = await _bannerRepository.GetByIdAsync(action.BannerId);
                if (banner != null)
                    await PrepareBanner(action, banner, userId);
            }
            if (action.ReactionType == UserReactionTypeEnum.InteractiveForm)
            {
                var interactiveform = await _interactiveFormRepository.GetByIdAsync(action.InteractiveFormId);
                if (interactiveform != null)
                    await PrepareInteractiveForm(action, interactiveform, userId);
            }

            var user = await _userService.GetUserById(userId);

            if (action.ReactionType == UserReactionTypeEnum.Email)
            {
                var workflowMessageService = _serviceProvider.GetRequiredService<IWorkflowMessageService>();

                if (action.ActionTypeId != userActionTypes.FirstOrDefault(x => x.SystemKeyword == "AddOrder").Id && action.ActionTypeId != userActionTypes.FirstOrDefault(x => x.SystemKeyword == "AddToCart").Id)
                {
                    await workflowMessageService.SendUserActionEvent_Notification(action,
                        _workContext.WorkingLanguage.Id, user);
                }
            }

            if (action.ReactionType == UserReactionTypeEnum.AssignToUserTag)
            {
                await AssignToUserTag(action, user);
            }

            await SaveActionToUser(action.Id, user.Id);

        }

        protected async Task PrepareBanner(UserAction action, Banner banner, string userId)
        {
            var banneractive = new PopupActive() {
                Body = banner.GetLocalized(x => x.Body, _workContext.WorkingLanguage.Id),
                CreatedOnUtc = DateTime.UtcNow,
                UserId = userId,
                UserActionId = action.Id,
                Name = banner.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id),
                PopupTypeId = (int)PopupType.Banner
            };
            await _popupService.InsertPopupActive(banneractive);
        }

        protected async Task PrepareInteractiveForm(UserAction action, InteractiveForm form, string userId)
        {

            var body = PrepareDataInteractiveForm(form);

            var formactive = new PopupActive() {
                Body = body,
                CreatedOnUtc = DateTime.UtcNow,
                UserId = userId,
                UserActionId = action.Id,
                Name = form.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id),
                PopupTypeId = (int)PopupType.InteractiveForm
            };
            await _popupService.InsertPopupActive(formactive);
        }

        protected string PrepareDataInteractiveForm(InteractiveForm form)
        {
            var body = form.GetLocalized(x => x.Body, _workContext.WorkingLanguage.Id);
            body += "<input type=\"hidden\" name=\"Id\" value=\"" + form.Id + "\">";
            foreach (var item in form.FormAttributes)
            {
                if (item.AttributeControlType == FormControlType.TextBox)
                {
                    string _style = string.Format("{0}", item.Style);
                    string _class = string.Format("{0} {1}", "form-control", item.Class);
                    string _value = item.DefaultValue;
                    var textbox = string.Format("<input type=\"text\"  name=\"{0}\" class=\"{1}\" style=\"{2}\" value=\"{3}\" {4}>", item.SystemName, _class, _style, _value, item.IsRequired ? "required" : "");
                    body = body.Replace(string.Format("%{0}%", item.SystemName), textbox);
                }
                if (item.AttributeControlType == FormControlType.MultilineTextbox)
                {
                    string _style = string.Format("{0}", item.Style);
                    string _class = string.Format("{0} {1}", "form-control", item.Class);
                    string _value = item.DefaultValue;
                    var textarea = string.Format("<textarea name=\"{0}\" class=\"{1}\" style=\"{2}\" {3}> {4} </textarea>", item.SystemName, _class, _style, item.IsRequired ? "required" : "", _value);
                    body = body.Replace(string.Format("%{0}%", item.SystemName), textarea);
                }
                if (item.AttributeControlType == FormControlType.Checkboxes)
                {
                    var checkbox = "<div class=\"custom-controls-stacked\">";
                    foreach (var itemcheck in item.FormAttributeValues.OrderBy(x => x.DisplayOrder))
                    {
                        string _style = string.Format("{0}", item.Style);
                        string _class = string.Format("{0} {1}", "custom-control-input", item.Class);

                        checkbox += "<div class=\"custom-control custom-checkbox\">";
                        checkbox += string.Format("<input type=\"checkbox\" class=\"{0}\" style=\"{1}\" {2} id=\"{3}\" name=\"{4}\" value=\"{5}\">", _class, _style,
                            itemcheck.IsPreSelected ? "checked" : "", itemcheck.Id, item.SystemName, itemcheck.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id));
                        checkbox += string.Format("<label class=\"custom-control-label\" for=\"{0}\">{1}</label>", itemcheck.Id, itemcheck.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id));
                        checkbox += "</div>";
                    }
                    checkbox += "</div>";
                    body = body.Replace(string.Format("%{0}%", item.SystemName), checkbox);
                }

                if (item.AttributeControlType == FormControlType.DropdownList)
                {
                    var dropdown = string.Empty;
                    string _style = string.Format("{0}", item.Style);
                    string _class = string.Format("{0} {1}", "form-control custom-select", item.Class);

                    dropdown = string.Format("<select name=\"{0}\" class=\"{1}\" style=\"{2}\">", item.SystemName, _class, _style);
                    foreach (var itemdropdown in item.FormAttributeValues.OrderBy(x => x.DisplayOrder))
                    {
                        dropdown += string.Format("<option value=\"{0}\" {1}>{2}</option>", itemdropdown.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id), itemdropdown.IsPreSelected ? "selected" : "", itemdropdown.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id));
                    }
                    dropdown += "</select>";
                    body = body.Replace(string.Format("%{0}%", item.SystemName), dropdown);
                }
                if (item.AttributeControlType == FormControlType.RadioList)
                {
                    var radio = "<div class=\"custom-controls-stacked\">";
                    foreach (var itemradio in item.FormAttributeValues.OrderBy(x => x.DisplayOrder))
                    {
                        string _style = string.Format("{0}", item.Style);
                        string _class = string.Format("{0} {1}", "custom-control-input", item.Class);

                        radio += "<div class=\"custom-control custom-radio\">";
                        radio += string.Format("<input type=\"radio\" class=\"{0}\" style=\"{1}\" {2} id=\"{3}\" name=\"{4}\" value=\"{5}\">", _class, _style,
                            itemradio.IsPreSelected ? "checked" : "", itemradio.Id, item.SystemName, itemradio.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id));
                        radio += string.Format("<label class=\"custom-control-label\" for=\"{0}\">{1}</label>", itemradio.Id, itemradio.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id));
                        radio += "</div>";
                    }
                    radio += "</div>";
                    body = body.Replace(string.Format("%{0}%", item.SystemName), radio);
                }
            }
            body = body.Replace("%sendbutton%", "<input type=\"submit\" id=\"send-interactive-form\" class=\"btn btn-success interactive-form-button\" value=\"Send\" \" />");
            body = body.Replace("%errormessage%", "<div class=\"message-error\"><div class=\"validation-summary-errors\"><div id=\"errorMessages\"></div></div></div>");

            return body;
        }

        protected async Task AssignToUserTag(UserAction action, User user)
        {
            if (user.UserTags.Where(x => x == action.UserTagId).Count() == 0)
            {
                var userTagService = _serviceProvider.GetRequiredService<IUserTagService>();
                await userTagService.InsertTagToUser(action.UserTagId, user.Id);
            }
        }

        protected async Task SaveActionToUser(string actionId, string userId)
        {
            await _userActionHistoryRepository.InsertAsync(new UserActionHistory() { UserId = userId, UserActionId = actionId, CreateDateUtc = DateTime.UtcNow });
        }

    }
}
