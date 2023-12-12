using UnityEngine;

public class MainMenuSceneLogic : MonoBehaviour
{
	public Ship[] resetShips;

	private const float RESET_TIME = 60f;

	private float timer;

	private Vector3[] startPositions;

	private Quaternion[] startRotations;

	public void Start()
	{
		startPositions = new Vector3[resetShips.Length];
		startRotations = new Quaternion[resetShips.Length];
		for (int i = 0; i < resetShips.Length; i++)
		{
			startPositions[i] = resetShips[i].Position;
			startRotations[i] = resetShips[i].transform.rotation;
		}
		timer = 60f;
	}

	public void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			for (int i = 0; i < resetShips.Length; i++)
			{
				resetShips[i].transform.position = startPositions[i];
				resetShips[i].transform.rotation = startRotations[i];
			}
			timer = 60f;
		}
	}
}
