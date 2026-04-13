namespace API.Contracts;

public interface ILifetimeDemoService
{
    Guid InstanceId { get; }
    string LifetimeType { get; }
}