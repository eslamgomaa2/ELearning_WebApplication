using AutoMapper;
using E_learning.Core.Entities.Identity;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Profiles;
using E_Learning.Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories.Profile;
using E_Learning.Core.Repository;
using E_Learning.Repository.Repositories.GenericesRepositories.Profile;
using E_Learning.Service.Contract;
using E_Learning.Service.DTOs.Profiles;
using E_Learning.Service.DTOs.Profiles.Admin;
using E_Learning.Service.DTOs.Profiles.Instructor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public InstructorService(
            IInstructorProfileRepository instructorProfileRepository,
            IGenericRepository<ApplicationUser, Guid> genericRepository,
            IUnitOfWork unit,
            IMapper mapper,
            ResponseHandler responseHandler,
            IFileService fileService,
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole<Guid>>roleManager
            )
        {
            _instructorProfileRepository = instructorProfileRepository;
            _genericRepository = genericRepository;
            _unit = unit;
            _mapper = mapper;
            _responseHandler = responseHandler;
            _fileService = fileService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ================= Create Instructor Profile =================
        // CreateInstructorProfile
        /*  public async Task<Response<InstructorProfileResponseDto>> CreateInstructorProfile(CreateInstructorProfileDto dto, CancellationToken ct = default)
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
                  imagePath = await _fileService.UploadFileAsync<InstructorProfile>(dto.ProfilePicture, "images/Instructors");
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

          */


        // ================= Update Instructor Profile =================
        public async Task<Response<InstructorProfileResponseDto>> UpdateInstructorProfile(Guid userId, UpdateInstructorProfileDto dto)
        {
            var profile = await _instructorProfileRepository.GetInstructorProfileWithUserByUserIdAsync(userId);

            //if (profile == null)
            //    return _responseHandler.NotFound<InstructorProfileResponseDto>("Instructor profile not found");

            if (profile == null)
            {
                profile = new InstructorProfile
                {
                    AppUserId = userId,
                    Location = dto.Location,
                  
                    ProfilePicture = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _instructorProfileRepository.AddAsync(profile);
                await _unit.SaveChangesAsync();
            }
            var user = profile.AppUser;

            if (profile.AppUser == null)
                return _responseHandler.NotFound<InstructorProfileResponseDto>("User not found");

            //// ================= تعديل الباسورد =================
            //if (!string.IsNullOrEmpty(dto.Password))
            //{
            //    if (await _userManager.HasPasswordAsync(user))
            //    {
            //        await _userManager.RemovePasswordAsync(user);
            //    }
            //    await _userManager.AddPasswordAsync(user, dto.Password); 
            //}

            // ================= تعديل الصورة =================

            string imagePath;

            if (dto.ProfilePicture != null)
            {
                if (!string.IsNullOrEmpty(profile.ProfilePicture))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", profile.ProfilePicture.TrimStart('/'));
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                imagePath = await _fileService.UploadFileAsync<InstructorProfile>(dto.ProfilePicture, "images/Instructors");
            }
            else
            {
                imagePath = profile.ProfilePicture;
            }
            //if (!string.IsNullOrEmpty(profile.ProfilePicture))
            //{
            //    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", profile.ProfilePicture.TrimStart('/'));
            //    if (File.Exists(oldPath))
            //        File.Delete(oldPath);
            //}

            //var imagePath = await _fileService.UploadFileAsync<InstructorProfile>(dto.ProfilePicture, "images/Instructors");

            // ================= تعديل باقي البيانات =================
            user.FullName = dto.FullName??user.FullName;
            //user.Email = dto.Email;
            user.PhoneNumber = dto.phoneNumber??user.PhoneNumber;

            profile.Bio = dto.Bio??profile.Bio;
            profile.Location = dto.Location??profile.Location;
            profile.ProfilePicture = imagePath;

            await _unit.SaveChangesAsync();

            // ================= تجهيز الـ Response =================
            var resultDto = _mapper.Map<InstructorProfileResponseDto>(profile);
      
            //resultDto.Password = dto.Password;  
           

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

        public async Task<Response<string>> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return _responseHandler.NotFound<string>("User not found");

            if (dto.NewPassword != dto.ConfirmPassword)
                return _responseHandler.BadRequest<string>("New password and confirmation do not match");

            var passwordValidation = await _userManager.PasswordValidators.First().ValidateAsync(_userManager, user, dto.NewPassword);
            if (!passwordValidation.Succeeded)
            {
                var errors = string.Join(", ", passwordValidation.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<string>($"New password invalid: {errors}");
            }


            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return _responseHandler.BadRequest<string>($"Current password incorrect or change failed: {errors}");
            }


            return _responseHandler.Success("Password changed successfully");
        }
    }
}