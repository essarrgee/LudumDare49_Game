using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrack : MonoBehaviour
{
	public int trackCount = 20;
	
	public GameObject trackPrefab;
	
	public bool reverse = false;
	
    void Awake()
	{
		if (trackPrefab != null) {
			for (int i=0; i<trackCount; i++) {
				GameObject newTrack = Instantiate(trackPrefab, transform);
				newTrack.transform.localPosition = newTrack.transform.localPosition
					+ new Vector3(0,0.4f*i*((reverse) ? -1 : 1),0);
			}
		}
	}
}
