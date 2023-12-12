using UnityEngine;

public class Gear
{
	private float state;

	private int extendLayer;

	private int compressLayer = 1;

	private int steeringLayer = 2;

	private GearState gearState;

	private float extensionSpeed = 0.2f;

	private WheelInfo wheelInfo;

	private WheelCollider wheel;

	private Animator anim;

	private float steeringInput;

	private float targetSteerAngle;

	private float brakePct;

	private const float MIN_MOTOR = 1E-06f;

	public bool weightOnWheel
	{
		get
		{
			return wheelInfo.wheel != null && wheelInfo.wheel.isGrounded;
		}
	}

	public Gear(GearState i_GearState, float i_TimeToExtend, Animator i_Animator, WheelInfo i_info)
	{
		switch (i_GearState)
		{
		case GearState.Extended:
		case GearState.Extending:
			state = 1f;
			gearState = GearState.Extended;
			break;
		case GearState.Retracted:
		case GearState.Retracting:
			state = 0f;
			gearState = GearState.Retracted;
			break;
		}
		if (i_TimeToExtend != 0f)
		{
			extensionSpeed = 1f / i_TimeToExtend;
		}
		wheelInfo = i_info;
		wheel = wheelInfo.wheel;
		anim = i_Animator;
		extendLayer = anim.GetLayerIndex(wheelInfo.extendLayer);
		compressLayer = anim.GetLayerIndex(wheelInfo.compressLayer);
		steeringLayer = anim.GetLayerIndex(wheelInfo.steeringLayer);
		wheel.motorTorque = 1E-06f;
	}

	public void Update()
	{
		switch (gearState)
		{
		case GearState.Extending:
			state = Mathf.MoveTowards(state, 1f, extensionSpeed * Time.deltaTime);
			gearState = ((state != 1f) ? gearState : GearState.Extended);
			break;
		case GearState.Retracting:
			state = Mathf.MoveTowards(state, 0f, extensionSpeed * Time.deltaTime);
			gearState = ((state != 0f) ? gearState : GearState.Retracted);
			break;
		}
		if (anim != null)
		{
			anim.Play(wheelInfo.extendStateName, extendLayer, state);
		}
		if (wheelInfo.steering)
		{
			wheel.steerAngle = Mathf.MoveTowards(wheel.steerAngle, targetSteerAngle, wheelInfo.steerSpeed * Time.deltaTime);
		}
		RunCompressAnimation();
		RunSteeringAnimation();
	}

	private void RunCompressAnimation()
	{
		if (anim != null && wheel != null)
		{
			Vector3 pos;
			Quaternion quat;
			wheel.GetWorldPose(out pos, out quat);
			float num = Vector3.Distance(pos, wheel.transform.position);
			float normalizedTime = 1f - num / wheel.suspensionDistance;
			if (anim.runtimeAnimatorController != null)
			{
				anim.Play(wheelInfo.compressStateName, compressLayer, normalizedTime);
			}
		}
	}

	private void RunSteeringAnimation()
	{
		if (!wheelInfo.steering || !(anim != null) || !(wheel != null))
		{
			return;
		}
		bool flag = wheel.steerAngle < 0f;
		float num = Mathf.Abs(wheel.steerAngle);
		float normalizedTime = num / wheelInfo.steerMaxAngle;
		if (anim.runtimeAnimatorController != null)
		{
			if (flag)
			{
				anim.Play(wheelInfo.steeringLeftStateName, steeringLayer, normalizedTime);
			}
			else
			{
				anim.Play(wheelInfo.steeringRightStateName, steeringLayer, normalizedTime);
			}
		}
	}

	public void RaiseGear()
	{
		if (!weightOnWheel)
		{
			gearState = GearState.Retracting;
		}
	}

	public void LowerGear()
	{
		gearState = GearState.Extending;
	}

	public float GetState()
	{
		return state;
	}

	public void SetSteering(float angle)
	{
		Mathf.Clamp(angle, -1f, 1f);
		if (wheelInfo.steering)
		{
			steeringInput = ((!wheelInfo.reverseSteer) ? angle : (0f - angle));
		}
		targetSteerAngle = steeringInput * wheelInfo.steerMaxAngle;
	}

	public Rigidbody GetLandedOnRigidbody()
	{
		Rigidbody result = null;
		WheelHit hit;
		wheel.GetGroundHit(out hit);
		if (hit.collider != null)
		{
			result = hit.collider.attachedRigidbody;
		}
		return result;
	}

	public void SetBrakePct(float pct)
	{
		brakePct = Mathf.Clamp01(pct);
		wheel.brakeTorque = wheelInfo.brakeTorque * brakePct;
	}
}
