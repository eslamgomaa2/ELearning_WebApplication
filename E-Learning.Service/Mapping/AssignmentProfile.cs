using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Entities.Assessments.Assignments;
using E_Learning.Core.Enums;
using E_Learning.Service.DTOs.AssignmentsDto;

namespace E_Learning.Service.Mapping
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<CreateAssignmentDto, Assignment>();

            CreateMap<UpdateAssignmentDto, Assignment>();

            CreateMap<Assignment, AssignmentDto>();
            CreateMap<CreateSubmissionDto,AssignmentSubmission>();

     CreateMap<AssignmentSubmission, AssignmentSubmissionDto>()
             .ForMember(dest => dest.AssignmentTitle,
                 opt => opt.MapFrom(src => src.Assignment.Title))
             .ForMember(dest => dest.StudentName,
                 opt => opt.MapFrom(src => src.Student.FullName))
             .ForMember(dest => dest.Status,
                 opt => opt.MapFrom(src => src.Status.ToString()))
             .ReverseMap()
             .ForMember(dest => dest.Assignment, opt => opt.Ignore())  
             .ForMember(dest => dest.Student, opt => opt.Ignore())
             .ForMember(dest => dest.Status,
                 opt => opt.MapFrom(src => Enum.Parse<AssignmentStatus>(src.Status)));
                }
    }
}
