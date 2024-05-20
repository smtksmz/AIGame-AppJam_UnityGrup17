using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonColider : MonoBehaviour
{
    public GameObject gate;
    
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gate.SetActive(false);
        }
    }
}
