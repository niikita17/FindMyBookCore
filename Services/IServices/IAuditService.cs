namespace MyBook_Backend.Services.IServices
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
