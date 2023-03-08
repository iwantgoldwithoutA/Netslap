using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapEvent : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            SlapControl.Instance.SlapReturn(other.gameObject);
        }
    }
}
