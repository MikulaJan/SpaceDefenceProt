using UnityEngine;

[RequireComponent(typeof(Ship))]
public class StoresManagement : MonoBehaviour
{
	private Ship ship;

	private Station[] stations;

	public void Awake()
	{
		ship = GetComponent<Ship>();
		stations = GetComponentsInChildren<Station>();
	}

	public float GetTotalMassOfStores() 
	{
		float num = 0f;
		Station[] array = stations; 
		foreach (Station station in array)
		{
			num += station.GetTotalStationMass();
		}
		return num;
	}
}
   