using MultiShop.DtoLayer.CommentDtos;

namespace MultiShop.WebUI.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            return _httpClient.GetFromJsonAsync<List<ResultCommentDto>>("comments");
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            await _httpClient.PostAsJsonAsync("comments", createCommentDto);
        }
        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            await _httpClient.PutAsJsonAsync("comments", updateCommentDto);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _httpClient.DeleteAsync($"comments?id={id}");
        }

        public async Task<UpdateCommentDto> GetByIdCommentAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateCommentDto>($"comments/{id}");
        }

        public async Task<List<ResultCommentDto>> GetCommentListByProductIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<List<ResultCommentDto>>($"comments/CommentListByProductId/{id}");
        }

        public async Task<int> GetTotalCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("comments/GetTotalCommentCount");
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }

        public async Task<int> GetActiveCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("comments/GetActiveCommentCount");
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }
       
        public async Task<int> GetPassiveCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("comments/GetPassiveCommentCount");
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }
    }
}
