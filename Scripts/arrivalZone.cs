using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrivalZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            FindObjectOfType<gameUI>().showGameWinUI();
        }
    }
}
