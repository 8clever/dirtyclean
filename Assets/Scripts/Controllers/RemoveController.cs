using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveController : MonoBehaviour
{
    public void OnRemove () {
        Destroy(gameObject);
    }
}
