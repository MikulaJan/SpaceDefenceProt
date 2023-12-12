using System;
using UnityEngine;

public class Station : MonoBehaviour, IComparable<Station>
{
	public Action<Store, Station> HangNewStoreOnStationAction;

	[Tooltip("Sets order in which stations are cycled through. Lower numbers take priority.")]
	public int cycleOrder;

	public bool requiresPylon;

	public StoresCarriage pylonPrefab;

	public Store storePrefab;
	 
	private Ship ship;

	private StoresCarriage pylon;

	private Store store;

	public Store MountedStore
	{
		get
		{
			return store;
		}
	}

	private void Start()
	{
		ship = GetComponentInParent<Ship>();
		MountTestStore();
	}

	[ContextMenu("Mount test store")]
	public void MountTestStore()
	{
		if ((bool)storePrefab)
		{
			MountNewStore(storePrefab);
		}
	}

	public void MountNewStore(Store newStore)
	{
		if (requiresPylon)
		{
			if (pylon == null)
			{
				MountPylon();
			}
			pylon.HangStore(newStore);
			HangNewStoreOnStationAction(pylon, this);
		}
		else if (store == null)
		{
			store = UnityEngine.Object.Instantiate(newStore, base.transform, false);
			store.Mount(base.transform);
			store.SetParentStoresCarrier(ship);
			HangNewStoreOnStationAction(store, this);
		}
	}

	private void MountPylon()
	{
		pylon = UnityEngine.Object.Instantiate(pylonPrefab, base.transform, false);
		pylon.IgnoreCollisionWithAllChildren(ship.transform);
	}

	public float GetTotalStationMass()
	{
		float num = 0f;
		if ((bool)pylon)
		{
			num += pylon.GetMass();
		}
		else if ((bool)store)
		{
			num += store.GetMass();
		}
		return num;
	}

	public int CompareTo(Station other)
	{
		return cycleOrder - other.cycleOrder;
	}
}
