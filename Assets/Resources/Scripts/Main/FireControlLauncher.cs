public class FireControlLauncher
{
	private int cycleOrder;

	private Launcher launcher;

	private string nameOfMunition;

	public Launcher Launcher
	{
		get
		{
			return launcher;
		}
	}

	public int CycleOrder
	{
		get
		{
			return cycleOrder;
		}
	}

	public string MunitionName
	{
		get
		{
			return nameOfMunition;
		}
		set
		{
			nameOfMunition = value;
		}
	}

	public FireControlLauncher(Launcher launcher, int cycleOrder)
	{
		this.launcher = launcher;
		this.cycleOrder = cycleOrder;
	}

	public FireControlLauncher(Launcher launcher, int cycleOrder, string nameOfMunition)
	{
		this.launcher = launcher;
		this.cycleOrder = cycleOrder;
		this.nameOfMunition = nameOfMunition;
	}
}
