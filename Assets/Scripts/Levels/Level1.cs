using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : GameController
{
    // Start is called before the first frame update
    private void Awake() {
        missions.Add(new Mission("Clean 20 cells with garbage").AddMission(Mission.Association.DestroyMusor, 20));
        missions.Add(new Mission("Collect 10 street workers at the same time").AddMission(Mission.Association.CollectDvornik, 10));
        missions.Add(new Mission("Create 10 buldozers").AddMission(Mission.Association.CreateBuldozers, 10));
        missions.Add(new Mission("Create 5 green peace").AddMission(Mission.Association.CreateGreenPeace, 5));
        missions.Add(new Mission("Create 1 president").AddMission(Mission.Association.CreatePresident, 1));
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
