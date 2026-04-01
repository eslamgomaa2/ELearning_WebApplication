using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Enums;
using E_Learning.Core.Interfaces.Services.Courses;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.LiveSessionDto;
using E_Learning.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using E_Learning.Service.Services.Profiles.InstructorSetting;

namespace E_Learning.Service.Services.LiveSessionServices
{
    public class LiveSessionService : ILiveSessionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;
        private readonly IInstructorService _instructorService;
        private readonly IHubContext<LiveSessionHub> _hubContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public LiveSessionService(
        IUnitOfWork uow,
        ResponseHandler responseHandler,
        IMapper mapper,
        ICourseService courseService,
        IHubContext<LiveSessionHub> hubContext,
        IHttpClientFactory httpClientFactory)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _courseService = courseService;
            _hubContext = hubContext;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Response<IReadOnlyList<LiveSessionResponseDto>>> GetAllAsync(string? search, string? status, CancellationToken ct = default)
        {
            var query = _uow.LiveSessions.GetTableNoTracking()
                            .Include(x => x.Course)
                            .Include(x => x.Attendees)
                            .Include(x => x.Instructor)
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Title.Contains(search));

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<LiveSessionStatus>(status, true, out var statusEnum))
                query = query.Where(x => x.Status == statusEnum);

            var sessions = await query.ToListAsync(ct);

            if (sessions == null || !sessions.Any())
                return _responseHandler.NotFound<IReadOnlyList<LiveSessionResponseDto>>("No live sessions found.");

            var mappedData = _mapper.Map<List<LiveSessionResponseDto>>(sessions);

            return _responseHandler.Success((IReadOnlyList<LiveSessionResponseDto>)mappedData);
        }

        public async Task<Response<LiveSessionResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await _uow.LiveSessions.GetTableNoTracking()
                .Include(x => x.Course)
                .Include(x => x.Attendees)
                .Include(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (session is null)
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Live Session with ID {id} not found.");

            // المابر سيتعرف على InstructorProfile ويضعه في الـ DTO مباشرة
            var dto = _mapper.Map<LiveSessionResponseDto>(session);

            return _responseHandler.Success(dto);
        }
        public async Task<Response<LiveSessionResponseDto>> CreateAsync(CreateLiveSessionDto dto, CancellationToken ct)
        {
            // 1. التحقق من وجود الكورس
            var courseResponse = await _courseService.GetCourseByIdAsync(dto.CourseId, ct);
            if (courseResponse.Data == null)
                return _responseHandler.NotFound<LiveSessionResponseDto>("Course not found.");

            // 2. مابينج وحفظ الجلسة في الداتابيز (بدون رابط زوم)
            var session = _mapper.Map<LiveSession>(dto);
            await _uow.LiveSessions.AddAsync(session, ct);
            await _uow.SaveChangesAsync(ct);

            // متغيرات لحفظ الروابط اللي رح تيجي من زوم
            string zoomStartUrl = string.Empty;
            string zoomJoinUrl = string.Empty;

            // 3. 🚀 نداء زوم (بنجيب اللينكات بس ما بنحفظها بالداتابيز)
            try
            {
                var (startUrl, joinUrl) = await CreateZoomMeetingAsync(session, ct);
                zoomStartUrl = startUrl;
                zoomJoinUrl = joinUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Zoom Error: {ex.Message}");
            }

            // 4. جلب البيانات من الداتابيز وتحويلها لـ DTO
            var result = await GetByIdAsync(session.Id, ct);

            // 5. ✨ السطر السحري: تخزين الرابط في الـ DTO فقط
            if (result.Data != null)
            {
                // بنحط الرابط في الحقل الموجود بالـ DTO (تأكدي إن اسمه RecordingUrl أو ZoomUrl)
                result.Data.ZoomUrl = zoomJoinUrl;

                // إذا بدك ترجعي StartUrl للمدرس كمان (لازم يكون الحقل موجود بالـ DTO)
                // result.Data.StartUrl = zoomStartUrl; 
            }

            // 6. إرسال التحديث عبر SignalR (الـ DTO الآن يحتوي على اللينك)
            await _hubContext.Clients.All.SendAsync("OnSessionCreated", result.Data);

            return _responseHandler.Created(result.Data);
        }
        // --- 2️⃣ ميثود جلب التوكن (خاصة بالـ Service) ---
        private async Task<string> GetZoomAccessTokenAsync(HttpClient client, CancellationToken ct)
        {
            string clientId = "3h47Ymv_QyOA37FXEeAhZw";
            string clientSecret = "4J380Ok1uQ9N4KwV610CV7sP2S7K4VFy";
            string accountId = "OYNW2okMTDW4Xn1YakVoxQ";

            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            // يفضل دائماً استخدام الـ URL الكامل في الـ Post
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var response = await client.SendAsync(request, ct);

            // 1️⃣ نقرأ الرد كنص أولاً عشان لو في خطأ نعرفه
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                // 🚨 هذا السطر رح يطبع لك في الـ Console السبب الحقيقي (مثلاً: invalid_client)
                Console.WriteLine($"🚨 ZOOM OAUTH ERROR: {response.StatusCode} - {responseBody}");
                throw new Exception($"Zoom Auth Failed: {responseBody}");
            }

            // 2️⃣ إذا نجح الطلب، نحول النص لـ JsonElement ونأخذ التوكن
            var data = System.Text.Json.JsonDocument.Parse(responseBody).RootElement;

            if (data.TryGetProperty("access_token", out var tokenElement))
            {
                return tokenElement.GetString() ?? "";
            }

            throw new Exception("Access token not found in Zoom response.");
        }


        // --- 3️⃣ ميثود إنشاء الميتنج ---
        public async Task<(string startUrl, string joinUrl)> CreateZoomMeetingAsync(LiveSession session, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient();
            var accessToken = await GetZoomAccessTokenAsync(client, ct);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/v2/users/me/meetings");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            request.Content = JsonContent.Create(new
            {
                topic = session.Title,
                type = 2,
                start_time = session.ScheduledAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                duration = session.DurationMinutes > 0 ? session.DurationMinutes : 60,
                settings = new
                {
                    join_before_host = false,
                    waiting_room = true
                }
            });

            var response = await client.SendAsync(request, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct); // 👈 نقرأ الرد كنص أولاً

            if (!response.IsSuccessStatusCode)
            {
                // 👈 اطبعي هذا السطر وشوفي شو مكتوب في الـ Console تحت
                Console.WriteLine($"ZOOM API ERROR: {response.StatusCode} - {responseBody}");
                return ("", "");
            }

            // 👈 الآن نحول النص لـ JsonElement عشان نسحب الروابط
            var data = System.Text.Json.JsonDocument.Parse(responseBody).RootElement;

            return (
                data.GetProperty("start_url").GetString() ?? "",
                data.GetProperty("join_url").GetString() ?? ""
            );
        }


        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.UpdateAsync(int id, UpdateLiveSessionDto dto, CancellationToken ct)
        {
            var session = await _uow.LiveSessions.GetByIdAsync(id, ct);
            if (session is null)
                return _responseHandler.NotFound<LiveSessionResponseDto>($"Live Session {id} not found.");

            var courseResponse = await _courseService.GetCourseByIdAsync(dto.CourseId, ct);
            if (courseResponse.Data == null)
                return _responseHandler.NotFound<LiveSessionResponseDto>("Course not found.");

            _mapper.Map(dto, session);
            _uow.LiveSessions.Update(session);
            await _uow.SaveChangesAsync(ct);

            var result = await GetByIdAsync(id, ct);

            await _hubContext.Clients.All.SendAsync("OnSessionUpdated", result.Data);
            return _responseHandler.Success(result.Data);
        }

        async Task<Response<LiveSessionResponseDto>> ILiveSessionService.DeleteAsync(int id, CancellationToken ct)
        {
            var result = await GetByIdAsync(id, ct);
            if (result.Data == null)
                return _responseHandler.NotFound<LiveSessionResponseDto>("Session not found.");

            var entityToDelete = await _uow.LiveSessions.GetByIdAsync(id, ct);
            _uow.LiveSessions.SoftDelete(entityToDelete);
            await _uow.SaveChangesAsync(ct);

            await _hubContext.Clients.All.SendAsync("OnSessionDeleted", id);

            return _responseHandler.Success(result.Data);
        }
    }
}