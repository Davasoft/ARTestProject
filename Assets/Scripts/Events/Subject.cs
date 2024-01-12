using System;

public class Subject 
{
    private event Action _subject = delegate { };

    public void Invoke()
    {
        _subject.Invoke();
    }

    public void AddListener(Action listener)
    {
        _subject += listener;
    }
    public void RemoveListener(Action listener)
    {
        _subject -= listener;
    }
}
public class Subject<T>
{
    private event Action<T> _subject = delegate { };

    public void Invoke(T obj)
    {
        _subject.Invoke(obj);
    }

    public void AddListener(Action<T> listener)
    {
        _subject += listener;
    }
    public void RemoveListener(Action<T> listener)
    {
        _subject -= listener;
    }
}
