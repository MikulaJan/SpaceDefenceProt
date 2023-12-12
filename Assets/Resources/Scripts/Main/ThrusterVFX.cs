using System.Collections.Generic;
using UnityEngine;

public class ThrusterVFX : MonoBehaviour
{
	private Light light;

	private List<ParticleSystem> pSystems;

	public List<ParticleSystem> ignoreList;

	private float lightRange;

	private float lightIntensity;

	private ParticleSystem.MinMaxCurve[] emitSpeed;
	 
	private ParticleSystem.MinMaxGradient[] startColor;

	public float power1;

	public float power2;

	private float totalPower;

	public bool useSmoothing = true;

	public float smoothingSpeed = 5f;

	public void Awake()
	{
		light = GetComponentInChildren<Light>();
		pSystems = new List<ParticleSystem>();
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem item in array)
		{
			if (!ignoreList.Contains(item))
			{
				pSystems.Add(item);
			}
		}
	}

	public void Start()
	{
		if ((bool)light)
		{
			lightRange = light.range;
			lightIntensity = light.intensity;
		}
		if (pSystems != null)
		{
			emitSpeed = new ParticleSystem.MinMaxCurve[pSystems.Count];
			startColor = new ParticleSystem.MinMaxGradient[pSystems.Count];
			for (int i = 0; i < emitSpeed.Length; i++)
			{
				emitSpeed[i] = pSystems[i].main.startSpeed;
				startColor[i] = pSystems[i].main.startColor;
			}
		}
	}

	private void Update()
	{
		if (useSmoothing)
		{
			totalPower = Mathf.MoveTowards(totalPower, power1 + power2, smoothingSpeed * Time.deltaTime);
		}
		else
		{
			totalPower = power1 + power2;
		}
		if ((bool)light)
		{
			light.range = totalPower * lightRange;
			light.intensity = totalPower * lightIntensity;
		}
		if (pSystems == null)
		{
			return;
		}
		if (totalPower == 0f)
		{
			for (int i = 0; i < pSystems.Count; i++)
			{
				ParticleSystem.EmissionModule emission = pSystems[i].emission;
				emission.enabled = false;
			}
			return;
		}
		for (int j = 0; j < pSystems.Count; j++)
		{
			ParticleSystem.EmissionModule emission2 = pSystems[j].emission;
			emission2.enabled = true;
			ParticleSystem.MainModule main = pSystems[j].main;
			main.startSpeed = totalPower * emitSpeed[j].constant;
			main.startColor = Color.Lerp(Color.black, startColor[j].color, totalPower);
		}
	}

	public void SetPower1(float newPower)
	{
		power1 = newPower;
	}

	public void SetPower2(float newPower)
	{
		power2 = newPower;
	}

	public void StopAllParticleEffects()
	{
		foreach (ParticleSystem pSystem in pSystems)
		{
			pSystem.Stop();
		}
	}
}
