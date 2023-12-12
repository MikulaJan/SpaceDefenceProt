using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Utilities
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PidValues
	{
		public float P { get; set; }

		public float I { get; set; }

		public float D { get; set; }

		public PidValues(Vector3 values)
		{
			P = values.x;
			I = values.y;
			D = values.z;
		}

		public PidValues(float p, float i, float d)
		{
			P = p;
			I = i;
			D = d;
		}
	}
}
