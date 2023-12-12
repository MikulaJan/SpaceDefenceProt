using UnityEngine;

public class Impulse : MonoBehaviour
{
	[Tooltip("Key is X")]
	public bool useKey;

	public Vector3 impulse;

	private void Start()
	{
		if (!useKey)
		{
			GetComponent<Rigidbody>().AddRelativeForce(impulse * 1000f);
		}
	}

	private void Update()
	{
		if (useKey && Input.GetKeyDown(KeyCode.X))
		{
			GetComponent<Rigidbody>().AddRelativeForce(impulse * 1000f);
		}
	} 
}
