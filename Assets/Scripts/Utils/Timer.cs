using UnityEngine;
using UnityEngine.Events;

public class Timer
{
	#region Fields

	// timer duration
	float totalSeconds = 0;

	// timer execution
	float startTime = 0;
	bool running = false;

	bool available = false;

	UnityEvent timerFinishedEvent = new UnityEvent();

	static UnityEvent timerUpdateEvent = new UnityEvent();

	#endregion

	#region Properties

	public float Duration
    {
		set
        {
			if (!running)
            {
				totalSeconds = value;
			}
		}
	}

	public bool Running
    {
		get { return running; }
	}
	public bool Available
	{
		get { return available; }
		set 
		{
			available = value;
			timerFinishedEvent.RemoveAllListeners();
			startTime = float.MaxValue;
			totalSeconds = 0;
			running = false;
		}
	}

	#endregion

	#region Public methods

	public Timer()
    {
		timerUpdateEvent.AddListener(Update);
    }

	public void Update()
	{
		if (!Available)
			return;
		if (running)
		{
			float elapsedSeconds = Time.realtimeSinceStartup - startTime;
			if (elapsedSeconds >= totalSeconds)
            {
				running = false;
				timerFinishedEvent.Invoke();
			}
		}
	}

	public void Run()
    {
        if (!Available)
        {
			Debug.LogError("Timer runned and not ready");
			return;
        }
		if (totalSeconds > 0)
		{
			running = true;
			startTime = Time.realtimeSinceStartup;
		}
	}

	public void Stop()
    {
		running = false;
	}

	public void AddTimerFinishedEventListener(UnityAction handler)
    {
		timerFinishedEvent.AddListener(handler);
	}

	public void SetDispose(UnityAction disposeAction)
    {
		timerFinishedEvent.AddListener(disposeAction);
	}

	public void Refresh()
	{
		startTime = Time.realtimeSinceStartup;
	}

	public static void UpdateTimers()
    {
		timerUpdateEvent.Invoke();
    }

	#endregion
}
