using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public List<GameObject> chunkPrefabList;
	
	public Vector3 chunkSpawnOffset;
	
	public bool destroyed = false;
	
	protected virtual void Awake()
	{
		
	}
	
	public virtual void DestroyObject()
	{
		foreach (GameObject chunkPrefab in chunkPrefabList) {
			destroyed = true;
			GameObject chunk = Instantiate(chunkPrefab, 
				transform.position + chunkSpawnOffset, 
				Quaternion.identity);
			
			chunk.transform.eulerAngles = new Vector3(Random.Range(-180f,180f),
				Random.Range(-180f,180f),Random.Range(-180f,180f));
		}
		Destroy(gameObject, 0.1f);
	}
}
