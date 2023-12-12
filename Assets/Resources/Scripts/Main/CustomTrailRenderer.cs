using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CustomTrailRenderer : MonoBehaviour
{
	public LineRenderer line;

	public int maxPoints;

	public bool useFixedUpdate;

	public Material material;

	private Queue<Vector3> positions;

	public void Start()
	{
		line = GetComponent<LineRenderer>();
		positions = new Queue<Vector3>(maxPoints);
	}

	public void Update()
	{
		if (!useFixedUpdate)
		{
			GeneratePoints();
		}
	}

	public void FixedUpdate()
	{
		if (useFixedUpdate)
		{
			GeneratePoints();
		}
	}

	public void GeneratePoints()
	{
		Vector3 position = base.transform.position;
		positions.Enqueue(position);
		if (positions.Count > maxPoints)
		{
			positions.Dequeue();
		}
		Vector3[] array = positions.ToArray();
		line.SetPositions(array);
		line.positionCount = array.Length;
	}

	public Vector3[] GetLinePositions()
	{
		return positions.ToArray();
	}

	public void SetLinePositions(Vector3[] newPos)
	{
		positions.Clear();
		foreach (Vector3 item in newPos)
		{
			positions.Enqueue(item);
		}
	}
}
