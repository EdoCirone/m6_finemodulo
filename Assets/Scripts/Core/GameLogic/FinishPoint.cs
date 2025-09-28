using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private UnityEvent _whenWin;

    private void OnTriggerEnter(Collider other)
    {
        _whenWin?.Invoke();
    }
}
