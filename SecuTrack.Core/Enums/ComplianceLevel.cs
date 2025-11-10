namespace SecuTrack.Core.Enums
{
    public enum ComplianceLevel
    {
        NotImplemented = 0,      // 0% - No implementado
        PartiallyImplemented = 25,  // 25% - Parcialmente implementado
        LargelyImplemented = 50,    // 50% - Mayormente implementado
        FullyImplemented = 75,      // 75% - Totalmente implementado
        ExceedsRequirements = 100   // 100% - Supera requisitos
    }
}