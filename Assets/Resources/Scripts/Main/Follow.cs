using UnityEngine;

public class Follow : MonoBehaviour
{
	public Transform target;

	public bool useFixed = true;

	public float speed = 10f;

	[Header("Jitter")]
	public bool useJitter;

	public float jitterMagnitude = 0.01f;

	private void Update() 
	{
		if (target != null && !useFixed)
		{
			UpdatePosition();
		}
	}

	private void FixedUpdate()
	{
		if (target != null && useFixed)
		{
			UpdatePosition();
		}
	}

	private void UpdatePosition()
	{
		float x = jitterMagnitude * Mathf.Sin(Time.time) / 1.3f;
		float y = jitterMagnitude * Mathf.Sin(Time.time) / 1f;
		float z = jitterMagnitude * Mathf.Sin(Time.time) / 1.6f;
		Vector3 vector = new Vector3(x, y, z);
		base.transform.position = Vector3.Lerp(base.transform.position + vector, target.position + vector, speed * Time.deltaTime);
	}
}
