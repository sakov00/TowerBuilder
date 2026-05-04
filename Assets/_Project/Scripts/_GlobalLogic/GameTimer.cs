using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts._GlobalLogic
{
    public class GameTimer : ITickable
    {
        private class TimerData
        {
            public float Interval;
            public float Elapsed;
            public Action Callback;
        }

        private readonly List<TimerData> _timers = new(8);
        
        public void Subscribe(float interval, Action callback)
        {
            if (interval <= 0f)
                throw new ArgumentException("Интервал должен быть больше 0", nameof(interval));
            if (callback == null)
                return;

            _timers.Add(new TimerData
            {
                Interval = interval,
                Elapsed = 0f,
                Callback = callback
            });
        }
        
        public void Unsubscribe(Action callback)
        {
            _timers.RemoveAll(t => t.Callback == callback);
        }

        public void Tick()
        {
            var delta = Time.deltaTime;

            for (int i = 0; i < _timers.Count; i++)
            {
                var t = _timers[i];
                t.Elapsed += delta;
                if (t.Elapsed >= t.Interval)
                {
                    t.Elapsed -= t.Interval;
                    t.Callback?.Invoke();
                }
            }
        }
    }
}