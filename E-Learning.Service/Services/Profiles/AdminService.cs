using AutoMapper;
using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Profiles.Admin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public AdminService(
            IAdminProfileRepository adminProfileRepository,
            IGenericRepository<ApplicationUser, Guid> genericRepository,
            IUnitOfWork unit,
            IMapper mapper,
            ResponseHandler responseHandler)
        {
            _adminProfileRepository = adminProfileRepository;
            _genericRepository = genericRepository;
            _unit = unit;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }


        // ================= Create Admin Profile =================
        public async Task<Response<AdminProfileResponseDto>> CreateAdminProfile(Guid userId, CreateAdminProfileDTo dto, CancellationToken ct = default)
        {
            var user = await _genericRepository.GetByIdAsync(userId, ct);

            if (user == null)
                return _responseHandler.NotFound<AdminProfileResponseDto>("User not found");

            
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.phoneNumber;

       
            var profile = new AdminProfile
            {
                AppUserId = userId,
                IsSuperAdmin = dto.IsSuperAdmin
            };

           
            await _adminProfileRepository.AddAsync(profile);
            await _unit.SaveChangesAsync(ct);

            var resultDto = _mapper.Map<AdminProfileResponseDto>(profile);
            return _responseHandler.Created(resultDto);
        }

        // ================= Update Admin Profile =================
        public async Task<Response<AdminProfileResponseDto>> UpdateAdminProfile(Guid userId, CreateAdminProfileDTo dto)
        {
            var profile = await _adminProfileRepository.GetAdminProfileWithUserByUserIdAsync(userId);

            if (profile == null)
                return _responseHandler.NotFound<AdminProfileResponseDto>("Admin profile not found");

          
            profile.AppUser.FullName = dto.FullName;
            profile.AppUser.Email = dto.Email;
            profile.AppUser.PhoneNumber = dto.phoneNumber;

          
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

            return _responseHandler.Deleted<AdminProfileResponseDto>( "Admin profile deleted successfully");
        }
        public async Task<Response<string>> UploadProfilePicture(Guid userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return _responseHandler.BadRequest<string>("No file uploaded");

            var profile = await _adminProfileRepository.GetAdminProfileWithUserByUserIdAsync(userId);
            if (profile == null)
                return _responseHandler.NotFound<string>("Admin profile not found");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/admins");
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
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", profile.ProfilePicture.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }
            profile.ProfilePicture = $"/images/admins/{fileName}";
            await _unit.SaveChangesAsync();

            return _responseHandler.Success(profile.ProfilePicture);
        }
    }
}