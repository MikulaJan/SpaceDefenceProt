using UnityEngine;

public class EffectsDetacherTester : MonoBehaviour
{
	public Transform toDetach;

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			Detach();
		}
	}

	[ContextMenu("Detach effect")]
	public void Detach()
	{
		if (toDetach != null)
		{
			ParticleSystem componentInChildren = toDetach.GetComponentInChildren<ParticleSystem>();
			if ((bool)componentInChildren)
			{
				componentInChildren.Stop();
			}
			toDetach.transform.SetParent(null);
		}
	}
}
