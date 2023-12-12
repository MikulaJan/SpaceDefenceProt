using UnityEngine;

namespace Utilities
{
	public class Timer
	{
		private float startTime;

		private bool started;

		public Timer()
		{
		}

		public Timer(float time)
		{
			startTime = time;
			started = true;
		}

		public void StartTimer()
		{
			startTime = Time.time;
			started = true;
		}

		public void StartTimer(float time)
		{
			startTime = time;
			started = true;
		}

		public float GetElapsedTime()
		{
			if (started)
			{
				return Mathf.Abs(Time.time - startTime);
			}
			return -1f;
		}

		public bool HasStarted()
		{
			return started;
		}

		public void StopTimer()
		{
			startTime = -1f;
			started = false;
		}
	}
}
