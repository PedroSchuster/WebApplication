using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Contratos;
using SocialMedia.Domain.Models;
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
            var post = _mapper.Map<Post>(model);
            post.UserId = userId;

            _postPersist.Add<Post>(post);

            if (await _postPersist.SaveChangesAsync())
            {
                var resultado = await _postPersist.GetPostByIdAsync(post.Id, userId); // verificaçao se foi criado msm
                return _mapper.Map<PostDto>(resultado);
            }
            return null;
        }

        public async Task<bool> Remove(int userId, int postId)
        {
            var post = await _postPersist.GetPostByIdAsync(postId, userId);
            if (post == null)  throw new Exception("Post para delete não encontrado. "); ;

            _postPersist.Delete<Post>(post);

            return await _postPersist.SaveChangesAsync();
        }

        public async Task<PostDto> UpdatePost(int userId, int postId, PostDto model)
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

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int userId)
        {
            var posts = await _postPersist.GetAllPostsAsync(userId);
            if (posts == null) return null;

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<PostDto> GetPostByIdAsync(int postId, int userId)
        {
            var post = await _postPersist.GetPostByIdAsync(postId, userId);
            if (post == null) return null;

            return _mapper.Map<PostDto>(post);
        }

    }
}
