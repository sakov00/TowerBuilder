using System.Collections.Generic;
using _Project.Scripts.ServicesGameplay;
using VContainer.Unity;

public class TickScheduler : ITickable
{
    private readonly List<ITickable> _services;
    private int _index;

    public TickScheduler(BlockDropService drop, BlockSwingService swing)
    {
        _services = new()
        {
            drop,
            swing,
        };
    }

    public void Tick()
    {
        if (_services.Count == 0)
            return;

        _services[_index].Tick();

        _index++;
        if (_index >= _services.Count)
            _index = 0;
    }
}