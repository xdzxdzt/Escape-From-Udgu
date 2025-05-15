using UnityEngine;

public class CursorControl : MonoBehaviour
{
    private void Start()
    {
        // Курсор видимый и управляемый
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
