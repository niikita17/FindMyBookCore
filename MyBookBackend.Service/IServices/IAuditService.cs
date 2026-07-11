namespace MyBookBackend.Service.IServices
{
    public interface IAuditService
    {
        Task Log(
      int userId,
      string action,
      string entityName,
      int entityId,
      string description);
    }
}
