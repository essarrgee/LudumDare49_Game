using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropOffZone : MonoBehaviour
{
	public int requiredAmount = 5;
	
	public TextMeshProUGUI UIText;
	
	protected int currentAmount;
	protected bool cleared;
	
	
    protected virtual void Awake() 
	{
		currentAmount = requiredAmount;
		cleared = false;
	}
	
	protected virtual void OnTriggerEnter(Collider collision)
	{
		if (!cleared && collision.gameObject != null) {
			GameObject obj = collision.gameObject;
			PenguinCharacter penguin = obj.GetComponent<PenguinCharacter>();
			AddPenguin(penguin);
		}
	}
	
	protected virtual void AddPenguin(PenguinCharacter penguin)
	{
		if (penguin != null && currentAmount > 0) {
			penguin.EnterBuilding();
			ChangeAmount(currentAmount - 1);
		}
	}
	
	protected virtual void ChangeAmount(int amount)
	{
		currentAmount = amount;
		if (UIText != null) {
			UIText.text = currentAmount.ToString();
		}
	}
}
