using UnityEngine;

public class POVRotator : MonoBehaviour
{
	private Vector3 targetRot;

	private Vector3 currentRot;

	private void Update()
	{
		float axis = Input.GetAxis("POVHorizontal");
		float axis2 = Input.GetAxis("POVVertical");
		targetRot.x += axis2;
		targetRot.y += axis;
		currentRot = Vector3.Lerp(currentRot, targetRot, 20f * Time.fixedDeltaTime);
		base.transform.eulerAngles = currentRot;
	}
}
  