﻿using AutoMapper;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Identity;
using SocialMedia.Domain.Models;

namespace ProEventos.Application.Helpers
{
    public class SocialMediaProfile : Profile
    {
        public SocialMediaProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Post, PostTLDto>().ReverseMap();
            CreateMap<Post, PostDetailsDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<ChatConnection, ConnectionDto>().ReverseMap();
        }
    }
}
