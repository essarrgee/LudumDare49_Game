using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	protected float currentFadeTime;
	protected Transform model;
	
	protected Rigidbody rb;
	
    protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();
		model = transform.GetChild(0);
		
		currentFadeTime = 2f;
	}
	
	protected virtual void Start()
	{
		if (rb != null) {
			rb.AddForce(
				new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),
					Random.Range(-1f,1f))*Random.Range(10f,15f),
				ForceMode.Impulse
			);
		}
	}
	
	protected virtual void Update()
	{
		currentFadeTime = (currentFadeTime > 0) ? currentFadeTime - Time.deltaTime : 0;
		if (currentFadeTime <= 0.6f && model != null) {
			model.gameObject.SetActive(model.gameObject.activeSelf 
				? false : true);
		}
		if (currentFadeTime <= 0) {
			Destroy(gameObject);
		}
	}
}
