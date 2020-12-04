using System;
using UnityEngine.Events;

namespace UnityEngine.GameGrowth
{
    [Serializable]
    public class TransactionHandler : UnityEvent<TransactionData> {}
}