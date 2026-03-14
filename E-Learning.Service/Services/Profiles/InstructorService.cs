using AutoMapper;
using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Core.Repository;
using E_Learning.Repository.Repositories.GenericesRepositories.Profile;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Instructor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorProfileRepository _instructorProfileRepository;
        private readonly IGenericRepository<ApplicationUser, Guid> _genericRepository;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;

        public InstructorService(
            IInstructorProfileRepository instructorProfileRepository,
            IGenericRepository<ApplicationUser, Guid> genericRepository,
            IUnitOfWork unit,
            IMapper mapper,
            ResponseHandler responseHandler)
        {
            _instructorProfileRepository = instructorProfileRepository;
            _genericRepository = genericRepository;
            _unit = unit;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        // ================= Create Instructor Profile =================
        // CreateInstructorProfile
        public async Task<Response<InstructorProfileResponseDto>> CreateInstructorProfile(CreateInstructorProfileDto dto, CancellationToken ct = default)
        {
          
            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email, 
                PhoneNumber = dto.phoneNumber,
                IsActive = true,
                MemberSince = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _genericRepository.AddAsync(user);
            await _unit.SaveChangesAsync(ct);

            // 2️⃣ إنشاء InstructorProfile
            var profile = new InstructorProfile
            {
                AppUserId = user.Id,
                Bio = dto.Bio,
                Location = dto.Location
            };

            await _instructorProfileRepository.AddAsync(profile); 
            await _unit.SaveChangesAsync(ct);

           
            var resultDto = _mapper.Map<InstructorProfileResponseDto>(profile);
            return _responseHandler.Created(resultDto);
        }




        // ================= Update Instructor Profile =================
        public async Task<Response<InstructorProfileResponseDto>> UpdateInstructorProfile(Guid userId, CreateInstructorProfileDto dto)
        {
            var profile = await _instructorProfileRepository.GetInstructorProfileWithUserByUserIdAsync(userId);

            if (profile == null)
                return _responseHandler.NotFound<InstructorProfileResponseDto>("Instructor profile not found");

            profile.AppUser.FullName = dto.FullName;
            profile.AppUser.Email = dto.Email;
            profile.AppUser.PhoneNumber = dto.phoneNumber;

            profile.Bio = dto.Bio;
            profile.Location = dto.Location;

            await _unit.SaveChangesAsync();

            var resultDto = _mapper.Map<InstructorProfileResponseDto>(profile);
            return _responseHandler.Success(resultDto);
        }

        // ================= Get Instructor Profile =================
        public async Task<Response<InstructorProfileResponseDto>> GetInstructorProfileByUserId(Guid userId)
        {
            var profile = await _instructorProfileRepository.GetInstructorProfileWithUserByUserIdAsync(userId);

            if (profile == null)
                return _responseHandler.NotFound<InstructorProfileResponseDto>("Instructor profile not found");

            var resultDto = _mapper.Map<InstructorProfileResponseDto>(profile);
            return _responseHandler.Success(resultDto);
        }

        // ================= Check Instructor Profile Exists =================
        public async Task<Response<bool>> InstructorProfileExists(Guid userId)
        {
            var exists = await _instructorProfileRepository.GetInstructorProfileWithUserByUserIdAsync(userId) != null;
            return _responseHandler.Success(exists);
        }

        // ================= Get All Instructors =================
        public async Task<Response<IEnumerable<InstructorProfileResponseDto>>> GetAllInstructors()
        {
            var profiles = await _instructorProfileRepository.GetAllInstructorsWithUserAsync();
            var resultDtos = _mapper.Map<IEnumerable<InstructorProfileResponseDto>>(profiles);

            return _responseHandler.Success(resultDtos);
        }

        // ================= Delete Instructor Profile =================
        public async Task<Response<InstructorProfileResponseDto>> DeleteInstructorProfile(Guid userId)
        {
            var profile = await _instructorProfileRepository.GetInstructorProfileWithUserByUserIdAsync(userId);

            if (profile == null)
                return _responseHandler.NotFound<InstructorProfileResponseDto>("Instructor profile not found");

            _instructorProfileRepository.Remove(profile);
            await _unit.SaveChangesAsync();

            return _responseHandler.Deleted<InstructorProfileResponseDto>("Instructor profile deleted successfully");
        }

        // ================= Upload Profile Picture =================
        public async Task<Response<string>> UploadProfilePicture(Guid userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return _responseHandler.BadRequest<string>("No file uploaded");

            var profile = await _instructorProfileRepository.GetInstructorProfileWithUserByUserIdAsync(userId);

            if (profile == null)
                return _responseHandler.NotFound<string>("Instructor profile not found");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/instructors");
            Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{userId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(profile.ProfilePicture))
            {
                var oldPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    profile.ProfilePicture.TrimStart('/')
                );

                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            profile.ProfilePicture = $"/images/instructors/{fileName}";
            await _unit.SaveChangesAsync();

            return _responseHandler.Success(profile.ProfilePicture);
        }
    }
}