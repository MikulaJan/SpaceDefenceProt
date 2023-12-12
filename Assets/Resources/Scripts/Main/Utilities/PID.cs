using UnityEngine;

namespace Utilities
{
	public class PID
	{
		public float pFactor;

		public float iFactor;

		public float dFactor;

		private float integral;

		private float lastError;

		public PID()
		{
			pFactor = 0f;
			iFactor = 0f;
			dFactor = 0f;
		}

		public PID(PidValues values)
		{
			pFactor = values.P;
			iFactor = values.I;
			dFactor = values.D;
		}

		public PID(Vector3 values)
		{
			pFactor = values.x;
			iFactor = values.y;
			dFactor = values.z;
		}

		public PID(float pFactor, float iFactor, float dFactor)
		{
			this.pFactor = pFactor;
			this.iFactor = iFactor;
			this.dFactor = dFactor;
		}

		public float Update(float setpoint, float actual, float timeFrame)
		{
			float num = setpoint - actual;
			integral += num * timeFrame;
			float num2 = (num - lastError) / timeFrame;
			lastError = num;
			return num * pFactor + integral * iFactor + num2 * dFactor;
		}
	}
}
