using UnityEngine;

public class ConvertEuler
{
	public static float Euler180(float angle)
	{
		float num = angle;
		if (num > 180f)
		{
			num -= 360f;
		}
		return num;
	}

	public static float Clamp(float angle, float max, float min)
	{
		float value = Euler180(angle);
		return Mathf.Clamp(value, 0f - max, min);
	}
}
