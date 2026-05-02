using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.MessageService;

namespace MultiShop.WebUI.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutHeaderComponentPartial : ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly ICommentService  _commentService;

        public _AdminLayoutHeaderComponentPartial(IMessageService messageService, IUserService userService, ICommentService commentService)
        {
            _messageService = messageService;
            _userService = userService;
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserInfo();
            //int totalMessageCountByReceiverId = await _messageService.GetTotalMessageCountByReceiverId(user.Id);
            int totalCommentCountByReceiverId = await _commentService.GetTotalCommentCount();
            //ViewBag.messageCount = totalMessageCountByReceiverId;
            ViewBag.commentCount = totalCommentCountByReceiverId;
            return View();
        }
    }
}
