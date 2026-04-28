using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Comment.Context;
using MultiShop.Comment.Dtos;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _context;
        private readonly IMapper _mapper;

        public CommentsController(CommentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetComments()
        {
            var values = _context.UserComments.ToList();
            return Ok(_mapper.Map<List<ResultUserCommentDto>>(values));
        }

        [HttpPost]
        public IActionResult AddComment(CreateUserCommentDto userCommentDto)
        {
            _context.UserComments.Add(_mapper.Map<UserComment>(userCommentDto));
            _context.SaveChanges();
            return Ok("Comment is added successfully");
        }

        [HttpPut]
        public IActionResult UpdateComment(ResultUserCommentDto userCommentDto)
        {
            _context.UserComments.Update(_mapper.Map<UserComment>(userCommentDto));
            _context.SaveChanges();
            return Ok("Comment is updated successfully");
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id)
        {
            var value = _context.UserComments.Find(id);
            return Ok(_mapper.Map<ResultUserCommentDto>(value));
        }

        [HttpDelete]
        public IActionResult DeleteComment(int id)
        {
            var value = _context.UserComments.Find(id);
            _context.UserComments.Remove(value);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("CommentListByProductId/{id}")]
        public IActionResult GetCommentListByProductId(string id)
        {
            var values = _context.UserComments.Where(x => x.ProductId == id).ToList();
            return Ok(_mapper.Map<List<ResultUserCommentDto>>(values));
        }

        [HttpGet("GetActiveCommentCount")]
        public IActionResult GetActiveCommentCount()
        {
            int value = _context.UserComments.Where(x => x.Status == true).Count();
            return Ok(value);
        }

        [HttpGet("GetPassiveCommentCount")]
        public IActionResult GetPassiveCommentCount()
        {
            int value = _context.UserComments.Where(x => x.Status == false).Count();
            return Ok(value);
        }

        [HttpGet("GetTotalCommentCount")]
        public IActionResult GetTotalCommentCount()
        {
            int value = _context.UserComments.Count();
            return Ok(value);
        }
    }
}
