using UnityEngine;

namespace Utilities
{
	public class RandomAsteroidDespawner : MonoBehaviour
	{
		private GameCamera cam;

		public float despawnRange = 1000f;

		public RandomAreaSpawner spawnedFrom;

		private void Update()
		{
			cam = CameraManager.GetActiveGameCamera();
			float sqrMagnitude = (cam.transform.position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude > despawnRange * despawnRange)
			{
				if ((bool)spawnedFrom)
				{
					spawnedFrom.CreateNewAstroid(base.transform);
				}
				Object.Destroy(base.gameObject);
			}
		}
	}
}
