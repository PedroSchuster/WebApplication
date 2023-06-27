using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Contratos;
using SocialMedia.Domain.Identity;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence;
using SocialMedia.Persistence.Contratos;
using SocialMedia.Persistence.Models;

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

                _postPersist.Add<Post>(post);

                if (await _postPersist.SaveChangesAsync())
                {
                    var resultado = await _postPersist.GetPostByIdAsync(post.Id); // verificaçao se foi criado msm
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
                var post = await _postPersist.GetPostByIdAsync(postId);
                if (post == null) throw new Exception("Post para delete não encontrado. ");

                _postPersist.Delete<Post>(post);

                return await _postPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PostDetailsDto> UpdatePost(int userId, int postId, PostDetailsDto model)
        {
            try
            {
                var post = await _postPersist.GetPostByIdAsync(postId);
                if (post == null) return null;

                model.Id = post.Id;
                model.UserId = post.UserId;

                _mapper.Map(model, post);

                _postPersist.Update<Post>(post);

                if (await _postPersist.SaveChangesAsync())
                {
                    var resultado = await _postPersist.GetPostByIdAsync(post.Id); // verificaçao se foi criado msm
                    return _mapper.Map<PostDetailsDto>(resultado);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PostTLDto>> GetAllPostsAsync(string userName, PageParams pageParams)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userName);
                if (user == null) return null;

                var posts = await _postPersist.GetAllPostsAsync(user.Id, pageParams); 
                if (posts == null) return null;

                var result = _mapper.Map<PageList<PostTLDto>>(posts);
                result.ToList().ForEach(x => x.IsLiked = _postPersist.LikePost(user.Id, x.Id.Value).Result == null ? false : true);

                result.CurrentPage = posts.CurrentPage;
                result.TotalCount = posts.TotalCount;
                result.PageSize = posts.PageSize;
                result.TotalPages = posts.TotalPages;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PostTLDto>> GetPostsFollowingPageAsync(int userId, PageParams pageParams)
        {
            try
            {
                var user = await _userPersist.GetUserByIdAsync(userId);
                if (user == null) return null;

                var posts = await _postPersist.GetPostsFollowingPageAsync(userId, pageParams);
                if (posts == null) return null;

                var result = _mapper.Map<PageList<PostTLDto>>(posts);

                result.ToList().ForEach(x => x.IsLiked = _postPersist.LikePost(user.Id, x.Id.Value).Result == null ? false : true);


                result.CurrentPage = posts.CurrentPage;
                result.TotalCount = posts.TotalCount;
                result.PageSize = posts.PageSize;
                result.TotalPages = posts.TotalPages;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PostTLDto>> GetPostsHomePageAsync(int userId, PageParams pageParams)
        {
            try
            {
                var user = await _userPersist.GetUserByIdAsync(userId);
                if (user == null) return null;

                var posts = await _postPersist.GetPostsHomePageAsync(pageParams);
                if (posts == null) return null;


                var result = _mapper.Map<PageList<PostTLDto>>(posts);
                result.ToList().ForEach(x => x.IsLiked = _postPersist.LikePost(user.Id, x.Id.Value).Result == null ? false : true);
                result.CurrentPage = posts.CurrentPage;
                result.TotalCount = posts.TotalCount;
                result.PageSize = posts.PageSize;
                result.TotalPages = posts.TotalPages;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PostDetailsDto>> GetAllParentsAsync(int postId)
        {
            try
            {
                var currentPost = await _postPersist.GetPostByIdAsync(postId);
                int? rootId = currentPost.RootId;
                List<PostDetailsDto> parents = new List<PostDetailsDto>();

                while (rootId != null)
                {
                    currentPost = await _postPersist.GetPostByIdAsync(rootId.Value);
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

        //tirar
        public async Task<IEnumerable<PostDetailsDto>> GetAllCommentsAsync(int postId)
        {
            try
            {
                var comments = await _postPersist.GetAllCommentsByPostIdAsync(postId);
                if (comments == null) return null;

                foreach (var comment in comments)
                {
                    comment.Comment = await _postPersist.GetPostByIdAsync(comment.CommentId);
                }
                List<Post> result = comments.Select(x => x.Comment).OrderByDescending(x=>x.Date).ToList();
                return _mapper.Map<List<PostDetailsDto>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<PostDetailsDto> GetPostByIdAsync(int userId, int postId)
        {
            try
            {
                var post = await _postPersist.GetPostByIdAsync(postId);
                if (post == null) return null;

                var postMapped =  _mapper.Map<PostDetailsDto>(post);
                postMapped.IsLiked = await _postPersist.LikePost(userId, postId) == null? false : true;

                var comments = post.PostComments;
                if (comments != null && comments.Count() > 0)
                {
                    foreach (var comment in comments)
                    {
                        comment.Comment = await _postPersist.GetPostByIdAsync(comment.CommentId);
                    }

                }

                List<Post> result = comments.Select(x => x.Comment).OrderByDescending(x => x.Date).ToList();

                postMapped.Comments = _mapper.Map<List<PostDetailsDto>>(result);
                postMapped.Comments.ToList().ForEach(x => x.IsLiked = _postPersist.LikePost(userId, x.Id.Value).Result == null ? false : true);


                postMapped.TotalComments = result.Count;

                postMapped.Parents = await GetAllParentsAsync(postId);
                postMapped.Parents.ToList().ForEach(x => x.IsLiked = _postPersist.LikePost(userId, x.Id.Value).Result == null ? false : true);

                return postMapped;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> LikePostAsync(int userId, int postId)
        {
            try
            {
                var userLikedPost = await _postPersist.LikePost(userId, postId);
                if (userLikedPost != null) return false;

                var likePost = new UserLikedPost { UserId = userId, PostId = postId };
                _postPersist.Add<UserLikedPost>(likePost);

                var post = await _postPersist.GetPostByIdAsync(postId);
                post.TotalLikes++;
                _postPersist.Update<Post>(post);

                return await _postPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> RemoveLikePostAsync(int userId, int postId)
        {
            try
            {
                var userLikedPost = await _postPersist.LikePost(userId, postId);
                if (userLikedPost == null) return false;

                _postPersist.Delete<UserLikedPost>(userLikedPost);
                var post = await _postPersist.GetPostByIdAsync(postId);
                post.TotalLikes--;
                _postPersist.Update<Post>(post);
                return await _postPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
