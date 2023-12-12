using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationTester : MonoBehaviour
{
	public int[] layerIDs = new int[10];

	public string[] stateNames = new string[10];

	private Animator anim;

	private void OnEnable()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			anim.Play(stateNames[0], layerIDs[0]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			anim.Play(stateNames[1], layerIDs[1]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			anim.Play(stateNames[2], layerIDs[2]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			anim.Play(stateNames[3], layerIDs[3]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			anim.Play(stateNames[4], layerIDs[4]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			anim.Play(stateNames[5], layerIDs[5]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			anim.Play(stateNames[6], layerIDs[6]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			anim.Play(stateNames[7], layerIDs[7]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			anim.Play(stateNames[8], layerIDs[8]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			anim.Play(stateNames[9], layerIDs[9]);
		}
	}
}
