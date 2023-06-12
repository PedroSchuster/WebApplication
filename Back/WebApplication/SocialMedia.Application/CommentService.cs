using System;
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
    public class CommentService : ICommentService
    {
        private readonly ICommentPersist _commentPersist;
        private readonly IMapper _mapper;

        public CommentService(ICommentPersist commentPersist, IMapper mapper)
        {
            _commentPersist = commentPersist;
            _mapper = mapper;

        }

        public async Task<CommentDto> AddComments(int userId, CommentDto model)
        {
            var comment = _mapper.Map<Comment>(model);
            comment.UserId = userId;

            _commentPersist.Add<Comment>(comment);

            if (await _commentPersist.SaveChangesAsync())
            {
                var resultado = await _commentPersist.GetCommentByIdAsync(comment.Id, comment.PostId); // verificaçao se foi criado msm
                return _mapper.Map<CommentDto>(resultado);
            }
            return null;
        }

        public async Task<bool> Remove(int id, int postId)
        {
            var comment = await _commentPersist.GetCommentByIdAsync(id, postId);
            if (comment == null)  throw new Exception("Post para delete não encontrado. "); ;

            _commentPersist.Delete<Comment>(comment);

            return await _commentPersist.SaveChangesAsync();
        }

        public async Task<CommentDto> UpdateComment(int id, int postId, CommentDto model)
        {
            var comment = await _commentPersist.GetCommentByIdAsync(id, postId);
            if (comment == null) return null;

            model.Id = comment.Id;
            model.UserId = comment.UserId;

            _mapper.Map(model, comment);

            _commentPersist.Update<Comment>(comment);

            if (await _commentPersist.SaveChangesAsync())
            {
                var resultado = await _commentPersist.GetCommentByIdAsync(comment.Id, comment.PostId); // verificaçao se foi criado msm
                return _mapper.Map<CommentDto>(resultado);
            }
            return null;
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync(int postId)
        {
            var comments = await _commentPersist.GetAllCommensByPostIdAsync(postId);
            if (comments == null) return null;

            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto> GetCommentByIdAsync(int id, int postId)
        {
            var comment = await _commentPersist.GetCommentByIdAsync(id, postId);
            if (comment == null) return null;

            return _mapper.Map<CommentDto>(comment);
        }

    }
}
