using UnityEngine;
using Utilities;

public class ShipAI : MonoBehaviour
{
    private const string THROTTLE_INCREASE = "Throttle Increase";

	private const string THROTTLE_DECREASE = "Throttle Decrease";

	private const string CYCLE_CAMERA = "Cycle Camera";

	public bool usePlayerInput;

    [Sapp.Utils.Header("Debug")]
	[Range(0f, 1f)]
    public float throttle;

	public float brakes;

	public bool parkingBrake;

	public bool fireGun;

	public bool fireMissile;

	public bool cycleWeapon;

	private bool throttleMaxed;

	private bool throttleMaxedLock;

	private bool throttleZeroed;

	private bool throttleZeroLock;

	private float LastThrottleIncreasePressTime = -0.2f;

	private float LastThrottleDecreasePressTime = -0.2f;

	private const float THROTTLE_SPEED = 0.5f;

	private const float THROTTLE_LANDED_MODIFIER = 0.1f;

	private const float THROTTLE_DISCONNECTED_SPEED = 5f;

	private const float THROTTLE_DOUBLE_TAP_TIME = 0.2f;

	public bool fcsConnected = true;

	private const float AI_RANDOM_TIME = 3f;

	private float randomTimer = 3f;

	private float tapBrakes;

	private Ship ship;

	[SerializeField]
	private GameObject pauseCamera;




    public void Awake()
	{
		ship = GetComponent<Ship>();
    }

	public void Start()
	{
		if (usePlayerInput)
		{
			CameraManager.ActivateSelectedCamera();
		}
    }

	public void Update()
	{
		if (usePlayerInput)
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				fcsConnected = !fcsConnected;
				MonoBehaviour.print("Flight computer disconnected.");
			}
			if (Input.GetButtonDown("Cycle Camera"))
			{
				CameraManager.CycleSelectedCamera();
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				CameraManager.EnablePauseMode(pauseCamera, this);

            }
			HandleBrakeInput();
			HandleThrottleInput();
			HandleWeaponInput();
		}
    }
    private void HandleBrakeInput()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			parkingBrake = !parkingBrake;
		}
		brakes = ((!parkingBrake) ? tapBrakes : 1f);
	}

	private void HandleWeaponInput()
	{
		fireGun = Input.GetButton("Fire1");
		fireMissile = Input.GetButtonDown("Fire2");
		cycleWeapon = Input.GetButtonDown("Fire3");
	}

	private void HandleThrottleInput()
	{
		CheckThrottleMaxed();
		CheckThrottleZeroed();
		if (fcsConnected)
		{
			float num = 0.5f;
			if (ship != null && ship.landingGear != null && ship.landingGear.IsWeightOnWheels())
			{
				num *= 0.1f;
			}
			if (throttleMaxed)
			{
				throttleMaxed = false;
				throttle = 1f;
			}
			else if (throttleZeroed)
			{
				throttleZeroed = false;
				throttle = 0f;
			}
			else if (!throttleZeroLock && !throttleMaxedLock)
			{
				if (Input.GetButton("Throttle Increase"))
				{
					throttle = Mathf.MoveTowards(throttle, 1f, num * Time.deltaTime);
				}
				else if (Input.GetButton("Throttle Decrease"))
				{
					throttle = Mathf.MoveTowards(throttle, -1f, num * Time.deltaTime);
				}
			}
		}
		else if (Input.GetButton("Throttle Increase"))
		{
			throttle = Mathf.MoveTowards(throttle, 1f, 5f * Time.deltaTime);
		}
		else if (Input.GetButton("Throttle Decrease"))
		{
			throttle = Mathf.MoveTowards(throttle, -1f, 5f * Time.deltaTime);
		}
		else
		{
			throttle = Mathf.MoveTowards(throttle, 0f, 10f * Time.deltaTime);
		}
	}

    private void CheckThrottleZeroed()
	{
		if (Input.GetButtonDown("Throttle Decrease"))
		{
			if (TimeUtils.Since(LastThrottleDecreasePressTime, Time.time) < 0.2f)
			{
				tapBrakes = 1f;
				throttleZeroed = true;
				throttleZeroLock = true;
			}
			LastThrottleDecreasePressTime = Time.time;
		}
		else if (Input.GetButtonUp("Throttle Decrease"))
		{
			throttleZeroLock = false;
			tapBrakes = 0f;
		}
	}

	private void CheckThrottleMaxed()
	{
		if (Input.GetButtonDown("Throttle Increase"))
		{
			if (TimeUtils.Since(LastThrottleIncreasePressTime, Time.time) < 0.2f)
			{
				throttleMaxed = true;
				throttleMaxedLock = true;
			}
			LastThrottleIncreasePressTime = Time.time;
		}
		else if (Input.GetButtonUp("Throttle Increase"))
		{
			tapBrakes = 0f;
			throttleMaxedLock = false;
		}
	}
}
