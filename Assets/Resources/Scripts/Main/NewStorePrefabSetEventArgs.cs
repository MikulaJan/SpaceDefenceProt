using System;

public class NewStorePrefabSetEventArgs : EventArgs
{
	public Store storePrefab;

	public NewStorePrefabSetEventArgs(Store newStore)
	{
		storePrefab = newStore;
	}
}
