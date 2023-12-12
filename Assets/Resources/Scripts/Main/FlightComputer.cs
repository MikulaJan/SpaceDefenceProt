using Arena;
using SpaceDenfece;
using UnityEngine;
using UnityEngine.Windows;
using Utilities;

public class FlightComputer : MonoBehaviour
{
	private Ship ship;

	public float maxSpeed;

	public float pitchRate;

	public float yawRate;

	public float rollRate;

	[Tooltip("Must be value between 0 and 1.\nX: Pitch\nY: Yaw\nZ: Roll")]
	public Vector3 arrestRateModifiers = Vector3.one;

	[Header("Translation PIDs")]
	public Vector3 PIDX = new Vector3(0.5f, 0f, 0f);

	public Vector3 PIDY = new Vector3(0.5f, 0f, 0f);

	public Vector3 PIDZ = new Vector3(0.25f, 0f, 0f);

	[Header("Rotation PIDs")]
	public Vector3 PIDPitch = new Vector3(0.5f, 0f, 0f);

	public Vector3 PIDYaw = new Vector3(0.5f, 0f, 0f);

	public Vector3 PIDRoll = new Vector3(0.5f, 0f, 0f);

	private const float INPUT_DEADZONE = 0.33f;

	private const float LANDING_SIM_GRAVITY = -7f;

	private const float LANDED_THRUST = -0.3f;

	[Header("Output info")]
	[SerializeField]
	private Vector3 appliedThrust;

	[SerializeField]
	private Vector3 appliedRotation;

	private PID horizontalPID;

	private PID verticalPID;

	private PID forwardPID;

	private PID pitchPID;

	private PID yawPID;

	private PID rollPID;

	private float pitch;

	private float yaw;

	private float roll;

	private float throttle;

	private float targetVerticalSpeed;

	private bool gearDown;

	private bool weightOnWheels;

	private bool fcsIsConnected = true;

	private Rigidbody rigidbody;

	private Rigidbody landedOnRigidbody;

    private float randomTimer = 3f;

	private ShipAI shipAi;

	public Vector3 CommandedThrust
	{
		get
		{
			return appliedThrust;
		}
	}

	public Vector3 CommandedRotation
	{
		get
		{
			return appliedRotation;
		}
	}

	private void Awake()
	{
		ship = GetComponent<Ship>();
		rigidbody = GetComponent<Rigidbody>();
        shipAi = GetComponent<ShipAI>();
        horizontalPID = new PID(PIDX.x, PIDX.y, PIDX.z);
		verticalPID = new PID(PIDY.x, PIDY.y, PIDY.z);
		forwardPID = new PID(PIDZ.x, PIDZ.y, PIDZ.z);
		pitchPID = new PID(PIDPitch.x, PIDPitch.y, PIDPitch.z);
		yawPID = new PID(PIDYaw.x, PIDYaw.y, PIDYaw.z);
		rollPID = new PID(PIDRoll.x, PIDRoll.y, PIDRoll.z);
	}

	private void Update()
	{
		if (!ship)
		{
			return;
		}
        if (!shipAi.usePlayerInput)
        {
            randomTimer -= Time.deltaTime;
            if (randomTimer < 0f)
            {
                pitch = Random.Range(-0.5f, 0.5f);
                yaw = Random.Range(-0.5f, 0.5f);
                roll = Random.Range(-0.5f, 0.5f);
                throttle = Random.Range(0.5f, 1f);
                randomTimer = Random.Range(-0.6f, 0.6f) + 3f;
                SetInput(pitch, yaw, roll, throttle, true);
            }
        }
        Vector3 vector = base.transform.InverseTransformDirection(rigidbody.velocity);
        Vector3 normalized = vector.normalized;
		Vector3 vector2 = base.transform.InverseTransformDirection(rigidbody.angularVelocity) * 57.29578f;
		DebugPIDs();
		if ((bool)ship.landingGear)
		{
			weightOnWheels = ship.landingGear.GetGearLandedOnRigidbody(out landedOnRigidbody);
		}
		else
		{
			weightOnWheels = false;
		}
		if (!weightOnWheels && fcsIsConnected)
		{
			targetVerticalSpeed = Mathf.MoveTowards(targetVerticalSpeed, CalculateTargetVerticalSpeed(), 1f);
			FlyToTargetPitchRate(vector2.x, pitchRate * pitch);
			FlyToTargetYawRate(vector2.y, yawRate * yaw);
			FlyToTargetRollRate(vector2.z, rollRate * roll);
			FlyToTargetHorizontalSpeed(vector.x, 0f, normalized.z);
			FlyToTargetVerticalSpeed(vector.y, targetVerticalSpeed, normalized.z);
			FlyToTargetSpeed(throttle * maxSpeed, vector.z);
			return;
		}
		appliedRotation.x = pitch;
		appliedThrust.x = 0f;
		if (!weightOnWheels)
		{
			appliedRotation.z = roll;
			appliedRotation.y = yaw;
			appliedThrust.y = 0f;
			appliedThrust.z = throttle;
			return;
		}
		if (!ship.ParkingBrake)
		{
			float targetSpeed = CalculateRelativeLandedSpeed(throttle * maxSpeed);
			FlyToTargetSpeed(targetSpeed, vector.z);
		}
		else
		{
			appliedThrust.z = 0f;
		}
		appliedThrust.y = -0.3f;
		appliedRotation.y = 0f;
		appliedRotation.z = 0f;
    }

