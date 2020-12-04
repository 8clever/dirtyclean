using System;
using com.adjust.sdk;
using UnityEngine.Events;

namespace UnityEngine.GameGrowth
{
    [Serializable]
    public class AttributionCallbackHandler : UnityEvent<AdjustAttribution> {}
}