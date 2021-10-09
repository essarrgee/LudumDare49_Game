using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinCharacter : Character
{	
    protected override void Awake()
	{
		base.Awake();
	}
	
	protected override void Update()
    {
		base.Update();
	}
	
	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (!destroyed && collision.gameObject != null) {
			GameObject obj = collision.gameObject;
			Destructible destructible = obj.GetComponent<Destructible>();
			if (destructible == null) {
				destructible = obj.transform.parent.GetComponent<Destructible>();
			}
			if (destructible != null && !destructible.destroyed) {
				destructible.DestroyObject();
				DestroyCharacter();
			}
		}
	}
	
	protected virtual void OnTriggerEnter(Collider collision)
	{
		if (!destroyed && collision.gameObject != null) {
			GameObject obj = collision.gameObject;
			Destructible destructible = obj.GetComponent<Destructible>();
			if (destructible == null) {
				destructible = obj.transform.parent.GetComponent<Destructible>();
			}
			if (destructible != null && !destructible.destroyed) {
				destructible.DestroyObject();
				DestroyCharacter();
			}
		}
	}
	
	public virtual void EnterBuilding()
	{
		if (!destroyed) {
			destroyed = true;
			
			if (controller != null) {
				controller.DestroyController();
			}
			
			Destroy(gameObject, 0.1f);
		}
	}
}
