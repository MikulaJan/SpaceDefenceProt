using UnityEngine;
using Utilities;

public class GenericFCS
{
	public PidValues PidX = new PidValues(0.5f, 0f, 0f);

	public PidValues PidY = new PidValues(0.5f, 0f, 0f);

	public PidValues PidZ = new PidValues(0.25f, 0f, 0f);

	public PidValues PidPitch = new PidValues(0.5f, 0f, 0f);

	public PidValues PidYaw = new PidValues(0.5f, 0f, 0f);

	public PidValues PidRoll = new PidValues(0.5f, 0f, 0f);

	private PID horizontalPID;

	private PID verticalPID;

	private PID forwardPID;

	private PID pitchPID;

	private PID yawPID;

	private PID rollPID;

	public bool UseSmoothMomentum { get; set; }

	public GenericFCS(Pid3DValues translationPids, Pid3DValues rotationPids)
	{
		SetTranslationPIDValues(translationPids);
		SetNewRotationPIDValues(rotationPids);
		horizontalPID = new PID(PidX.P, PidX.I, PidX.D);
		verticalPID = new PID(PidY.P, PidY.I, PidY.D);
		forwardPID = new PID(PidZ.P, PidZ.I, PidZ.D);
		pitchPID = new PID(PidPitch.P, PidPitch.I, PidPitch.D);
		yawPID = new PID(PidYaw.P, PidYaw.I, PidYaw.D);
		rollPID = new PID(PidRoll.P, PidRoll.I, PidRoll.D);
	}

	public void SetTranslationPIDValues(Pid3DValues translationPids)
	{
		PidX = translationPids.X;
		PidY = translationPids.Y;
		PidZ = translationPids.Z;
	}

	public void SetNewRotationPIDValues(Pid3DValues rotationPids)
	{
		PidPitch = rotationPids.X;
		PidYaw = rotationPids.Y;
		PidRoll = rotationPids.Z;
	}

	public float FlyToTargetSpeed(float targetSpeed, float curSpeed)
	{
		float num = 0f;
		num = forwardPID.Update(targetSpeed, curSpeed, Time.deltaTime);
		return Mathf.Clamp(num, -1f, 1f);
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

	public float FlyToTargetPitchRate(float currentPitchRate, float targetPitchRate)
	{
		return FlyToTargetRotationRate(pitchPID, currentPitchRate, targetPitchRate);
	}

	public float FlyToTargetYawRate(float currentYawRate, float targetYawRate)
	{
		return FlyToTargetRotationRate(yawPID, currentYawRate, targetYawRate);
	}

	public float FlyToTargetRollRate(float currentRollRate, float targetRollRate)
	{
		return FlyToTargetRotationRate(rollPID, currentRollRate, targetRollRate);
	}

	private float FlyToTargetRotationRate(PID pid, float rate, float targetRate)
	{
		float num = 0f;
		num = pitchPID.Update(targetRate, rate, Time.deltaTime);
		return Mathf.Clamp(num, -1f, 1f);
	}

	private float CalculatePartialThrustWhenFlyingBackwards(float forwardSpeed, float fullThrust)
	{
		float value = Mathf.InverseLerp(-0.5f, 0f, forwardSpeed);
		value = Mathf.Clamp(value, -1f, 1f);
		return fullThrust * value;
	}
}
