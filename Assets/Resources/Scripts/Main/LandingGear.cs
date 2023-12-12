using UnityEngine;

[RequireComponent(typeof(Ship))]
public class LandingGear : MonoBehaviour
{
	public float gearDownPercent;

	public float timeToExtend = 5f;

	public GearState startingState;

	public string ExtendAnimState = "Extend";

	public Light noseGearLight;

	public float brakePct;

	public WheelInfo[] wheelInfo;

	private Ship ship;

	private Gear[] gear;

	private float steering;

	private bool gearDownCommanded;

	public void Awake() 
	{
		ship = GetComponent<Ship>();
		if (ship.animator != null)
		{
			gear = new Gear[wheelInfo.Length];
			for (int i = 0; i < gear.Length; i++)
			{
				gear[i] = new Gear(startingState, timeToExtend, ship.animator, wheelInfo[i]);
			}
		}
		else
		{
			Debug.Log(base.name + ": Landing Gear has no ship animator to reference.");
			base.enabled = false;
		}
	}

	public void Update()
	{
		RunControls();
		Gear[] array = this.gear;
		foreach (Gear gear in array)
		{
			gear.Update();
			gear.SetSteering(steering);
			gearDownPercent = Mathf.Max(gear.GetState());
		}
		if (noseGearLight != null)
		{
			noseGearLight.gameObject.SetActive(gearDownPercent >= 1f);
		}
	}

	private void RunControls()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (gearDownCommanded)
			{
				RaiseGear();
			}
			else
			{
				LowerGear();
			}
			gearDownCommanded = !gearDownCommanded;
		}
		if (gearDownPercent >= 1f)
		{
			steering = ship._input.Yaw;
		}
		else
		{
			steering = 0f;
		}
	}

	[ContextMenu("Raise Gear")]
	public void RaiseGear()
	{
		Gear[] array = this.gear;
		foreach (Gear gear in array)
		{
			gear.RaiseGear();
		}
	}

	[ContextMenu("Lower Gear")]
	public void LowerGear()
	{
		Gear[] array = this.gear;
		foreach (Gear gear in array)
		{
			gear.LowerGear();
		}
	}

	public void SetBrakePct(float pct)
	{
		brakePct = pct;
		Gear[] array = this.gear;
		foreach (Gear gear in array)
		{
			gear.SetBrakePct(brakePct);
		}
	}

	public bool GetGearLandedOnRigidbody(out Rigidbody rbody)
	{
		rbody = null;
		bool result = false;
		Gear[] array = this.gear;
		foreach (Gear gear in array)
		{
			if (gear.weightOnWheel)
			{
				rbody = gear.GetLandedOnRigidbody();
				result = true;
			}
		}
		return result;
	}

	public bool IsWeightOnWheels()
	{
		bool flag = false;
		Gear[] array = this.gear;
		foreach (Gear gear in array)
		{
			flag |= gear.weightOnWheel;
		}
		return flag;
	}
}
