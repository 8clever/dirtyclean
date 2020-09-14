using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : GameController
{
    public static readonly string Level = Levels.lvl1.ToString();
    private void Awake() {
        missions.Add(new Mission(typeof(Musorka), Mission.Type.Destroy, 20));
        missions.Add(new Mission(typeof(Dvornik), Mission.Type.Collect, 10));
        missions.Add(new Mission(typeof(Buldozer), Mission.Type.Create, 10));
        missions.Add(new Mission(typeof(GreenPeace), Mission.Type.Create, 5));
        missions.Add(new Mission(typeof(President), Mission.Type.Create, 1));
        DefaultAwake();
    }
    void Start()
    {
        DefaultStart();
    }

    // Update is called once per frame
    void Update()
    {
        DefaultUpdate();
    }
}
