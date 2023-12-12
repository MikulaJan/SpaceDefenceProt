using UnityEngine;

public class LookAt : MonoBehaviour
{
	public Transform target;

	private void Update()
	{
		if ((bool)target)
		{
			base.transform.LookAt(target);
		}
	}
} 
