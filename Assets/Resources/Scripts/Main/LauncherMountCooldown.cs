using UnityEngine;

public class LauncherMountCooldown
{
	private float coolTime = 1f;

	private float cooldown;

	private bool needsReset;

	public float CoolTime
	{
		get
		{
			return coolTime;
		}
		set
		{
			coolTime = value;
		}
	}

	public float CoolDown
	{
		get
		{
			return cooldown;
		}
	}

	public LauncherMountCooldown(float cooldownTime)
	{
		coolTime = cooldownTime;
	}

	public bool UpdateCooldownAndCheckIfFinished()
	{
		bool result = false;
		cooldown -= Time.deltaTime;
		if (cooldown <= 0f && !needsReset)
		{
			result = true;
			needsReset = true;
		}
		return result;
	}

	public void SetFinished()
	{
		cooldown = 0f;
	}

	public void ResetCooldown()
	{
		cooldown = coolTime;
		needsReset = false;
	}
}
