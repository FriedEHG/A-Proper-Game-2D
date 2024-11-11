using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPowerUp : MonoBehaviour
{
    public TESTPowerTemplate powerTemplate;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Triggered");
		if (other.GetComponent<TESTPowerPlayer>() != null)
		{
			Destroy(gameObject);        //Self destruct
			powerTemplate.Apply(other.gameObject);  //Apply effect
		}
	}
}
