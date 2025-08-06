using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;

public abstract class Subject : Singleton<Subject>
{
    private List<IObserver> _subscribers = new List<IObserver>();

    public void Subscribe(IObserver observer)
    {
        _subscribers.Add(observer);
    }

    public void Subscribe(IObserver[] observers)
    {
        _subscribers.AddRange(observers);
    }

    public void Unsubscribe(IObserver observer)
    {
        _subscribers.Remove(observer);
    }

    public void Notify(EventsEnum evt)
    {
        _subscribers.ForEach((_observer) => {_observer.OnNotify(evt);});
    }
}