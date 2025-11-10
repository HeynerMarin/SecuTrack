using AutoMapper;
using SecuTrack.Application.DTOs;
using SecuTrack.Core.Entities;
using SecuTrack.Core.Enums;

namespace SecuTrack.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Provider mappings
            CreateMap<Provider, ProviderDto>()
                .ForMember(dest => dest.TypeDisplay, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.TotalAudits, opt => opt.MapFrom(src => src.AuditSessions.Count))
                .ForMember(dest => dest.LastAuditDate, opt => opt.MapFrom(src =>
                    src.AuditSessions.OrderByDescending(a => a.AuditDate).FirstOrDefault() != null
                    ? src.AuditSessions.OrderByDescending(a => a.AuditDate).First().AuditDate
                    : (DateTime?)null))
                .ForMember(dest => dest.LastComplianceScore, opt => opt.MapFrom(src =>
                    src.AuditSessions.OrderByDescending(a => a.AuditDate).FirstOrDefault() != null
                    && src.AuditSessions.OrderByDescending(a => a.AuditDate).First().ComplianceScore != null
                    ? src.AuditSessions.OrderByDescending(a => a.AuditDate).First().ComplianceScore!.GlobalScore
                    : (decimal?)null));

            CreateMap<CreateProviderDto, Provider>();

            // Audit mappings
            CreateMap<AuditSession, AuditDto>()
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => src.Provider.Name))
                .ForMember(dest => dest.GlobalScore, opt => opt.MapFrom(src =>
                    src.ComplianceScore != null ? src.ComplianceScore.GlobalScore : (decimal?)null))
                .ForMember(dest => dest.TotalEvaluations, opt => opt.MapFrom(src => src.Evaluations.Count))
                .ForMember(dest => dest.CriticalGaps, opt => opt.MapFrom(src =>
                    src.ComplianceScore != null ? src.ComplianceScore.CriticalGaps : 0));

            CreateMap<CreateAuditDto, AuditSession>();

            CreateMap<AuditSession, AuditDetailDto>()
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => src.Provider.Name));

            // Evaluation mappings
            CreateMap<Evaluation, EvaluationDto>()
                .ForMember(dest => dest.ControlId, opt => opt.MapFrom(src => src.ISOControl.ControlId))
                .ForMember(dest => dest.ControlTitle, opt => opt.MapFrom(src => src.ISOControl.Title))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ISOControl.Category))
                .ForMember(dest => dest.ComplianceLevelDisplay, opt => opt.MapFrom(src => GetComplianceLevelDisplay(src.ComplianceLevel)))
                .ForMember(dest => dest.IsCritical, opt => opt.MapFrom(src => src.ISOControl.IsCritical));

            CreateMap<CreateEvaluationDto, Evaluation>();

            // ComplianceScore mappings
            CreateMap<ComplianceScore, ComplianceScoreDto>()
                .ForMember(dest => dest.ComplianceLevel, opt => opt.MapFrom(src => GetComplianceLevelText(src.GlobalScore)));

            // ISOControl mappings
            CreateMap<ISOControl, ISOControlDto>()
                .ForMember(dest => dest.CategoryDisplay, opt => opt.MapFrom(src => GetCategoryDisplay(src.Category)));
        }

        private static string GetComplianceLevelDisplay(ComplianceLevel level)
        {
            return level switch
            {
                ComplianceLevel.NotImplemented => "No Implementado (0%)",
                ComplianceLevel.PartiallyImplemented => "Parcialmente Implementado (25%)",
                ComplianceLevel.LargelyImplemented => "Mayormente Implementado (50%)",
                ComplianceLevel.FullyImplemented => "Totalmente Implementado (75%)",
                ComplianceLevel.ExceedsRequirements => "Supera Requisitos (100%)",
                _ => "No Evaluado"
            };
        }

        private static string GetComplianceLevelText(decimal score)
        {
            return score switch
            {
                >= 90 => "Excelente",
                >= 75 => "Bueno",
                >= 60 => "Aceptable",
                >= 40 => "Necesita Mejora",
                _ => "Crítico"
            };
        }

        private static string GetCategoryDisplay(ISOCategory category)
        {
            return category switch
            {
                ISOCategory.OrganizationalControls => "Controles Organizacionales",
                ISOCategory.PeopleControls => "Controles de Personas",
                ISOCategory.PhysicalControls => "Controles Físicos",
                ISOCategory.TechnologicalControls => "Controles Tecnológicos",
                _ => "Desconocido"
            };
        }
    }
}