using UnityEngine;

public class CarTester : MonoBehaviour
{
	private WheelCollider[] wheels;

	public float brakeTorque;

	public float pushTorque;

	private Rigidbody rigid;

	private bool flag = true;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelCollider>();
	}

	private void Update()
	{
		WheelCollider[] array = wheels;
		foreach (WheelCollider wheelCollider in array)
		{
			wheelCollider.brakeTorque = ((!Input.GetKey(KeyCode.B)) ? 0f : brakeTorque);
			wheelCollider.motorTorque = 1E-06f;
		}
	}

	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.R))
		{
			rigid.AddRelativeForce(Vector3.forward * pushTorque);
			Debug.DrawRay(base.transform.position, base.transform.forward * 100f, Color.red);
		}
		else if (Input.GetKey(KeyCode.F))
		{
			rigid.AddRelativeForce(-Vector3.forward * pushTorque);
			Debug.DrawRay(base.transform.position, -base.transform.forward * 100f, Color.red);
		}
	}
}
