using AutoMapper;
using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Core.Repository;
using E_Learning.Repository.Repositories.GenericesRepositories.Profile;
using E_Learning.Service.Contract;
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
    public class AdminService : IAdminService
    {
        private readonly IAdminProfileRepository _adminProfileRepository;
        private readonly IGenericRepository<ApplicationUser, Guid> _genericRepository;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;
        private readonly IFileService _fileService;
        private readonly IInstructorProfileRepository _instructorProfileRepository;

        public AdminService(
            IAdminProfileRepository adminProfileRepository,
            IGenericRepository<ApplicationUser, Guid> genericRepository,
            IUnitOfWork unit,
            IMapper mapper,
            ResponseHandler responseHandler,IFileService fileService, IInstructorProfileRepository instructorProfileRepository)
        {
            _adminProfileRepository = adminProfileRepository;
            _genericRepository = genericRepository;
            _unit = unit;
            _mapper = mapper;
            _responseHandler = responseHandler;
            _fileService = fileService;
            _instructorProfileRepository = instructorProfileRepository;
        }

        // ================= Create Admin Profile =================
        public async Task<Response<AdminProfileResponseDto>> CreateAdminProfile(CreateAdminProfileDto dto, CancellationToken ct = default)
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
            string imagePath = null;

            if (dto.ProfilePicture != null)
            {
                imagePath = await _fileService.UploadFileAsync<AdminProfile>(dto.ProfilePicture, "images/admins");
            }

            var profile = new AdminProfile
            {
                AppUserId = user.Id,
                IsSuperAdmin = dto.IsSuperAdmin,
                 ProfilePicture = imagePath
            };

            await _adminProfileRepository.AddAsync(profile);
            await _unit.SaveChangesAsync(ct);

            var resultDto = _mapper.Map<AdminProfileResponseDto>(profile);
            return _responseHandler.Created(resultDto);
        }
        // ================= Create Instructor Profile =================
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

            string imagePath = null;

            if (dto.ProfilePicture != null)
            {
                imagePath = await _fileService.UploadFileAsync<AdminProfile>(dto.ProfilePicture, "images/admins");
            }
            var profile = new InstructorProfile
            {
                AppUserId = user.Id,
                Bio = dto.Bio,
                Location = dto.Location,
                ProfilePicture = imagePath
            };

            await _instructorProfileRepository.AddAsync(profile);
            await _unit.SaveChangesAsync(ct);


            var resultDto = _mapper.Map<InstructorProfileResponseDto>(profile);
            return _responseHandler.Created(resultDto);
        }



        // ================= Update Admin Profile =================
        public async Task<Response<AdminProfileResponseDto>> UpdateAdminProfile(Guid userId, CreateAdminProfileDto dto)
        {
            var profile = await _adminProfileRepository.GetAdminProfileWithUserByUserIdAsync(userId);

            if (profile == null)
                return _responseHandler.NotFound<AdminProfileResponseDto>("Admin profile not found");

            if (!string.IsNullOrEmpty(profile.ProfilePicture))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", profile.ProfilePicture.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }


            var newPath = await _fileService.UploadFileAsync<AdminProfile>(dto.ProfilePicture, "images/admins");
           
            profile.AppUser.FullName = dto.FullName;
            profile.AppUser.Email = dto.Email;
            profile.AppUser.PhoneNumber = dto.phoneNumber;
            profile.ProfilePicture = newPath;
            profile.IsSuperAdmin = dto.IsSuperAdmin;

            await _unit.SaveChangesAsync();

            var resultDto = _mapper.Map<AdminProfileResponseDto>(profile);
            return _responseHandler.Success(resultDto);
        }

        // ================= Get Admin Profile by UserId =================
        public async Task<Response<AdminProfileResponseDto>> GetAdminProfileByUserId(Guid userId)
        {
            var profile = await _adminProfileRepository.GetAdminProfileWithUserByUserIdAsync(userId);
            if (profile == null)
                return _responseHandler.NotFound<AdminProfileResponseDto>("Admin profile not found");

            var resultDto = _mapper.Map<AdminProfileResponseDto>(profile);
            return _responseHandler.Success(resultDto);
        }

        // ================= Check if Admin Profile Exists =================
        public async Task<Response<bool>> AdminProfileExists(Guid userId)
        {
            var exists = await _adminProfileRepository.GetAdminProfileWithUserByUserIdAsync(userId) != null;
            return _responseHandler.Success(exists);
        }

        // ================= Get All Admins =================
        public async Task<Response<IEnumerable<AdminProfileResponseDto>>> GetAllAdmins()
        {
            var profiles = await _adminProfileRepository.GetAllAdminProfilesWithUsersAsync();
            var resultDtos = _mapper.Map<IEnumerable<AdminProfileResponseDto>>(profiles);
            return _responseHandler.Success(resultDtos);
        }

        // ================= Delete Admin Profile =================
        public async Task<Response<AdminProfileResponseDto>> DeleteAdminProfile(Guid userId)
        {
            var profile = await _adminProfileRepository.GetAdminProfileWithUserByUserIdAsync(userId);

            if (profile == null)
                return _responseHandler.NotFound<AdminProfileResponseDto>("Admin profile not found");

            _adminProfileRepository.Remove(profile);
            await _unit.SaveChangesAsync();

            return _responseHandler.Deleted<AdminProfileResponseDto>("Admin profile deleted successfully");
        }

      
    }
}