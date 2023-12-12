using System;
using System.Collections.Generic;
using UnityEngine;

public class StoresCarriage : Store
{
	[Header("Stores Carriage")]
	[Tooltip("Automatically uses any gameobjects with a name of the format \"MountHpX\" where X is a digit from 1 to 6.")]
	public List<MountPoint> mountPoints;

	public Store storePrefab;

	private const int MAX_MOUNT_POINTS = 6;

	public int NumOfMountPoints 
	{
		get
		{
			return mountPoints.Count;
		}
	}

	public event EventHandler<NewStorePrefabSetEventArgs> NewStorePrefabSetEvent;

	protected override void Awake()
	{
		base.Awake();
		mountPoints = new List<MountPoint>(6);
		if (mountPoints.Count != 0)
		{
			return;
		}
		for (int i = 1; i <= 6; i++)
		{
			Transform transform = base.transform.Find("MountHp" + i);
			if (transform != null)
			{
				mountPoints.Add(new MountPoint(transform, null));
			}
		}
	}

	private void Start()
	{
		HangTestMountable();
	}

	protected virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			Jettison();
		}
		foreach (MountPoint mountPoint in mountPoints)
		{
			if (mountPoint.store != null && mountPoint.store.transform.parent == null)
			{
				mountPoint.store = null;
			}
		}
	}

	[ContextMenu("Hang test store on all mounts.")]
	public void HangTestMountable()
	{
		if (storePrefab != null)
		{
			for (int i = 0; i < mountPoints.Count; i++)
			{
				HangStore(storePrefab, i);
			}
		}
	}

	public void HangStore(Store newStorePrefab)
	{
		for (int i = 0; i < mountPoints.Count; i++)
		{
			if (mountPoints[i].store == null)
			{
				HangStore(newStorePrefab, i);
				break;
			}
		}
	}

	public void HangStore(Store newStorePrefab, int mountNum)
	{
		RaiseNewStorePrefabSet(new NewStorePrefabSetEventArgs(newStorePrefab));
		if (mountPoints[mountNum].transform != null && mountPoints[mountNum].store == null)
		{
			Store store = UnityEngine.Object.Instantiate(newStorePrefab, base.transform, false);
			Transform obj = store.transform;
			Vector3 zero = Vector3.zero;
			store.transform.localEulerAngles = zero;
			obj.localPosition = zero;
			store.Mount(mountPoints[mountNum].transform);
			mountPoints[mountNum].store = store;
		}
	}

	public float GetTotalMass()
	{
		float num = _mass;
		foreach (MountPoint mountPoint in mountPoints)
		{
			num += mountPoint.GetMassOfStore();
		}
		return num;
	}

	public override void Jettison(Vector3 initVelocity)
	{
		if (!canJettison)
		{
			foreach (MountPoint mountPoint in mountPoints)
			{
				if (mountPoint.store != null)
				{
					mountPoint.store.Jettison(initVelocity);
				}
				mountPoint.store = null;
			}
			return;
		}
		base.Jettison(initVelocity);
	}

	public override float GetMass()
	{
		float num = _mass;
		foreach (MountPoint mountPoint in mountPoints)
		{
			if (mountPoint.store != null)
			{
				num += mountPoint.store.GetMass();
			}
		}
		return num;
	}

	protected virtual void RaiseNewStorePrefabSet(NewStorePrefabSetEventArgs e)
	{
		EventHandler<NewStorePrefabSetEventArgs> newStorePrefabSetEvent = this.NewStorePrefabSetEvent;
		if (newStorePrefabSetEvent != null)
		{
			e.storePrefab = storePrefab;
			newStorePrefabSetEvent(this, e);
		}
	}
}
