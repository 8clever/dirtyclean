using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnEndAnimation : MonoBehaviour
{   

    public EventTrigger.TriggerEvent trigger;
    public void OnEnd () {
        trigger.Invoke(new BaseEventData(EventSystem.current));
    }
}
