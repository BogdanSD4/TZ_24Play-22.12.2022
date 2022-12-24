using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static bool _shake;
    public static CameraShake Main;
    private void Start()
    {
        Main = this;
    }
    public void Shake()
    {
        if (!_shake)
        {
            _shake = true;
            Handheld.Vibrate();
            StartCoroutine(Shaker());
        }
    }

    IEnumerator Shaker()
    {
        var camera = Camera.main;
        var transformCam = camera.transform;

        var shift = 0.025f;
        var center = camera.transform.position;
        var right = new Vector3(
            center.x + shift,
            center.y,
            center.z);
        var left = new Vector3(
            center.x - shift,
            center.y,
            center.z);
        
        var repeat = 10;
        var side = true;

        for(int i = 0; i < repeat;)
        {
            if (side)
            {
                transformCam.position = right;
            }
            else
            {
                transformCam.position = left;
            }

            side = !side;
            repeat--;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        transformCam.position = center;
        _shake = false;

        StopCoroutine(Shaker());
    }
}
