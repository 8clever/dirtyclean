using UnityEngine;
using UnityEngine.UI;
public class MissionInfo : MonoBehaviour {
    public Sprite Checked;
    public Sprite NotChecked;
    public Image Check;
    public Text Text;
    public Mission mission;
    private void Start () {
        Check.sprite = mission.IsComplete() ? Checked : NotChecked;
        Text.text = mission.GetMissionInfo();
    }   
}