using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInvisiable : MonoBehaviour
{
    void Start()
    {
        // Mouse imlecini görünmez yap ve ekranýn ortasýna kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Eðer ESC tuþuna basýlýrsa mouse imlecini görünür yap ve serbest býrak
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
