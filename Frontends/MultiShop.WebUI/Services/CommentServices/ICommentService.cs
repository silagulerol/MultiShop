using MultiShop.DtoLayer.CommentDtos;

namespace MultiShop.WebUI.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDto>> GetAllCommentAsync();
        Task UpdateCommentAsync(UpdateCommentDto updateCommentDto);
        Task CreateCommentAsync(CreateCommentDto createCommentDto);
        Task DeleteCommentAsync(string id);
        Task<UpdateCommentDto> GetByIdCommentAsync(string id);
        Task<List<ResultCommentDto>> GetCommentListByProductIdAsync(string id);
    }
}
