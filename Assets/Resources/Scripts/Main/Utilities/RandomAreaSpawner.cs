using UnityEngine;

namespace Utilities
{
	public class RandomAreaSpawner : MonoBehaviour
	{
		public Transform prefab;

		public int asteroidCount = 50;

		public float range = 1000f;

		public float rotationPower = 100f;

		public bool randomRotationOnAllAxes = true;

		public bool scaleMass = true;

		public Vector2 scaleRange = new Vector2(1f, 3f);

		public Vector3 spawnLocation = new Vector3(0f, 0f, 0f);

        private void Start()
		{
            if (prefab != null)
			{
				for (int i = 0; i < asteroidCount; i++)
				{
					CreateAsteroid(false);
				}
			}
		}

		private void CreateAsteroid(bool fromEdge)
		{
			Vector3 vector = ((!fromEdge) ? Random.insideUnitSphere : Random.onUnitSphere);
			vector = vector * range + spawnLocation;
			Quaternion rotation = ((!randomRotationOnAllAxes) ? Quaternion.Euler(0f, Random.Range(0, 360), 0f) : Random.rotation);
			Transform transform = Object.Instantiate(prefab, vector, rotation);
			transform.parent = base.transform;
			float num = Random.Range(scaleRange.x, scaleRange.y);
			transform.localScale = Vector3.one * num;
			Rigidbody component = transform.GetComponent<Rigidbody>();
			if ((bool)component)
			{
				if (scaleMass)
				{
					component.mass *= num * num * num;
				}
				component.AddRelativeTorque(Random.insideUnitSphere * component.mass * rotationPower);
			}
			RandomAsteroidDespawner component2 = transform.GetComponent<RandomAsteroidDespawner>();
			if ((bool)component2)
			{
				component2.despawnRange = range * 1.2f;
				component2.spawnedFrom = this;
			}
		}

		public void CreateNewAstroid(Transform astTransform)
		{
			CreateAsteroid(true);
		}

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnLocation, range);
        }
    }
}
