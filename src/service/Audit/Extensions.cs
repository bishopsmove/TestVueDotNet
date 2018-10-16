using TestVueDotNet.Contract;

namespace TestVueDotNet.Service
{
    public static partial class Extensions
    {
        public static void TokenCreatedEvent(this TestVueDotNet.Contract.IAuditService audit, string userName, string issuer, int? exp)
        {
            var eventTypeId = TestVueDotNet.Service.AuditServiceEventType.TokenCreated;
            IAuditEventData data = new TestVueDotNet.Service.Model.AuditEventData(eventTypeId, $"Token Create. Issuer: {issuer}. User Name: {userName}. Exp: {exp}");
            audit.Record(data);
        }
    }
}