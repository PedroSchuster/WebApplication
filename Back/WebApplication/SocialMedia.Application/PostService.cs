using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Contratos;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence;
using SocialMedia.Persistence.Contratos;

namespace SocialMedia.Application
{
    public class PostService : IPostService
    {
        private readonly IPostPersist _postPersist;
        private readonly IMapper _mapper;

        public PostService(IPostPersist postPersist, IMapper mapper)
        {
            _postPersist = postPersist;
            _mapper = mapper;

        }

        public async Task<PostDto> AddPosts(int userId, PostDto model)
        {
            try
            {
                var post = _mapper.Map<Post>(model);
                post.UserId = userId; // ver se precisa

                _postPersist.Add<Post>(post);

                if (await _postPersist.SaveChangesAsync())
                {
                    var resultado = await _postPersist.GetPostByIdAsync(post.Id, userId); // verificaçao se foi criado msm
                    if (post.RootId != null || post.RootId != 0)
                    {
                        await SaveComment(post.Id, post.RootId.Value);
                    }
                    return _mapper.Map<PostDto>(resultado);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SaveComment(int id, int postId)
        {
            try
            {
            var postComment = new PostComment();
            postComment.PostId =  postId;
            postComment.CommentId = id;
            _postPersist.Add<PostComment>(postComment);
            await _postPersist.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Remove(int userId, int postId)
        {
            try
            {
                var post = await _postPersist.GetPostByIdAsync(postId, userId);
                if (post == null) throw new Exception("Post para delete não encontrado. ");

                _postPersist.Delete<Post>(post);

                return await _postPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PostDto> UpdatePost(int userId, int postId, PostDto model)
        {
            try
            {
                var post = await _postPersist.GetPostByIdAsync(postId, userId);
                if (post == null) return null;

                model.Id = post.Id;
                model.UserId = post.UserId;

                _mapper.Map(model, post);

                _postPersist.Update<Post>(post);

                if (await _postPersist.SaveChangesAsync())
                {
                    var resultado = await _postPersist.GetPostByIdAsync(post.Id, userId); // verificaçao se foi criado msm
                    return _mapper.Map<PostDto>(resultado);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int userId)
        {
            try
            {
                var posts = await _postPersist.GetAllPostsAsync(userId);
                if (posts == null) return null;
                
                return _mapper.Map<IEnumerable<PostDto>>(posts);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PostDto>> GetAllCommentsAsync(int postId, int userId)
        {
            try
            {
                var comments = await _postPersist.GetAllCommentsByPostIdAsync(postId);
                if (comments == null) return null;

                foreach (var comment in comments)
                {
                    comment.Comment = await _postPersist.GetPostByIdAsync(comment.CommentId, userId);
                    comment.PostId = postId;
                }
                List<Post> result = comments.Select(x => x.Comment).ToList();
                return _mapper.Map<List<PostDto>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PostDto> GetPostByIdAsync(int postId, int userId)
        {
            try
            {
                var post = await _postPersist.GetPostByIdAsync(postId, userId);
                if (post == null) return null;

                var result = _mapper.Map<PostDto>(post);
                result.Comments = await GetAllCommentsAsync(postId, userId);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
