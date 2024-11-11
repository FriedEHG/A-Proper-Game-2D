using UnityEngine;
public class ImmortalitySpell : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}