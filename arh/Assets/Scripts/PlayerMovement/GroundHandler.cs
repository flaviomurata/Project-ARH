using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundHandler : MonoBehaviour
{
    public UnityEvent IsGroundedEvent;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            IsGroundedEvent.Invoke();
        }
    }
}
