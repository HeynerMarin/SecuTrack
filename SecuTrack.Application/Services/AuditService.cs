using AutoMapper;
using SecuTrack.Application.DTOs;
using SecuTrack.Core.Entities;
using SecuTrack.Core.Enums;
using SecuTrack.Core.Interfaces;

namespace SecuTrack.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuditDto>> GetAllAuditsAsync()
        {
            var audits = await _unitOfWork.Audits.GetCompletedAuditsAsync();
            return _mapper.Map<IEnumerable<AuditDto>>(audits);
        }

        public async Task<IEnumerable<AuditDto>> GetAuditsByProviderAsync(int providerId)
        {
            var audits = await _unitOfWork.Audits.GetAuditsByProviderAsync(providerId);
            return _mapper.Map<IEnumerable<AuditDto>>(audits);
        }

        public async Task<AuditDetailDto?> GetAuditDetailAsync(int auditId)
        {
            var audit = await _unitOfWork.Audits.GetAuditWithDetailsAsync(auditId);
            return audit != null ? _mapper.Map<AuditDetailDto>(audit) : null;
        }

        public async Task<AuditDto> CreateAuditAsync(CreateAuditDto dto)
        {
            var audit = _mapper.Map<AuditSession>(dto);
            await _unitOfWork.Audits.AddAsync(audit);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AuditDto>(audit);
        }

        public async Task<bool> CompleteAuditAsync(int auditId)
        {
            var audit = await _unitOfWork.Audits.GetByIdAsync(auditId);
            if (audit == null) return false;

            audit.IsCompleted = true;
            audit.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Audits.Update(audit);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<ComplianceScoreDto?> CalculateComplianceScoreAsync(int auditId)
        {
            var evaluations = await _unitOfWork.Evaluations.GetEvaluationsByAuditAsync(auditId);
            var evaluationsList = evaluations.ToList();

            if (!evaluationsList.Any()) return null;

            var orgControls = evaluationsList.Where(e => e.ISOControl.Category == ISOCategory.OrganizationalControls).ToList();
            var peopleControls = evaluationsList.Where(e => e.ISOControl.Category == ISOCategory.PeopleControls).ToList();
            var physicalControls = evaluationsList.Where(e => e.ISOControl.Category == ISOCategory.PhysicalControls).ToList();
            var techControls = evaluationsList.Where(e => e.ISOControl.Category == ISOCategory.TechnologicalControls).ToList();

            var complianceScore = new ComplianceScore
            {
                AuditSessionId = auditId,
                GlobalScore = CalculateCategoryScore(evaluationsList),
                OrganizationalScore = CalculateCategoryScore(orgControls),
                PeopleScore = CalculateCategoryScore(peopleControls),
                PhysicalScore = CalculateCategoryScore(physicalControls),
                TechnologicalScore = CalculateCategoryScore(techControls),
                TotalControls = evaluationsList.Count,
                EvaluatedControls = evaluationsList.Count,
                CriticalGaps = evaluationsList.Count(e => e.ISOControl.IsCritical && (int)e.ComplianceLevel < 50)
            };

            // Verificar si ya existe un score para esta auditoría
            var existingScore = await _unitOfWork.Repository<ComplianceScore>()
                .FirstOrDefaultAsync(cs => cs.AuditSessionId == auditId);

            if (existingScore != null)
            {
                existingScore.GlobalScore = complianceScore.GlobalScore;
                existingScore.OrganizationalScore = complianceScore.OrganizationalScore;
                existingScore.PeopleScore = complianceScore.PeopleScore;
                existingScore.PhysicalScore = complianceScore.PhysicalScore;
                existingScore.TechnologicalScore = complianceScore.TechnologicalScore;
                existingScore.TotalControls = complianceScore.TotalControls;
                existingScore.EvaluatedControls = complianceScore.EvaluatedControls;
                existingScore.CriticalGaps = complianceScore.CriticalGaps;
                existingScore.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Repository<ComplianceScore>().Update(existingScore);
            }
            else
            {
                await _unitOfWork.Repository<ComplianceScore>().AddAsync(complianceScore);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ComplianceScoreDto>(complianceScore);
        }

        private decimal CalculateCategoryScore(List<Evaluation> evaluations)
        {
            if (!evaluations.Any()) return 0;

            var totalWeight = evaluations.Sum(e => e.ISOControl.Weight);
            var weightedScore = evaluations.Sum(e => (int)e.ComplianceLevel * e.ISOControl.Weight / 100m);

            return Math.Round(weightedScore / totalWeight * 100, 2);
        }
    }
}