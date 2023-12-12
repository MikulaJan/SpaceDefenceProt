using UnityEngine;
using UnityEngine.UI;

public class HUDTargetManager : MonoBehaviour
{
	private new RectTransform transform;

	[Tooltip("When true, target boxes will only appear when they are around the HUD frame.")]
	public bool useTargetBoxMask;

	[Tooltip("Must contain a working Image and Mask")]
	public RectTransform targetMask;

	[Header("Target locking")]
	[Tooltip("When true, a small diamond will move towards the targed based on Lock Progress\n\nTo use this, the player must set the target and lock Progress.")]
	public bool useLockingAnimation = true;

	[Tooltip("Must be value of 0 to 1. 1 being fully locked, 0 being lock start.")]
	public float lockProgress;

	public Image lockDiamond;

	public ITargetable target;

	public static HUDTargetManager reference;

	private void Awake()
	{
		transform = GetComponent<RectTransform>(); 
		if (reference == null)
		{
			reference = this;
		}
	}

	public void AddNewTarget(ITargetable newTarget)
	{
		if (newTarget.AssignedHudTargetBox == null)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("TargetBox")) as GameObject;
			HUDTargetBox component = gameObject.GetComponent<HUDTargetBox>();
			if (useTargetBoxMask)
			{
				component.transform.SetParent(targetMask);
			}
			else
			{
				component.transform.SetParent(transform);
			}
			component.AssignParentHUD(GetComponent<HUDValues>());
			component.AssignTarget(newTarget);
			newTarget.AssignedHudTargetBox = component;
		}
	}

	public void RemoveTarget(ITargetable target)
	{
		if (target.AssignedHudTargetBox != null)
		{
			target.AssignedHudTargetBox.DestroyTargetBox();
		}
	}

	private void Update()
	{
		if (useLockingAnimation && target != null && target.AssignedHudTargetBox != null && lockProgress > 0f && lockProgress < 1f)
		{
			lockDiamond.enabled = true;
			lockDiamond.transform.position = Vector3.Lerp(lockDiamond.rectTransform.position, target.AssignedHudTargetBox.transform.position, lockProgress * lockProgress);
		}
		else
		{
			lockDiamond.transform.localPosition = Vector3.zero;
			lockDiamond.enabled = false;
		}
	}
}
