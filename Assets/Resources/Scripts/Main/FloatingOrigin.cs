using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
	public float threshold = 100f;

	public float physicsThreshold = 1000f;

	public float defaultSleepThreshold = 0.14f;

	private ParticleSystem.Particle[] parts;

	private void LateUpdate()
	{
		Vector3 position = base.gameObject.transform.position;
		if (!(position.magnitude > threshold))
		{
			return;
		}
		Object[] array = Object.FindObjectsOfType<Transform>();
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			Transform transform = (Transform)@object;
			if (transform.parent == null)
			{
				transform.position -= position;
			}
		}
		array = Object.FindObjectsOfType<ParticleSystem>();
		Object[] array3 = array;
		foreach (Object object2 in array3)
		{
			ParticleSystem particleSystem = (ParticleSystem)object2;
			if (particleSystem.main.simulationSpace != ParticleSystemSimulationSpace.World)
			{
				continue; 
			}
			int maxParticles = particleSystem.main.maxParticles;
			if (maxParticles > 0)
			{
				bool isPaused = particleSystem.isPaused;
				bool isPlaying = particleSystem.isPlaying;
				if (!isPaused)
				{
					particleSystem.Pause();
				}
				if (parts == null || parts.Length < maxParticles)
				{
					parts = new ParticleSystem.Particle[maxParticles];
				}
				int particles = particleSystem.GetParticles(parts);
				for (int k = 0; k < particles; k++)
				{
					parts[k].position -= position;
				}
				particleSystem.SetParticles(parts, particles);
				if (isPlaying)
				{
					particleSystem.Play();
				}
			}
		}
		array = Object.FindObjectsOfType<TrailRenderer>();
		Object[] array4 = array;
		foreach (Object object3 in array4)
		{
			TrailRenderer trailRenderer = (TrailRenderer)object3;
			trailRenderer.Clear();
		}
		array = Object.FindObjectsOfType<CustomTrailRenderer>();
		Object[] array5 = array;
		foreach (Object object4 in array5)
		{
			CustomTrailRenderer customTrailRenderer = (CustomTrailRenderer)object4;
			Vector3[] linePositions = customTrailRenderer.GetLinePositions();
			for (int n = 0; n < linePositions.Length; n++)
			{
				linePositions[n] -= position;
			}
			customTrailRenderer.SetLinePositions(linePositions);
		}
		if (!(physicsThreshold > 0f))
		{
			return;
		}
		float num = physicsThreshold * physicsThreshold;
		array = Object.FindObjectsOfType(typeof(Rigidbody));
		Object[] array6 = array;
		foreach (Object object5 in array6)
		{
			Rigidbody rigidbody = (Rigidbody)object5;
			if (rigidbody.gameObject.transform.position.sqrMagnitude > num)
			{
				rigidbody.sleepThreshold = float.MaxValue;
			}
			else
			{
				rigidbody.sleepThreshold = defaultSleepThreshold;
			}
		}
	}
}
