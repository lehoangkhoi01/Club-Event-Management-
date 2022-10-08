﻿using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(EventPost post)
        {
            post.CreatedDate = DateTime.Now;
            post.UpdatedDate = DateTime.Now;
            await _repository.AddNewPost(post);
        }

        public async Task Delete(int postId)
        {
            await _repository.DeletePost(postId);
        }

        public async Task<IEnumerable<EventPost>> GetAllPost()
        {
            return await _repository.GetAllPost();
        }

        public async Task<EventPost> GetPostById(int postId)
        {
            return await _repository.GetPostById(postId);
        }

        public async Task Update(EventPost post)
        {
            await _repository.UpdatePost(post);
        }
    }
}
