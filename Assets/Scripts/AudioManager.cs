using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public List<AudioSource> audioSourceList;
	
	public List<string> audioPlayDefaultList;
	
	protected Dictionary<string, AudioSource> audioMap;
	
	protected Transform audioContainerObject;
	
	public bool persistAcrossScenes = false;
	
    protected virtual void Awake()
	{
		audioMap = new Dictionary<string, AudioSource>();
		
		audioContainerObject = transform.Find("Audio");
		if (audioContainerObject != null) {
			for (int i=0; i < audioContainerObject.childCount; i++) {
				Transform audioObject = audioContainerObject.GetChild(i);
				AudioSource audioSource = 
					audioObject.GetComponent<AudioSource>();
				if (audioSource != null) {
					audioMap.Add(audioObject.name, audioSource);
				}
			}
		}
		
		if (audioPlayDefaultList.Count > 0) {
			for (int i=0; i<audioPlayDefaultList.Count; i++) {
				Play(audioPlayDefaultList[i]);
			}
		}
		
		if (persistAcrossScenes) {
			DontDestroyOnLoad(this.gameObject);
		}
	}
	
	public virtual void Play(string audioName)
	{
		if (audioMap.ContainsKey(audioName)) {
			audioMap[audioName].Play(0);
		}
	}
}