	private void DebugPIDs()
	{
		horizontalPID.pFactor = PIDX.x;
		horizontalPID.iFactor = PIDX.y;
		horizontalPID.dFactor = PIDX.z;
		verticalPID.pFactor = PIDY.x;
		verticalPID.iFactor = PIDY.y;
		verticalPID.dFactor = PIDY.z;
		forwardPID.pFactor = PIDZ.x;
		forwardPID.iFactor = PIDZ.y;
		forwardPID.dFactor = PIDZ.z;
		pitchPID.pFactor = PIDPitch.x;
		pitchPID.iFactor = PIDPitch.y;
		pitchPID.dFactor = PIDPitch.z;
		yawPID.pFactor = PIDYaw.x;
		yawPID.iFactor = PIDYaw.y;
		yawPID.dFactor = PIDYaw.z;
		rollPID.pFactor = PIDRoll.x;
		rollPID.iFactor = PIDRoll.y;
		rollPID.dFactor = PIDRoll.z;
	}

	private void FlyToTargetSpeed(float targetSpeed, float curSpeed)
	{
		appliedThrust.z = forwardPID.Update(targetSpeed, curSpeed, Time.deltaTime);
		appliedThrust.z = Mathf.Clamp(appliedThrust.z, -1f, 1f);
	}

	private void FlyToTargetHorizontalSpeed(float horizontalSpeed, float targetSpeed, float forwardSpeed)
	{
		float value = horizontalPID.Update(targetSpeed, horizontalSpeed, Time.deltaTime);
		value = Mathf.Clamp(value, -1f, 1f);
		if (forwardSpeed > 0f)
		{
			appliedThrust.x = value;
		}
		else
		{
			appliedThrust.x = CalculatePartialThrustWhenFlyingBackwards(forwardSpeed, value);
		}
	}

	private void FlyToTargetVerticalSpeed(float verticalSpeed, float targetSpeed, float forwardSpeed)
	{
		float value = verticalPID.Update(targetSpeed, verticalSpeed, Time.deltaTime);
		value = Mathf.Clamp(value, -1f, 1f);
		if (forwardSpeed > 0f || gearDown)
		{
			appliedThrust.y = value;
		}
		else
		{
			appliedThrust.y = CalculatePartialThrustWhenFlyingBackwards(forwardSpeed, value);
		}
	}

	private float CalculatePartialThrustWhenFlyingBackwards(float forwardSpeed, float fullThrust)
	{
		float value = Mathf.InverseLerp(-0.5f, 0f, forwardSpeed);
		value = Mathf.Clamp(value, -1f, 1f);
		return fullThrust * value;
	}

	private float CalculateTargetVerticalSpeed()
	{
		float result = 0f;
		gearDown = false;
		weightOnWheels = false;
		if ((bool)ship.landingGear)
		{
			result = ship.landingGear.gearDownPercent * -7f;
			gearDown = true;
		}
		return result;
	}

	private void FlyToTargetPitchRate(float currentPitchRate, float targetPitchRate)
	{
		float value = pitchPID.Update(targetPitchRate, currentPitchRate, Time.deltaTime);
		value = Mathf.Clamp(value, -1f, 1f) * LowPowerWhenNoActiveInputFilter(arrestRateModifiers.x, pitch);
		appliedRotation.x = value;
	}

	private void FlyToTargetYawRate(float currentYawRate, float targetYawRate)
	{
		float value = yawPID.Update(targetYawRate, currentYawRate, Time.deltaTime);
		value = Mathf.Clamp(value, -1f, 1f) * LowPowerWhenNoActiveInputFilter(arrestRateModifiers.y, yaw);
		appliedRotation.y = value;
	}

	private void FlyToTargetRollRate(float currentRollRate, float targetRollRate)
	{
		float value = rollPID.Update(targetRollRate, currentRollRate, Time.deltaTime);
		value = Mathf.Clamp(value, -1f, 1f) * LowPowerWhenNoActiveInputFilter(arrestRateModifiers.z, roll);
		appliedRotation.z = value;
	}

	private float LowPowerWhenNoActiveInputFilter(float min, float input)
	{
		float num = Mathf.Abs(input);
		min = Mathf.Clamp(min, 0f, 1f);
		if (num < 0.33f)
		{
			num = Mathf.InverseLerp(0f, 0.33f, num);
		}
		return Mathf.Lerp(min, 1f, num);
	}

	private float CalculateRelativeLandedSpeed(float targetSpeed)
	{
		float result = targetSpeed;
		if (landedOnRigidbody != null)
		{
			result = ship.transform.InverseTransformDirection(landedOnRigidbody.velocity).z + targetSpeed;
		}
		return result;
	}

	public void SetInput(float i_Pitch, float i_Yaw, float i_Roll, float i_Throttle, bool i_bIsConnected)
	{
		pitch = Mathf.Clamp(i_Pitch, -1f, 1f);
		yaw = Mathf.Clamp(i_Yaw, -1f, 1f);
		roll = Mathf.Clamp(i_Roll, -1f, 1f);
		throttle = Mathf.Clamp(i_Throttle, -1f, 1f);
		fcsIsConnected = i_bIsConnected;
	}

	public void SetRigidBodyReference(Rigidbody rbody)
	{
		rigidbody = rbody;
	}
	//
}
