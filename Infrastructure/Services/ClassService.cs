﻿using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications;
using System.Net.WebSockets;

namespace Infrastructure.Services
{
    public class ClassService : IClassService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IUnitOfWork _unitOfWork;

        // ==============================
        // === Constructors
        // ==============================

        public ClassService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==============================
        // === Methods
        // ==============================

        public Task CreateClassAsync(Classroom classroom)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClassAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Classroom?> GetClassAsync(ISpecification<Classroom> spec)
        {
            return await _unitOfWork.Repository<Classroom>().GetEntityWithSpec(spec);
        }

        public Task<Course> GetClassByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Classroom>> GetClassesAsync(ISpecification<Classroom> spec)
        {
            var classes = await _unitOfWork.Repository<Classroom>().ListAsync(spec);
            return classes;
        }

        public Task UpdateClassAsync(Classroom classroom)
        {
            throw new NotImplementedException();
        }
    }
}
