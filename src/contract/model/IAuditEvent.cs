namespace TestVueDotNet.Contract
{
    public interface IAuditEventData
    {
        int AuditEventTypeId { get; }
        string Message { get; }
    }
}