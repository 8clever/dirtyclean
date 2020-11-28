using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class StarController : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    public Image starImage;
    public bool clickable = true;
    private GameObject clickableAnimation;
    private void Awake() {
        text.text = "0";
        SetText().GetAwaiter();
    }
    private void Start() {
        starImage.GetComponent<Button>().onClick.AddListener(delegate {
            clickable = false;
        });
    }
    private async Task SetText () {
        var controller = FindObjectOfType<GameController>();
        text.text = controller.stars.ToString();
        await Task.Delay(1000);
        await SetText();
    }
    private void Update() {
        if (clickable && !clickableAnimation) {
            var animation = Resources.Load<GameObject>("Animations/ClickableAnimation");
            var render = animation.GetComponent<SpriteRenderer>();
            render.sprite = starImage.sprite;
            clickableAnimation = Instantiate(animation, starImage.transform);
        }

        if (!clickable && clickableAnimation) {
            Destroy(clickableAnimation.gameObject);
            clickableAnimation = null;
        }
    }
}
