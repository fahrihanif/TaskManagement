using API.Contracts;

namespace API.Services;

public class SingletonDemoService : ILifetimeDemoService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    public string LifetimeType => "Singleton";
}

public class ScopedDemoService : ILifetimeDemoService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    public string LifetimeType => "Scoped";
}

public class TransientDemoService : ILifetimeDemoService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    public string LifetimeType => "Transient";
}
