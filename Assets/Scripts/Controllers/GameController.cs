using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    public int point = 0;
    public bool isPause = false;
    public Scenes Level;
    public bool gameInitialized = false;
    public List<Mission> missions = new List<Mission>();
    public List<Item> GenerateItems = new List<Item>();
    public List<Nip> GenerateNipsOnStart = new List<Nip>();
    public Nip GenerateNipOnMorning;
    public Nip GenerateNipOnNight;
    public List<Nip> missionNips = new List<Nip>();
    private int step = 0;
    private int maxHealth = 150;
    public int health = 0;
    private System.DateTime? timeStartAddHealth = null;
    private GameObject shadow = null;
    private Config config = null;
    private bool IsGameEnd = false;
    private int currentGrade = 1;
    public int stars = 0;
    private void Awake() {
        DefaultAwake();
    }

    public void DefaultAwake () {
        config = Config.GetConfig();
    }

    private void Update () {
        DefaultUpdate();
    } 
    public void DefaultUpdate()
    {
        isPause = SceneManager.sceneCount > 1;     
    }

    private void Start () {
        DefaultStart();
    }

    public void DefaultStart () {
        AddPointsToHealth(maxHealth);
        AddHealthPointsByTime().GetAwaiter();
        ToggleShadow(false);
        GenerateItem();
        foreach (var nip in GenerateNipsOnStart) {
            var emptyCell = GetRandomCell();
            if (emptyCell) {
                Instantiate(nip, emptyCell.transform);
            }
        }
        GenerateMission();
        GenerateMission();
        GenerateMission();
    }

    private Nip GetRandomNip () {
        var rnd = new System.Random();
        var nips = missionNips.FindAll(i => i.Grade <= currentGrade);
        var availableNips = nips.FindAll(n => {
            var exists = missions.Find(m => m.Nip.GetName() == n.GetName());
            return exists == null;
        });
        Nip rndNip = availableNips[rnd.Next(availableNips.Count)];
        return rndNip;
    }

    private void GenerateMission () {
        var rnd = new System.Random();
        var types = System.Enum.GetValues(typeof(Mission.Type));
        var rndType = (Mission.Type)types.GetValue(rnd.Next(types.Length));
        var rndNip = GetRandomNip();

        Mission mission = new Mission() {
            type = rndType,
            requiredCount = Random.Range(1, 10),
            Nip = rndNip
        };
        missions.Add(mission);
    }

    private void MoveNips () {
        var persons = GameObject.FindObjectsOfType<Nip>();
        foreach (var p in persons) {
            (p as INip)?.NextStep();
            (p as NpcMain)?.NextStep();
        }
    }

    public int RenderCurrentTime () {
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
        if (Level == Scenes.endlessMode) return;
        foreach (var m in missions) {
            if (!m.IsComplete()) return;
        }
        IsGameEnd = true;
        PlayerPrefs.SetString(Level.ToString(), "done");
        SceneManager.LoadScene(Scenes.Win.ToString(), LoadSceneMode.Additive);
    }

    private void IsGameOver () {
        var gameObjects = GameObject.FindGameObjectsWithTag("cell");
        if (gameObjects.Length == 0) return;

        foreach (var go in gameObjects) {
            var cell = go.GetComponent<Cell>();
            if (cell.isPrison) continue;
            if (cell.transform.childCount == 0) {
                return;
            }
        }
        
        IsGameEnd = true;
        SceneManager.LoadScene(Scenes.GameOver.ToString(), LoadSceneMode.Additive);
    }

    private void ToggleShadow (bool active) {
        shadow = shadow ?? GameObject.Find("Shadow");
        shadow?.SetActive(active);
    }

    public void NextStep () {
        gameInitialized = true;
        AddPointsToHealth(-1);
        step += 1;

        if (health == 0) {
            SceneManager.LoadSceneAsync(Scenes.NeedTurns.ToString(), LoadSceneMode.Additive);
        }

        var time = RenderCurrentTime();
        
        var halfTime = time % 2;

        var cell = GetRandomRespawnCell();

        if (cell) {
            Instantiate(
                halfTime == 0 ?
                GenerateNipOnMorning :
                GenerateNipOnNight, 
                cell.transform
            );
        }
        

        if (time == 0) {
            ToggleShadow(false);
        }

        if (time == 4) {
            ToggleShadow(true);
        }

        MoveNips();
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

        // dump after complete move
        await Task.Delay(System.Convert.ToInt32(config.nip.moveSpeed) * 1000);
        Dump();
    }

    public static void CreateItem (Item resource) {
        var itemField = GameObject.Find("ItemField");
        if (itemField == null) throw new System.Exception("ItemField is required");
        foreach (Transform child in itemField.transform) {
            Destroy(child.gameObject);
        }
        Instantiate(resource, itemField.transform);
    }

    public void GenerateItem () {
        if (GenerateItems.Count == 0) {
            throw new System.Exception("Items for generator not assigned to GameController");
        }
        var idx = Random.Range(0, GenerateItems.Count);
        CreateItem(GenerateItems[idx]);
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
        if (health > maxHealth) {
            health = maxHealth;
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
        if (health == maxHealth) {
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

    public void SetMission (Nip nip, Mission.Type type, int count) {
        var mission = missions.Find(m => m.Nip.GetName() == nip.GetName() && m.type == type);
        if (mission == null) return;

        if (type == Mission.Type.Collect) {
            var allNips = new List<GameObject>(GameObject.FindGameObjectsWithTag("nip"));
            var nips = allNips.FindAll(n => {
                return n.GetComponent<Nip>().GetName() == nip.GetName();
            });
            mission.count = nips.Count;
        }

        mission.count += count;
        if (mission.count < 0) mission.count = 0;
        if (mission.IsComplete()) {
            var anim = Resources.Load<GameObject>("Animations/MissionComplete");
            var canvas = GameObject.Find("Canvas");
            Instantiate(anim, canvas.transform);
            stars += 1;
            var newGrade = mission.Nip.Grade + 1;
            if (newGrade > currentGrade) currentGrade = newGrade;
            missions.Remove(mission);
            GenerateMission();
            var starController = FindObjectOfType<StarController>();
            if (starController) {
                starController.clickable = true;
            }
        }
    }

    [System.Serializable]
    public class Save {
        public string level;
        public int step;
        public int health;
        public int point;
        public int grade;
        public int stars;
        [SerializeField]
        private string s_timeStartAddHealth = string.Empty;
        public System.DateTime? timeStartAddHealth {
            get {
                if (s_timeStartAddHealth == string.Empty) {
                    return null;
                }
                return System.Convert.ToDateTime(s_timeStartAddHealth);
            } 
            set {
                s_timeStartAddHealth = value?.ToString() ?? string.Empty;
            }
        }

        public List<Nip.Save> nips = new List<Nip.Save>();

        public List<Mission> missions = new List<Mission>();
        public Item.Save item = null;
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
            // restore nips
            foreach (var n in nips) {
                n.Restore();
            }
            // restore controller data
            var controller = FindObjectOfType<GameController>();
            controller.step = step;
            controller.health = health;
            controller.point = point;
            controller.timeStartAddHealth = timeStartAddHealth;
            controller.missions = missions;
            controller.currentGrade = grade;
            controller.stars = stars;
            controller.RenderHealth();
            controller.RenderPoints();
            controller.RenderCurrentTime();

            Debug.Log(grade);

            // restore item;
            if (item?.ResourcePath == string.Empty) {
                controller.AfterNextStep();
            } else {
                item.Restore();
            }
            
            SceneManager.UnloadSceneAsync(Scenes.Loading.ToString());
        }    
    }

    public Save GetSave () {
        var save = new Save () {
            level = Level.ToString(),
            step = step,
            health = health,
            point = point,
            timeStartAddHealth = timeStartAddHealth,
            missions = missions,
            grade = currentGrade,
            stars = stars
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
        if (IsGameEnd) {
            PlayerPrefs.DeleteKey(saveKey);
            return;
        }
        var save = GetSave();
        var json = JsonUtility.ToJson(save);
        PlayerPrefs.SetString(saveKey, json);
    }

    public static void LoadLevel () {
        var json = PlayerPrefs.GetString(saveKey);
        var save = JsonUtility.FromJson<Save>(json);
        save.Restore();
    }
}