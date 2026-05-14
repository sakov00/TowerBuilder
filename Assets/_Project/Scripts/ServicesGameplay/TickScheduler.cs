using System.Collections.Generic;
using _Project.Scripts.ServicesGameplay;
using VContainer;
using VContainer.Unity;

public class TickScheduler : ITickable
{
    private readonly List<ITickable> _services;
    private int _index;

    [Inject]
    public TickScheduler(BlockSwingService swing, TowerSwayService sway, CameraFollowService cameraFollow)
    {
        _services = new()
        {
            swing,
            sway,
            cameraFollow
        };
    }

    public void Tick()
    {
        _services.ForEach(x => x.Tick());
    }
}