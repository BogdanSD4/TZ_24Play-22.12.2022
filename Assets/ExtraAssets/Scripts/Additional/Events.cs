using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

class Events : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> _events = new List<UnityEvent>();

    public void InvokeEvent(int index = 0)
    {
        _events[index].Invoke();
    }

    public void DellThis() => Destroy(gameObject);
    public void DellObject(GameObject point)
    {
        Destroy(point);
    }
}
