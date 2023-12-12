using UnityEngine;

[RequireComponent(typeof(Strut))]
public class LandingGearPrototype : MonoBehaviour
{
	public float state;

	public float timeToExtend = 5f;

	public GearState gearState;

	public string ExtendAnimState = "Extend";

	private float extensionSpeed = 0.2f;

	private WheelCollider wheel;

	private Animator anim;

	public bool weightOnWheel
	{
		get
		{
			return wheel != null && wheel.isGrounded;
		}
	}

	public void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		wheel = GetComponentInChildren<WheelCollider>();
	}

	public void Start()
	{
		switch (gearState)
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
		if (timeToExtend != 0f)
		{
			extensionSpeed = 1f / timeToExtend;
		}
	}

	public void Update()
	{
		DebugControls();
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
			anim.Play(ExtendAnimState, 0, state);
		}
	}

	private void DebugControls()
	{
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.G))
		{
			RaiseGear();
		}
		else if (Input.GetKeyDown(KeyCode.G))
		{
			LowerGear();
		}
	}

	[ContextMenu("Raise Gear")]
	public void RaiseGear()
	{
		if (!weightOnWheel)
		{
			gearState = GearState.Retracting;
		}
	}

	[ContextMenu("Lower Gear")]
	public void LowerGear()
	{
		gearState = GearState.Extending;
	}
}
