using System.Collections;
using UnityEngine;

public class SmokeJitter : MonoBehaviour
{
	[Tooltip("Speed at which the jitter's angle moves towards a new angle.")]
	public float jitterSpeed = 20f;

	[Tooltip("Maximum angle a new target jitter can be.")]
	[Range(0f, 90f)]
	public float jitterAngleMax = 1f;

	[Tooltip("How often a new angle for the jitter to move to is created.")]
	public float jitterRefreshRate = 15f;

	private Vector3 startingEulers;

	private Vector3 newJitterRot;

	private Vector3 jitterRot = Vector3.zero;
	 
	private void Start()
	{
		startingEulers = base.transform.localEulerAngles;
		StartCoroutine(NewRotationTarget());
	}

	private void Update()
	{
		jitterRot = Vector3.MoveTowards(jitterRot, newJitterRot, jitterSpeed * Time.deltaTime);
		base.transform.localEulerAngles = startingEulers + jitterRot;
	}

	private IEnumerator NewRotationTarget()
	{
		while (true)
		{
			newJitterRot.x = Random.Range(0f - jitterAngleMax, jitterAngleMax);
			newJitterRot.y = Random.Range(0f - jitterAngleMax, jitterAngleMax);
			newJitterRot.z = Random.Range(0f - jitterAngleMax, jitterAngleMax);
			if (jitterSpeed <= 0f)
			{
				jitterSpeed = 1f;
			}
			yield return new WaitForSeconds(1f / jitterRefreshRate);
		}
	}
}
