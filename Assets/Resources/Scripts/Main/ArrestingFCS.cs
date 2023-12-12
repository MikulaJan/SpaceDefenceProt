using UnityEngine;
using Utilities;

public class ArrestingFCS
{
	public PidValues PidX = new PidValues(0.5f, 0f, 0f);

	public PidValues PidY = new PidValues(0.5f, 0f, 0f);

	private PID horizontalPID;

	private PID verticalPID;

	public bool UseSmoothMomentum { get; set; }

	public ArrestingFCS(PidValues horizontal, PidValues vertical, bool useSmoothMomentum)
	{
		SetTranslationPIDValues(horizontal, vertical);
		horizontalPID = new PID(PidX);
		verticalPID = new PID(PidY);
		UseSmoothMomentum = useSmoothMomentum;
	}

	public void SetTranslationPIDValues(PidValues horizontal, PidValues vertical)
	{
		PidX = horizontal;
		PidY = vertical;
	}

	private float CalculatePartialThrustWhenFlyingBackwards(float forwardSpeed, float fullThrust)
	{
		float value = Mathf.InverseLerp(-0.5f, 0f, forwardSpeed);
		value = Mathf.Clamp(value, -1f, 1f);
		return fullThrust * value;
	}

	public float FlyToTargetHorizontalSpeed(float horizontalSpeed, float targetSpeed, float forwardSpeed)
	{
		return FlyToTargetLateralSpeed(horizontalPID, horizontalSpeed, targetSpeed, forwardSpeed);
	}

	public float FlyToTargetVerticalSpeed(float verticalSpeed, float targetSpeed, float forwardSpeed)
	{
		return FlyToTargetLateralSpeed(verticalPID, verticalSpeed, targetSpeed, forwardSpeed);
	}

	private float FlyToTargetLateralSpeed(PID pid, float speed, float targetSpeed, float forwardSpeed)
	{
		float num = 0f;
		float value = pid.Update(targetSpeed, speed, Time.deltaTime);
		value = Mathf.Clamp(value, -1f, 1f);
		if (UseSmoothMomentum && forwardSpeed <= 0f)
		{
			num = CalculatePartialThrustWhenFlyingBackwards(forwardSpeed, value);
			if (forwardSpeed > 0f)
			{
				num = value;
			}
		}
		else
		{
			num = value;
		}
		return num;
	}
}
