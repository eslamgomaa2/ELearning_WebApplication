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
using E_Learning.Service.DTOs.Profiles.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles
{
    public class StudentService : IStudentService
    {
        private readonly IStudentProfileRepository _studentProfileRepository;
        private readonly IGenericRepository<ApplicationUser, Guid> _genericRepository;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public StudentService(
            IStudentProfileRepository studentProfileRepository,
            IGenericRepository<ApplicationUser, Guid> genericRepository,
            IUnitOfWork unit,
            IMapper mapper,
            ResponseHandler responseHandler,
            IFileService fileService,
            UserManager<ApplicationUser>userManager,
            RoleManager<IdentityRole<Guid>> roleManager

            )
        {
            _studentProfileRepository = studentProfileRepository;
            _genericRepository = genericRepository;
            _unit = unit;
            _mapper = mapper;
            _responseHandler = responseHandler;
            _fileService = fileService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ================= Create Student Profile =================
       /* public async Task<Response<StudentProfileResponseDto>> CreateStudentProfile(UpdateStudentProfileDto dto, CancellationToken ct = default)
        {
            // 1️⃣ إنشاء الـ ApplicationUser جديد
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

            // حفظ الـ user أولًا عشان ياخد الـ Id
            await _genericRepository.AddAsync(user);
            await _unit.SaveChangesAsync(ct);

            string imagePath = null;

            if (dto.ProfilePicture != null)
            {
                imagePath = await _fileService.UploadFileAsync<StudentProfile>(dto.ProfilePicture, "images/student");
            }
            var profile = new StudentProfile
            {
                AppUserId = user.Id, // هنا نستخدم الـ Id اللي اتعمل تلقائي
                Location = dto.location,
                DateOfBirth = dto.DateOfBirth,
                ProfilePicture = imagePath
            };

            await _studentProfileRepository.AddAsync(profile);
            await _unit.SaveChangesAsync(ct);

          
            var resultDto = _mapper.Map<StudentProfileResponseDto>(profile);
            return _responseHandler.Created(resultDto);
        }
       */
        // ================= Update Student Profile =================
        public async Task<Response<StudentProfileResponseDto>> UpdateStudentProfile(Guid userId, UpdateStudentProfileDto dto)
        {
            var profile = await _studentProfileRepository.GetStudentProfileWithUserByUserIdAsync(userId);
            if (profile == null)
                return _responseHandler.NotFound<StudentProfileResponseDto>("Student profile not found");
            var user = profile.AppUser;
           

            if (!string.IsNullOrEmpty(dto.Password))
            {
                if (await _userManager.HasPasswordAsync(user))
                {
                   
                    await _userManager.RemovePasswordAsync(user);
                }
           
                await _userManager.AddPasswordAsync(user, dto.Password);
            }


            if (!string.IsNullOrEmpty(profile.ProfilePicture))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", profile.ProfilePicture.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

          
            var newPath = await _fileService.UploadFileAsync<AdminProfile>(dto.ProfilePicture, "images/students");
           
            profile.AppUser.FullName = dto.FullName;
            profile.AppUser.Email = dto.Email;
            profile.AppUser.PhoneNumber = dto.phoneNumber;
            profile.AppUser.MemberSince = DateTime.UtcNow;
            profile.Location = dto.location;
            profile.DateOfBirth = dto.DateOfBirth;
            profile.ProfilePicture = newPath; 


            await _unit.SaveChangesAsync();

            var resultDto = _mapper.Map<StudentProfileResponseDto>(profile);
        
            resultDto.Password = dto.Password;
            return _responseHandler.Success(resultDto);
        }

        // ================= Get Student Profile =================
        public async Task<Response<StudentProfileResponseDto>> GetStudentProfileByUserId(Guid userId)
        {
            var profile = await _studentProfileRepository.GetStudentProfileWithUserByUserIdAsync(userId);
            if (profile == null)
                return _responseHandler.NotFound<StudentProfileResponseDto>("Student profile not found");

            var resultDto = _mapper.Map<StudentProfileResponseDto>(profile);
            return _responseHandler.Success(resultDto);
        }
        public async Task<Response<bool>> StudentProfileExists(Guid userId)
        {
          
            var exists = await _studentProfileRepository.GetStudentProfileWithUserByUserIdAsync(userId) != null;
            return _responseHandler.Success(exists);
        }


        // ================= Get All Students =================
        public async Task<Response<IEnumerable<StudentProfileResponseDto>>> GetAllStudents()
        {
            var profiles = await _studentProfileRepository.GetAllStudentProfilesWithUsersAsync();
            var resultDtos = _mapper.Map<IEnumerable<StudentProfileResponseDto>>(profiles);
            return _responseHandler.Success(resultDtos);
        }

        // ================= Delete Student Profile =================
        public async Task<Response<StudentProfileResponseDto>> DeleteStudentProfile(Guid userId)
        {
            var profile = await _studentProfileRepository.GetStudentProfileWithUserByUserIdAsync(userId);
            if (profile == null)
                return _responseHandler.NotFound<StudentProfileResponseDto>("Student profile not found");

            _studentProfileRepository.Remove(profile);
            await _unit.SaveChangesAsync();

            return _responseHandler.Deleted<StudentProfileResponseDto>("Student profile deleted successfully");
        }

        
    }
}