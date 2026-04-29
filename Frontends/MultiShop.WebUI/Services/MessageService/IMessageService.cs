using MultiShop.DtoLayer.MessageDtos;

namespace MultiShop.WebUI.Services.MessageService
{
    public interface IMessageService
    {
        Task<List<ResultInboxMessageDto>> GetInboxMessageAsync(string id);
        Task<List<ResultSendboxMessageDto>> GetSendboxMessageAsync(string id);

        //Task CreateMessageAsync(CreateMessageDto createMessageDto);
        //Task UpdateMessageAsync(UpdateMessageDto updateMessageDto);
        //Task DeleteMessageAsync(int id);
        //Task<GetByIdMessageDto> GetByIdMessageAsync(int id);
        //Task<int> GetTotalMessageCount();
        Task<int> GetTotalMessageCountByReceiverId(string id);
    }
}
