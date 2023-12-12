using UnityEngine;

public class Hardpoint : MonoBehaviour
{
	[Tooltip("Automatically uses the first gameobject with the name that starts with \"AttachHp\" if not assigned.\nIf nothing is found, then uses the object's origin.")]
	public Transform attachPoint;

	protected Collider _collider;

	public Collider Collider
	{
		get
		{
			return _collider;
		}
	}

	protected virtual void Awake()
	{
		if (attachPoint == null)
		{
			attachPoint = base.transform.Find("AttachHp");
		}
		_collider = GetComponent<Collider>();
	}

	public virtual void Mount(Transform newParent)
	{
		base.transform.SetParent(newParent);
		Vector3 attachPointOffset = GetAttachPointOffset();
		base.transform.localPosition = attachPointOffset;
		base.transform.localEulerAngles = Vector3.zero;
	}

	public Vector3 GetAttachPointOffset()
	{
		if (attachPoint != null)
		{
			return -attachPoint.localPosition;
		}
		return Vector3.zero;
	}

	public void IgnoreCollisionWith(Collider other)
	{
		if (_collider != null && other != null)
		{
			Physics.IgnoreCollision(_collider, other);
		}
	}

	public void IgnoreCollisionWithAllChildren(Transform other)
	{
		if (_collider != null)
		{
			Collider[] componentsInChildren = other.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				Physics.IgnoreCollision(collider, _collider);
			}
		}
	}

	public void IgnoreCollisionWithAllChildren(Transform other, Collider self)
	{
		if (self != null)
		{
			Collider[] componentsInChildren = other.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				Physics.IgnoreCollision(collider, self);
			}
		}
	}
}
