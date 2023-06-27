using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SocialMedia.API.Extensions;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Contratos;
using SocialMedia.Persistence.Models;

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

        [HttpPost("getposts/{userName}")]
        public async Task<IActionResult> GetPosts(string userName, [FromBody] JObject jsonObj)
        {
            try
            {
                // Acessar os dados do objeto JSON
                int pageNumber = (int)jsonObj["PageNumber"];
                int pageSize = (int)jsonObj["PageSize"];

                var pageParams = new PageParams();
                pageParams.PageNumber = pageNumber;
                pageParams.PageSize = pageSize;

                var posts = await _postService.GetAllPostsAsync(userName, pageParams);
                if (posts == null) return NoContent();

                return Ok(posts);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }

        [HttpPost("getposts/following/{userId}")]
        public async Task<IActionResult> GetPostsFollowingPage(int userId, [FromBody] JObject jsonObj)
        {
            try
            {
                // Acessar os dados do objeto JSON
                int pageNumber = (int)jsonObj["PageNumber"];
                int pageSize = (int)jsonObj["PageSize"];

                var pageParams = new PageParams();
                pageParams.PageNumber = pageNumber;
                pageParams.PageSize = pageSize;

                var posts = await _postService.GetPostsFollowingPageAsync(userId, pageParams);
                if (posts == null) return NoContent();

                return Ok(posts);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }

        [HttpPost("getposts/home/{userId}")]
        public async Task<IActionResult> GetPostsHomePage(int userId, [FromBody] JObject jsonObj)
        {
            try
            {
                // Acessar os dados do objeto JSON
                int pageNumber = (int)jsonObj["PageNumber"];
                int pageSize = (int)jsonObj["PageSize"];

                var pageParams = new PageParams();
                pageParams.PageNumber = pageNumber;
                pageParams.PageSize = pageSize;

                var posts = await _postService.GetPostsHomePageAsync(userId, pageParams);
                if (posts == null) return NoContent();

                return Ok(posts);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }


        [HttpGet("getpostbyid/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(User.GetUserId(), postId);
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
        public async Task<IActionResult> UpdatePost(int postId, PostDetailsDto model)
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
                var post = await _postService.GetPostByIdAsync(User.GetUserId(), postId);
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

        [HttpGet("like/{postId}")]
        public async Task<IActionResult> LikePost(int postId)
        {
            try
            {
                var post = await _postService.LikePostAsync(User.GetUserId(), postId);
                if (post == null) return NoContent();

                return Ok(post);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }

        [HttpGet("removelike/{postId}")]
        public async Task<IActionResult> RemoveLikePost(int postId)
        {
            try
            {
                var post = await _postService.RemoveLikePostAsync(User.GetUserId(), postId);
                if (post == null) return NoContent();

                return Ok(post);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
            }
        }
    }
}
