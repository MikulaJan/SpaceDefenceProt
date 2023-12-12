namespace Utilities
{
	public static class TimeUtils
	{
		public static float Since(float refTime, float curTime)
		{
			return curTime - refTime;
		}
	}
}
