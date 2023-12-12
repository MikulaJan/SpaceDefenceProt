using System;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
	public ITargetable target;

	public bool radarLock;

	private List<Queue<FireControlLauncher>> launcherQueues;

	private Station[] stations;

	private Ship ship;

	private Gun gun;

	private int selectedWep;

	private int numWeapons; 

	public void Awake()
	{
		ship = GetComponent<Ship>();
		stations = GetComponentsInChildren<Station>();
		launcherQueues = new List<Queue<FireControlLauncher>>();
	}

	public void OnEnable()
	{
		Station[] array = stations;
		foreach (Station station in array)
		{
			station.HangNewStoreOnStationAction = (Action<Store, Station>)Delegate.Combine(station.HangNewStoreOnStationAction, new Action<Store, Station>(HangNewStoreOnStationListener));
		}
	}

	public void OnDisable()
	{
		Station[] array = stations;
		foreach (Station station in array)
		{
			station.HangNewStoreOnStationAction = (Action<Store, Station>)Delegate.Remove(station.HangNewStoreOnStationAction, new Action<Store, Station>(HangNewStoreOnStationListener));
		}
	}

	public void Update()
	{
		if (!ship)
		{
			return;
		}
		if (ship.Controls.cycleWeapon)
		{
			CycleWeapon();
		}
		if (ship.Gun != null && ship.Controls.fireGun)
		{
			ship.Gun.Fire(ship.Rigidbody.velocity);
		}
		if (launcherQueues.Count <= 0 || launcherQueues[selectedWep].Count <= 0 || !ship.Controls.fireMissile)
		{
			return;
		}
		bool flag = false;
		int num = 0;
		do
		{
			FireControlLauncher fireControlLauncher = launcherQueues[selectedWep].Dequeue();
			if (target != null && radarLock)
			{
				fireControlLauncher.Launcher.target = target;
			}
			flag = fireControlLauncher.Launcher.LaunchWeapon(ship.Rigidbody.velocity);
			launcherQueues[selectedWep].Enqueue(fireControlLauncher);
			num++;
		}
		while (!flag && num < launcherQueues[selectedWep].Count + 1);
	}

	public void CycleWeapon()
	{
		selectedWep++;
		if (selectedWep >= numWeapons)
		{
			selectedWep = 0;
		}
		MonoBehaviour.print("new selected weapon: " + selectedWep);
	}

	public void SetWeapon(int newWep)
	{
		selectedWep = Mathf.Clamp(newWep, 0, numWeapons);
		MonoBehaviour.print("new selected weapon: " + selectedWep);
	}

	private void HangNewStoreOnStationListener(Store newStore, Station source)
	{
		Launcher componentInChildren = newStore.GetComponentInChildren<Launcher>();
		if (componentInChildren != null)
		{
			AddNewLauncherToAppropriateQueue(source, componentInChildren);
		}
	}

	private void AddNewLauncherToAppropriateQueue(Station sourceStation, Launcher potentialNewLauncher)
	{
		int num = -1;
		for (int i = 0; i < launcherQueues.Count; i++)
		{
			if (launcherQueues[i].Count > 0 && launcherQueues[i].Peek().Launcher.GetMunitionFromStoresCarriage() != null && launcherQueues[i].Peek().Launcher.GetMunitionFromStoresCarriage() == potentialNewLauncher.GetMunitionFromStoresCarriage())
			{
				num = i;
			}
		}
		if (num >= 0)
		{
			AddNewLauncherToQueue(sourceStation, potentialNewLauncher, launcherQueues[num]);
			return;
		}
		Queue<FireControlLauncher> queue = new Queue<FireControlLauncher>();
		FireControlLauncher item = new FireControlLauncher(potentialNewLauncher, sourceStation.cycleOrder);
		queue.Enqueue(item);
		launcherQueues.Add(queue);
		numWeapons++;
	}

	private void AddNewLauncherToQueue(Station sourceStation, Launcher potentialNewLauncher, Queue<FireControlLauncher> queue)
	{
		FireControlLauncher item = new FireControlLauncher(potentialNewLauncher, sourceStation.cycleOrder);
		queue.Enqueue(item);
		FireControlLauncher[] array = queue.ToArray();
		SortedList<int, FireControlLauncher> sortedList = new SortedList<int, FireControlLauncher>();
		FireControlLauncher[] array2 = array;
		foreach (FireControlLauncher fireControlLauncher in array2)
		{
			sortedList.Add(fireControlLauncher.CycleOrder, fireControlLauncher);
		}
		queue.Clear();
		foreach (KeyValuePair<int, FireControlLauncher> item2 in sortedList)
		{
			queue.Enqueue(item2.Value);
		}
	}

	public string GetNameOfSelectedWeapon()
	{
		string result = string.Empty;
		if (launcherQueues.Count > 0 && launcherQueues[selectedWep].Count > 0)
		{
			result = launcherQueues[selectedWep].Peek().Launcher.GetNameOfMunition();
		}
		return result;
	}

	public int GetTotalAmmoOfSelectedWeapon()
	{
		int num = 0;
		if (launcherQueues.Count > 0)
		{
			FireControlLauncher[] array = launcherQueues[selectedWep].ToArray();
			foreach (FireControlLauncher fireControlLauncher in array)
			{
				num += fireControlLauncher.Launcher.AmmoCount;
			}
		}
		return num;
	}

	public void ReloadAllWeapons()
	{
		foreach (Queue<FireControlLauncher> launcherQueue in launcherQueues)
		{
			foreach (FireControlLauncher item in launcherQueue)
			{
				item.Launcher.Reload();
			}
		}
	}
}
