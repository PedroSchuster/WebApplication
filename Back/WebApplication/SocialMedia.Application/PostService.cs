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
        private readonly IUserPersist _userPersist;
        private readonly IMapper _mapper;

        public PostService(IPostPersist postPersist, IUserPersist userPersist,IMapper mapper)
        {
            _postPersist = postPersist;
            _userPersist = userPersist;
            _mapper = mapper;

        }

        public async Task<PostDto> AddPosts(int userId, string userName,PostDto model)
        {
            try
            {
                var post = _mapper.Map<Post>(model);
                post.UserId = userId; // ver se precisa
                post.UserName = userName;

                _postPersist.Add<Post>(post);

                if (await _postPersist.SaveChangesAsync())
                {
                    var resultado = await _postPersist.GetPostByIdAsync(post.Id, userId); // verificaçao se foi criado msm
                    if (post.RootId != null && post.RootId != 0)
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

        public async Task<IEnumerable<PostTLDto>> GetAllPostsAsync(int userId)
        {
            try
            {
                var posts = await _postPersist.GetAllPostsAsync(userId);
                if (posts == null) return null;
                
                return _mapper.Map<IEnumerable<PostTLDto>>(posts);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PostDetailsDto>> GetAllParentsAsync(int postId, int userId)
        {
            try
            {
                var currentPost = await _postPersist.GetPostByIdAsync(postId, userId);
                int? rootId = currentPost.RootId;
                List<PostDetailsDto> parents = new List<PostDetailsDto>();

                while (rootId != null)
                {
                    currentPost = await _postPersist.GetPostByIdAsync(rootId.Value, userId);
                    parents.Add(_mapper.Map<PostDetailsDto>(currentPost));
                    rootId = currentPost.RootId;
                }
              
                return parents.OrderBy(x=>x.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PostDetailsDto>> GetAllCommentsAsync(int postId, int userId)
        {
            try
            {
                var comments = await _postPersist.GetAllCommentsByPostIdAsync(postId);
                if (comments == null) return null;

                foreach (var comment in comments)
                {
                    comment.Comment = await _postPersist.GetPostByIdAsync(comment.CommentId, userId);
                }
                List<Post> result = comments.Select(x => x.Comment).ToList();
                return _mapper.Map<List<PostDetailsDto>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PostDetailsDto> GetPostByIdAsync(int postId, int userId)
        {
            try
            {
                var post = await _postPersist.GetPostByIdAsync(postId, userId);
                if (post == null) return null;

                var postMapped =  _mapper.Map<PostDetailsDto>(post);
                postMapped.Comments = await GetAllCommentsAsync(postId, userId);
                postMapped.Parents = await GetAllParentsAsync(postId, userId);
                return postMapped;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
