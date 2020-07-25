using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RevertPrefabs : MonoBehaviour {

    [MenuItem("Tools/Revert to Prefab %r")]
    static void Revert() {
        var selection = Selection.gameObjects;
        foreach(var sel in selection) {
            var img = sel.GetComponent<Image>();
            PrefabUtility.RevertObjectOverride(img, InteractionMode.UserAction);
            PrefabUtility.RevertObjectOverride(img, InteractionMode.AutomatedAction);
        }
    }
}
