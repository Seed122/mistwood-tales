using System.Collections.Generic;
using System.Linq;
using MistwoodTales.Game.Client.Entities;
using MistwoodTales.Game.Client.Scheduling;

namespace MistwoodTales.Game.Client.Systems
{
    class SchedulingSystem
    {
        private volatile int _time;
        private readonly SortedDictionary<int, List<IScheduleable>> _scheduleables;

        public SchedulingSystem()
        {
            _time = 0;
            _scheduleables = new SortedDictionary<int, List<IScheduleable>>();
        }

        // Add a new object to the schedule 
        // Place it at the current time plus the object's Time property.
        public void Add(IScheduleable scheduleable)
        {
            int key = _time + scheduleable.Time;
            if (!_scheduleables.ContainsKey(key))
            {
                //_scheduleables.Add(key, new List<IScheduleable>());
                _scheduleables.Add(key, new List<IScheduleable>());
            }
            lock (_scheduleables[key])
            {
                _scheduleables[key].Add(scheduleable);
            }
        }
        // Add a new object to the schedule 
        // Place it at the current time plus the object's Time property.
        public void Add(IEnumerable<IScheduleable> scheduleableToAdd)
        {
            foreach (var scheduleable in scheduleableToAdd)
            {
                Add(scheduleable);
            }
        }

        // Remove a specific object from the schedule.
        // Useful for when an monster is killed to remove it before it's action comes up again.
        public void Remove(IScheduleable scheduleable)
        {
            foreach (var scheduleablesList in _scheduleables)
            {
                if (scheduleablesList.Value.Contains(scheduleable))
                {
                    var scheduleableListFound = scheduleablesList;
                    scheduleableListFound.Value.Remove(scheduleable);
                    if (scheduleableListFound.Value.Count <= 0)
                    {
                        _scheduleables.Remove(scheduleableListFound.Key);
                    }
                    break;
                }
            }
        }
        // Remove a specific object from the schedule.
        // Useful for when an monster is killed to remove it before it's action comes up again.
        public void Remove(IEnumerable<IScheduleable> scheduleableToRemove)
        {
            foreach (var scheduleable in scheduleableToRemove)
            foreach (var scheduleablesList in _scheduleables)
            {
                if (!scheduleablesList.Value.Contains(scheduleable))
                        continue;
                var scheduleableListFound = scheduleablesList;
                scheduleableListFound.Value.Remove(scheduleable);
                if (scheduleableListFound.Value.Count <= 0)
                {
                    _scheduleables.Remove(scheduleableListFound.Key);
                }
                break;
            }
        }

        // Get the next object whose turn it is from the schedule. Advance time if necessary
        public IList<IScheduleable> Get()
        {
            if(!_scheduleables.Any())
                return new List<IScheduleable>();
            var firstScheduleableGroup = _scheduleables.First();
            var scheduleables = firstScheduleableGroup.Value.ToList();
            Remove(scheduleables);
            _time = firstScheduleableGroup.Key;
            return scheduleables;
        }

        // Get the current time (turn) for the schedule
        public int GetTime()
        {
            return _time;
        }

        // Reset the time and clear out the schedule
        public void Clear()
        {
            _time = 0;
            _scheduleables.Clear();
        }

        public void RunActions()
        {
            var scheduleables = Get();
            foreach (var scheduleable in scheduleables)
            {
                var actor = scheduleable as Actor;
                actor?.PerformAction(Game.CommandSystem);
            }
            Add(scheduleables);
        }
    }
}
