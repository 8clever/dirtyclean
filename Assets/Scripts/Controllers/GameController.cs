using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    private int step = 0;

    private int health = 0;

    public int point = 0;

    private System.DateTime? timeStartAddHealth;

    private GameObject shadow;

    private Image setka;

    public bool gameInitialized = false;

    public bool isPause = false;

    private Config config;

    public List<Mission> missions = new List<Mission>();

    private void Awake() {
        DefaultAwake();
    }

    public void DefaultAwake () {
        config = Config.GetConfig();
    }

    // Update is called once per frame
    public void DefaultUpdate()
    {
        isPause = SceneManager.sceneCount > 1;     

        if (config.gameFieldWeb != setka.enabled) {
            var cells = GameObject.FindGameObjectsWithTag("cell");
            foreach(var c in cells) {
                c.GetComponent<Image>().enabled = config.gameFieldWeb;
            }
        }
    }

    public void DefaultStart () {
        setka = GameObject.Find("Cell").GetComponent<Image>();
        
        // AddPointsToHealth(config.maxHealth);


        AddHealthPointsByTime().GetAwaiter();
        ToggleShadow(false);
        GenerateItem();
        var cell = GetRandomRespawnCell();
        if (cell) {
            Instantiate(Resources.Load<Dvornik>(Dvornik.ResourcePath), cell.transform);
        }
        var staticNips = new List<Nip>();
        staticNips.Add(Resources.Load<Valun>(Valun.ResourcePath));
        staticNips.Add(Resources.Load<Tree>(Tree.ResourcePath));
        staticNips.Add(Resources.Load<Kolodec>(Kolodec.ResourcePath));
        staticNips.Add(Resources.Load<Kust>(Kust.ResourcePath));
        foreach (var nip in staticNips) {
            var emptyCell = GetRandomCell();
            if (emptyCell) {
                Instantiate(nip, emptyCell.transform);
            }
        }
    }

    private IEnumerator MoveNips () {
        var persons = GameObject.FindObjectsOfType<Nip>();
        foreach (var p in persons) {
            (p as INip).NextStep();
            yield return new WaitForSeconds(0);
        }
    }

    public int CurrentTime () {
        var time = step % 8;
        var dayTime = GameObject.Find("DayTime");
        if (dayTime == null) throw new System.Exception("DayTime not exist");

        var indicator = time < 4 ? "sun" : "moon";
        var indicatorId = (step % 4) + 1;
        var indicatorPath = $"Indicators/{indicator}{indicatorId}";
        var sprite = Resources.Load<Sprite>(indicatorPath);
        if (sprite == null) throw new System.Exception($"{indicatorPath} not found");
        
        dayTime.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        return step % 8;
    }

    private void RenderHealth () {
        var healthObject = GameObject.Find("Health").GetComponent<Text>();
        if (healthObject == null) throw new System.Exception("Health not exists");
        healthObject.text = health.ToString();
    }

    private void RenderPoints () {
        var pointsObject = GameObject.Find("Points").GetComponent<Text>();
        if (pointsObject == null) throw new System.Exception("Points not exists");
        pointsObject.text = point.ToString();
    }

    private void IsComplete () {
        foreach (var m in missions) {
            if (!m.IsComplete()) return;
        }
        SceneManager.LoadScene(Scenes.Win.ToString());
    }

    private void IsGameOver () {
        var cells = GameObject.FindGameObjectsWithTag("cell");
        foreach (var cell in cells) {
            if (cell.transform.childCount == 0) {
                return;
            }
        }
        SceneManager.LoadScene(Scenes.GameOver.ToString());
    }

    private void ToggleShadow (bool active) {
        shadow = shadow ? shadow : GameObject.Find("Shadow");
        if (shadow == null) throw new System.Exception("Shadow GameObject is required");
        shadow.SetActive(active);
    }

    public void NextStep () {
        gameInitialized = true;
        AddPointsToHealth(-1);
        step += 1;

        var time = CurrentTime();
        
        if (time == 0) {
            ToggleShadow(false);
            var cell = GetRandomRespawnCell();
            if (cell) {
                Instantiate(Resources.Load<Dvornik>(Dvornik.ResourcePath), cell.transform);
            }
        }

        if (time == 4) {
            ToggleShadow(true);
            var cell = GetRandomRespawnCell();
            if (cell) {
                Instantiate(Resources.Load<StreetDog>(StreetDog.ResourcePath), cell.transform);
            }
        }

        StartCoroutine(MoveNips());
        AfterNextStep();
    }

    private async void AfterNextStep () {
        IsGameOver();
        IsComplete();
        
        if (health == 0) {
            await Task.Delay(1000);
            AfterNextStep();
            return;
        }

        GenerateItem();
        Dump();
    }

    public static void CreateItem (string resource) {
        var itemField = GameObject.Find("ItemField");
        if (itemField == null) throw new System.Exception("ItemField is required");
        foreach (Transform child in itemField.transform) {
            Destroy(child.gameObject);
        }
        Instantiate(Resources.Load(resource), itemField.transform);
    }

    public void GenerateItem () {
        var chance = Random.Range(0, 100);
        if (chance > 50) {
            CreateItem("Item/Hand");
            return;
        }
        CreateItem("Item/Mayka");
    }

    public Cell GetRandomRespawnCell () {
        var objects = GameObject.FindGameObjectsWithTag("cell");
        var emptyCells = new List<Cell>();
        foreach(var obj in objects) {
            var cell = obj.GetComponent<Cell>();
            if (cell && cell.isRespawn && cell.transform.childCount == 0) {
                emptyCells.Add(cell);
            }
        }
        if (emptyCells.Count == 0) return null;
        var idx = Random.Range(0, emptyCells.Count);
        return emptyCells[idx];
    }

    private Cell GetRandomCell () {
        var objects = GameObject.FindGameObjectsWithTag("cell");
        var emptyCells = new List<Cell>();
        foreach(var obj in objects) {
            var cell = obj.GetComponent<Cell>();
            if (
                !cell.isPrison &&
                !cell.isRespawn && 
                cell.transform.childCount == 0
            ) {
                emptyCells.Add(cell);
            }
        }
        if (emptyCells.Count == 0) return null;
        var idx = Random.Range(0, emptyCells.Count);
        return emptyCells[idx];
    }

    public void AddPointsToHealth (int num) {
        health += num;
        if (health > config.maxHealth) {
            health = config.maxHealth;
        }
        RenderHealth();
    }

    public void AddPointsToPoints (int num) {
        point += num;
        RenderPoints();
    }

    async Task AddHealthPointsByTime () {
        await Task.Delay(1000);
        // start add health
        if (health == 0 && timeStartAddHealth == null) {
            timeStartAddHealth = System.DateTime.UtcNow;
        }
        // end add health
        if (health == config.maxHealth) {
            timeStartAddHealth = null;
        }
        var diffTime = System.DateTime.UtcNow.Subtract(timeStartAddHealth ?? System.DateTime.UtcNow);
        var points = diffTime.TotalSeconds / config.healthPointsAtSeconds;
        var bar = points - System.Math.Truncate(points);
        var addPoints = System.Convert.ToInt32(System.Math.Truncate(points));
        RenderHealthBar(bar);
        if (addPoints > 0) {
            AddPointsToHealth(addPoints);
            timeStartAddHealth = System.DateTime.UtcNow;
        }
        await AddHealthPointsByTime();
    }

    void RenderHealthBar (double bar) {
        var health = GameObject.Find("HealthIndicator/Indicator").GetComponent<Image>();
        health.fillAmount = System.Convert.ToSingle(bar);
    }

    public void SetMission (System.Type nip, Mission.Type type, int count) {
        var mission = missions.Find(m => m.nip == nip && m.type == type);
        if (mission == null) return;

        mission.count += count;
    }

    [System.Serializable]
    public class Save {
        public string level;
        public int step;

        public int health;

        public int point;

        [SerializeField]
        private string s_timeStartAddHealth;
        public System.DateTime? timeStartAddHealth {
            get {
                if (s_timeStartAddHealth == null) return null;
                return System.Convert.ToDateTime(s_timeStartAddHealth);
            } 
            set {
                s_timeStartAddHealth = value == null ? null : value.ToString();
            }
        }

        public List<Nip.Save> nips = new List<Nip.Save>();

        public List<Mission> missions = new List<Mission>();

        public Item.Save item;
        public async void Restore () {
            // load previous level;
            var asyncLoad = SceneManager.LoadSceneAsync(level);
            SceneManager.LoadScene(Scenes.Loading.ToString(), LoadSceneMode.Additive);
            while (!asyncLoad.isDone) {
                await Task.Delay(1000);
            }
            var root = SceneManager.GetActiveScene().GetRootGameObjects();
            
            // clear game field
            foreach(var nip in GameObject.FindObjectsOfType<Nip>()) {
                Destroy(nip.gameObject);
            }
            foreach(var i in GameObject.FindObjectsOfType<Item>()) {
                Destroy(i.gameObject);
            }
            // restore controller data
            foreach (var o in root) {
                var controller = o.GetComponent<GameController>();
                if (controller) {
                    controller.step = step;
                    controller.health = health;
                    controller.point = point;
                    controller.timeStartAddHealth = timeStartAddHealth;
                    controller.missions = missions;
                    controller.RenderHealth();
                    controller.RenderPoints();
                }
            }
            // restore nips
            foreach (var n in nips) {
                n.Restore();
            }
            // restore item
            if (item != null) {
                item.Restore();
            }
            SceneManager.UnloadSceneAsync(Scenes.Loading.ToString());
        }    
    }

    public Save GetSave () {
        var save = new Save () {
            level = GetType().GetField("Level").GetValue(null) as string,
            step = step,
            health = health,
            point = point,
            timeStartAddHealth = timeStartAddHealth,
            missions = missions
        };
        foreach (var o in GameObject.FindObjectsOfType<Nip>()) {
            save.nips.Add(o.GetSave());
        }
        var item = GameObject.FindObjectOfType<Item>();
        if (item) {
            save.item = item.GetSave();
        }
        return save;
    }

    public static readonly string saveKey = "GameController";
    private void Dump () {
        var save = GetSave();
        var json = JsonUtility.ToJson(save);
        PlayerPrefs.SetString(saveKey, json);
    }

    public static void LoadLevel () {
        var save = JsonUtility.FromJson<Save>(PlayerPrefs.GetString(saveKey));
        save.Restore();
    }
}