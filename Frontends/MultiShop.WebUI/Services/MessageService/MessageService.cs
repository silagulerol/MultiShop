using MultiShop.DtoLayer.MessageDtos;

namespace MultiShop.WebUI.Services.MessageService
{
    public class MessageService : IMessageService
    {
        public readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultInboxMessageDto>> GetInboxMessageAsync(string id)
        {
            var values= await _httpClient.GetFromJsonAsync<List<ResultInboxMessageDto>>($"UserMessages/GetMessageInbox?id={id}");
            return values;
        }

        public Task<List<ResultSendboxMessageDto>> GetSendboxMessageAsync(string id)
        {
            var values = _httpClient.GetFromJsonAsync<List<ResultSendboxMessageDto>>($"UserMessages/GetMessageSendbox?id={id}");
            return values;
        }

        public Task<int> GetTotalMessageCountByReceiverId(string id)
        {
            var values = _httpClient.GetFromJsonAsync<int>($"UserMessages/GetTotalMessageCountByReceiverId?id={id}");
            return values;
        }
    }
}
