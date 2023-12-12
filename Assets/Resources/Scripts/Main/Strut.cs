using UnityEngine;

public class Strut : MonoBehaviour
{
	public Animator animate;

	public string CompressAnimState = "Compress";

	private WheelCollider wheel;

	public void Awake()
	{
		wheel = GetComponentInChildren<WheelCollider>();
		if (wheel == null)
		{
			Debug.LogWarning("No wheel found on  " + base.name);
		}
		if (animate == null)
		{
			animate = GetComponentInChildren<Animator>();
			if (animate == null)
			{
				Debug.LogWarning("No animator found on " + base.name);
			}
			else if (animate.runtimeAnimatorController != null)
			{
				Debug.LogWarning("No animation controller found on " + base.name);
			}
		}
	}

	private void Update()
	{
		if (animate != null && wheel != null)
		{
			Vector3 pos;
			Quaternion quat;
			wheel.GetWorldPose(out pos, out quat);
			float num = Vector3.Distance(pos, wheel.transform.position);
			float normalizedTime = 1f - num / wheel.suspensionDistance;
			if (animate.runtimeAnimatorController != null)
			{
				animate.Play(CompressAnimState, 1, normalizedTime);
			}
		}
	}
}
