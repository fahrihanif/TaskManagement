using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LifetimeController : ControllerBase
{
    private readonly ScopedDemoService _scoped;
    private readonly ScopedDemoService _scoped2;
    private readonly SingletonDemoService _singleton;
    private readonly SingletonDemoService _singleton2;
    private readonly TransientDemoService _transient;
    private readonly TransientDemoService _transient2;

    public LifetimeController(
        SingletonDemoService singleton,
        ScopedDemoService scoped,
        TransientDemoService transient,
        SingletonDemoService singleton2,
        ScopedDemoService scoped2,
        TransientDemoService transient2)
    {
        _singleton = singleton;
        _scoped = scoped;
        _transient = transient;
        _singleton2 = singleton2;
        _scoped2 = scoped2;
        _transient2 = transient2;
    }

    [HttpGet]
    public IActionResult Get()
        => Ok(new
        {
            singleton = new
            {
                _singleton.LifetimeType, 
                instanceId = _singleton.InstanceId, 
                instanceId2 = _singleton2.InstanceId
            },
            scoped = new
            {
                _scoped.LifetimeType, 
                instanceId = _scoped.InstanceId, 
                instanceId2 = _scoped2.InstanceId
            },
            transient = new
            {
                _transient.LifetimeType, 
                instanceId = _transient.InstanceId, 
                instanceId2 = _transient2.InstanceId
            }
        });
}