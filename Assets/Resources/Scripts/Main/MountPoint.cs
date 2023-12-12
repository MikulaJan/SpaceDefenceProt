using System;
using UnityEngine;

[Serializable]
public class MountPoint
{
	public Transform transform;

	public Store store;

	public MountPoint(Transform tform, Store mountedStore)
	{
		transform = tform;
		store = mountedStore;
	}

	public float GetMassOfStore()
	{
		if (store != null)
		{
			return store.GetMass();
		}
		return 0f;
	}
}
