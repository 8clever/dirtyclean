using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission {
    public enum Type
    {
        Destroy,
        Collect,
        Create
    }
    public int requiredCount;
    public int count = 0;
    public Type type;
    [UnityEngine.SerializeField]
    public Nip Nip;
    public string CustomMissionText = string.Empty;
    private bool Completed = false;
    public bool IsComplete () {
        if (Completed) return true;
        if (count < requiredCount) return false;
        Completed = true;
        return true;
    }
}


