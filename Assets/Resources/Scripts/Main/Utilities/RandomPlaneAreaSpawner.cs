using UnityEngine;

namespace Utilities
{
	public class RandomPlaneAreaSpawner : MonoBehaviour
	{
		public Transform prefab;

		public int asteroidCount = 50;

		public float range = 1000f;

		public float verticalRange = 200f;

		public float rotationPower = 100f;

		public bool randomRotationOnAllAxes = true; 

		public bool scaleMass = true;

		public Vector2 scaleRange = new Vector2(1f, 3f);

		public Vector3 spawnLocation = new Vector3(0f, 0f, 0f);

		private void Start()
		{
			if (range <= 0f)
			{
				range = 1f;
			}
			if (prefab != null)
			{
				for (int i = 0; i < asteroidCount; i++)
				{
					CreateAsteroidOnPlane();
				}
			}
		}

		private void CreateAsteroidOnPlane()
		{
			Vector3 insideUnitSphere = Random.insideUnitSphere;
			insideUnitSphere.y = Random.insideUnitCircle.x * (verticalRange / range);
			insideUnitSphere = insideUnitSphere * range + spawnLocation;
			Quaternion rotation = ((!randomRotationOnAllAxes) ? Quaternion.Euler(0f, Random.Range(0, 360), 0f) : Random.rotation);
			Transform transform = Object.Instantiate(prefab, insideUnitSphere, rotation);
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
				component.AddRelativeTorque(Random.insideUnitSphere * component.mass * rotationPower * rotationPower * rotationPower);
			}
		}

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(spawnLocation, new Vector3(range * 2, verticalRange * 2, range * 2));
        }
    }
}
