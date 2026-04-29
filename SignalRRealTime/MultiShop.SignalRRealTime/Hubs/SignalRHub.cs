using Microsoft.AspNetCore.SignalR;
using MultiShop.Services.SignalRRealTime.Services.SignalRCommentServices;
using MultiShop.Services.SignalRRealTime.Services.SignalRMessageServices;

namespace MultiShop.Services.SignalRRealTime.Hubs
{
    /*
     * Veri akışı
        1. Client server’a bağlanır
        2. Hub’a subscribe olur
        3. Server veri üretir
        4. Hub → tüm clientlara gönder
     */
    public class SignalRHub : Hub
    {
        //Amaç notification and message sayılarını getirmek ve clientlara göndermek
        private readonly ISignalRCommentService _signalRCommentService;
        private readonly ISignalRMessageService _signalRMessageService;

        public SignalRHub(ISignalRCommentService signalRCommentService, ISignalRMessageService signalRMessageService)
        {
            _signalRCommentService = signalRCommentService;
            _signalRMessageService = signalRMessageService;
        }

        public async Task SendStatisticCount()
        {
            var getTotalCommentCount = await _signalRCommentService.GetTotalCommentCount();
            // burada clientlara gönderiyoruz. clientlarda ReceiveCommentCount metodunu yakalayarak commentCount değerini alacaklar
            await Clients.All.SendAsync("ReceiveCommentCount", getTotalCommentCount);

            //var messageCount = await _signalRMessageService.GetTotalMessageCountByReceiverId(id);
            //await Clients.All.SendAsync("ReceiveMessageCount", messageCount);
        }
    }
}
