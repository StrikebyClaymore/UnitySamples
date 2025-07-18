﻿using System.Collections.Generic;
using Plugins.ServiceLocator;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class ApplicationLoop : MonoBehaviour, IService
    {
        private ApplicationLoop _instance;
        private readonly List<IUpdate> _updates = new List<IUpdate>();
        private readonly List<IFixedUpdate> _fixedUpdates = new List<IFixedUpdate>();
        private readonly List<ILateUpdate> _lateUpdates = new List<ILateUpdate>();

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(this);
        }

        public void AddUpdatable<T>(T obj)
        {
            if (obj is IUpdate update) _updates.Add(update);
            if (obj is IFixedUpdate fixedUpdate) _fixedUpdates.Add(fixedUpdate);
            if (obj is ILateUpdate lateUpdate) _lateUpdates.Add(lateUpdate);
        }

        public void RemoveUpdatable<T>(T obj)
        {
            if (obj is IUpdate update) _updates.Remove(update);
            if (obj is IFixedUpdate fixedUpdate) _fixedUpdates.Remove(fixedUpdate);
            if (obj is ILateUpdate lateUpdate) _lateUpdates.Remove(lateUpdate);
        }

        private void Update()
        {
            for (var i = 0; i < _updates.Count; i++)
            {
                var obj = _updates[i];
                obj.CustomUpdate();
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < _fixedUpdates.Count; i++)
            {
                var obj = _fixedUpdates[i];
                obj.CustomFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            for (var i = 0; i < _lateUpdates.Count; i++)
            {
                var obj = _lateUpdates[i];
                obj.CustomLateUpdate();
            }
        }
    }
}