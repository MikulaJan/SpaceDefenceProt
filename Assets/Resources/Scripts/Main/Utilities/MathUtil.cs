using UnityEngine;

namespace Utilities
{
	public static class MathUtil
	{
		public static Vector3 ClampVec3(Vector3 vec, float min, float max)
		{
			vec.x = Mathf.Clamp(vec.x, min, max);
			vec.y = Mathf.Clamp(vec.y, min, max);
			vec.z = Mathf.Clamp(vec.z, min, max);
			return vec;
		}
	}
}
