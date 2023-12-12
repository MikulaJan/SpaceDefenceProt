using UnityEngine;

public class Mover : MonoBehaviour
{
	public Vector3 move;

	public bool useFixed;
	 
	private Rigidbody rigidbody;

	public void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (useFixed)
		{
			MoveObject();
		}
	}

	private void Update()
	{
		if (!useFixed)
		{
			MoveObject();
		}
	}

	private void MoveObject()
	{
		if ((bool)rigidbody)
		{
			rigidbody.velocity = base.transform.InverseTransformDirection(move);
		}
		else
		{
			base.transform.Translate(move * Time.deltaTime, Space.Self);
		}
	}
}
