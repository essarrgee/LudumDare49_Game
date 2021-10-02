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
		if (collision.gameObject != null) {
			GameObject obj = collision.gameObject;
			Destructible destructible = obj.GetComponent<Destructible>();
			if (destructible != null) {
				Destroy(obj);
				DestroyCharacter();
			}
		}
	}
}
