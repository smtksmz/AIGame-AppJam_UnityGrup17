using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInvisiable : MonoBehaviour
{
    void Start()
    {
        // Mouse imlecini g�r�nmez yap ve ekran�n ortas�na kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // E�er ESC tu�una bas�l�rsa mouse imlecini g�r�n�r yap ve serbest b�rak
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
