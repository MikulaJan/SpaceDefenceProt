using System;
using UnityEngine;

namespace Utilities
{
	[Serializable]
	public class Pid3DValues
	{
		public PidValues X { get; set; }

		public PidValues Y { get; set; }

		public PidValues Z { get; set; }

		public Pid3DValues()
		{
			X = new PidValues(0f, 0f, 0f);
			Y = new PidValues(0f, 0f, 0f);
			Z = new PidValues(0f, 0f, 0f);
		}

		public Pid3DValues(Vector3 x, Vector3 y, Vector3 z)
		{
			X = new PidValues(x);
			Y = new PidValues(y);
			Z = new PidValues(z);
		}

		public Pid3DValues(PidValues x, PidValues y, PidValues z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}
