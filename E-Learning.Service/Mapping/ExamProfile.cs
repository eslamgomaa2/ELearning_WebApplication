using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Learning.Core.Entities.Assessments.Exams;
using E_Learning.Service.DTOs.ExamsDtos;

namespace E_Learning.Service.Mapping
{
    public class ExamProfile:Profile
    {
      public  ExamProfile() {
            CreateMap<Exam, CreateExamDto>();

          
            CreateMap<UpdateExamDto, Exam>();

           
            CreateMap<Exam, ExamDto>();
            CreateMap<CreateExamDto, Exam>()
            .ForMember(dest => dest.SourceFileUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Questions, opt => opt.Ignore())
            .ForMember(dest => dest.Attempts, opt => opt.Ignore());


        }
    }
}
