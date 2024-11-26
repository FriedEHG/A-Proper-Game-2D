using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehav : MonoBehaviour
{
    [SerializeField] InputAction exitAction;
    [SerializeField] GameObject controlsDisplay;
	[SerializeField] Button controlsShowBttn;
	[SerializeField] Button controlsReturnBttn;

    void Start()
    {
        controlsDisplay.SetActive(false);
    }

    void Update()
    {
        //ExitCheck
        if (exitAction.ReadValue<int>() != 0)
        {
            Exit();
        }
    }

    public void LaunchClassic()
    {
		/////////////////////////////////////////SceneManager.LoadScene("CharacterSelect");
		SceneManager.LoadScene("GameClassic");
	}

	public void ControlsDisplayReveal()
    {
        //Debug.Log($"{controlsDisplay.name}  {controlsDisplay.activeSelf}");
        controlsDisplay.SetActive( true );
        controlsReturnBttn.Select();
    }

	public void ControlsDisplayHide()
	{
		controlsDisplay.SetActive(false);
		controlsShowBttn.Select();
	}

	public void Exit()
    {   //Called by hitting Escape or clicking ExitBttn
        Application.Quit();
	}
}
