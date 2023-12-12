using UnityEngine;

public class Store : Hardpoint
{
	[Tooltip("Mass of the object not including anything that might attach to it.")]
	public float emptyMass;

	[Header("Jettison")]
	public bool canJettison; 

	public float jettisonVelocity = 1f;

	public float jettisonRotation = 0.2f;

	public IStoresCapable StoresCarrier;

	protected Rigidbody _rigidbody;

	protected bool _hasBeenJettisoned;

	protected float _mass;

	protected IStoresCapable _storesCarrier;

	protected override void Awake()
	{
		base.Awake();
		_mass = emptyMass;
		if (canJettison)
		{
			_rigidbody = GetComponent<Rigidbody>();
			if (_rigidbody == null)
			{
				_rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			_rigidbody.useGravity = false;
			_rigidbody.angularDrag = 0f;
			_rigidbody.drag = 0f;
			_rigidbody.mass = _mass;
			_rigidbody.isKinematic = true;
		}
	}

	public void SetParentStoresCarrier(IStoresCapable carrier)
	{
		_storesCarrier = carrier;
	}

	public void Jettison()
	{
		Vector3 initVelocity = Vector3.zero;
		if (_storesCarrier != null)
		{
			initVelocity = _storesCarrier.Velocity;
		}
		Jettison(initVelocity);
	}

	public virtual void Jettison(Vector3 initVelocity)
	{
		if (canJettison && !_hasBeenJettisoned)
		{
			base.transform.SetParent(null);
			if ((bool)_rigidbody)
			{
				_rigidbody.isKinematic = false;
				_rigidbody.velocity = initVelocity;
				_rigidbody.AddRelativeForce(Vector3.down * jettisonVelocity, ForceMode.VelocityChange);
				_rigidbody.AddRelativeTorque(Random.insideUnitSphere * jettisonRotation, ForceMode.VelocityChange);
			}
			Rigidbody[] componentsInChildren = GetComponentsInChildren<Rigidbody>();
			Rigidbody[] array = componentsInChildren;
			foreach (Rigidbody rigidbody in array)
			{
				if (rigidbody != _rigidbody)
				{
					Object.Destroy(rigidbody);
				}
			}
		}
		_hasBeenJettisoned = true;
	}

	public override void Mount(Transform newParent)
	{
		base.Mount(newParent);
		_storesCarrier = GetComponentInParent<IStoresCapable>();
		if (_storesCarrier != null && _collider != null)
		{
			IgnoreCollisionWithAllChildren(_storesCarrier.Transform, _collider);
		}
		else
		{
			Debug.Log(base.name + ": Store created but has either no collider on self or parent ship to ignore collisions with.");
		}
	}

	public bool GetJettisonedStatus()
	{
		return _hasBeenJettisoned;
	}

	public virtual float GetMass()
	{
		return _mass;
	}
}
