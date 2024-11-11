using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBttnBehav : MonoBehaviour
{
    [SerializeField] Color defaultColor;
    [SerializeField] Color lightColor;
    [SerializeField] Color darkColor;   //Maybe in the future we can put these rbg values into Universals



    public void DefaultColorSet()
    {
		GetComponent<Image>().color = defaultColor;
	}

    public void LightColorSet()
    {
        GetComponent<Image>().color = lightColor;
    }

	public void DarkColorSet()
	{
		GetComponent<Image>().color = darkColor;
	}
}
