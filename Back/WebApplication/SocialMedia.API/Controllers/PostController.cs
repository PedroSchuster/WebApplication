using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Extensions;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Contratos;

namespace SocialMedia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("getposts/{userId}")]
        public async Task<IActionResult> GetPosts(int userId)
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync(userId);
                if (posts == null) return NoContent();

                return Ok(posts);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }

        [HttpGet("getpostbyid/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(postId);
                if (post == null) return NoContent();

                return Ok(post);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }

        [HttpGet("comments/{postId}")]
        public async Task<IActionResult> GetAllCommentsByPostId(int postId)
        {
            var comments = await _postService.GetAllCommentsAsync(postId);
            if (comments == null) return NoContent();
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(PostDto model)
        {
            try
            {
                var post = await _postService.AddPosts(User.GetUserId(), User.GetUserName(), model);
                if (post == null) return NoContent();

                return Ok(post);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, PostDto model)
        {
            try
            {
                var post = await _postService.UpdatePost(User.GetUserId(), postId, model);
                if (post == null) return BadRequest("Erro ao tentar atualizar post");

                return Ok(post);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }



        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(postId);
                if (post == null) return NoContent();

                if (await _postService.Remove(User.GetUserId(), postId))
                {
                    return Ok(new { message = "Deletado" });
                }
                
                 return BadRequest("Erro ao tentar deletar post");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }
    }
}
