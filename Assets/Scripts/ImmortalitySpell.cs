using UnityEngine;
public class ImmortalitySpell : MonoBehaviour
{
	bool isFocused;

	void OnApplicationFocus(bool hasFocus)
	{
		isFocused = hasFocus;

		if (isFocused)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}