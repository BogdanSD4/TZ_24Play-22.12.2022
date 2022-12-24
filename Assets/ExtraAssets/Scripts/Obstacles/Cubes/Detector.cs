using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public static Transform FirstCube; 

    private void OnTriggerEnter(Collider other)
    {
        if (FirstCube != transform) return;
        if (other.gameObject.CompareTag(Constans.TAG_CUBE_PICKUP))
        {
            PlayerController.Main.AddCube((BoxCollider)other);
        }
    }
}
