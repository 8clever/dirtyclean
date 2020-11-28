using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMain : Nip
{
    public AudioClip audioCatchEnemy;
    public bool CanDrag = true;
    public bool DisableMove = false;

    [System.Serializable]
    public class Move {
        public Nip To;
        public Nip ChangeTo;
    }
    public List<Move> Moves = new List<Move>();
    public int AddPointsOnCreate = 0;
    public Nip Synthesis = null;
    public void NextStep () 
    {
        if (DisableMove) return;   
        MoveToNips(Moves.ConvertAll<Nip>(move => move.To));
    }
    private void Awake()
    {
        DefAwake();
        AddPoints(AddPointsOnCreate);
    }
    public void OnCollision(Collision collision)
    {
        foreach(var move in Moves) {
            var nip = collision.gameObject.GetComponentInParent<Nip>();
            if (move.To.GetName() == nip?.GetName()) {
                if (audioCatchEnemy) {
                    MusicController.PlayOnce(audioCatchEnemy);
                }
                var anim = Resources.Load<GameObject>("Animations/FightNip");
                Instantiate(anim, nip.transform.parent);
                AddPoints(1);
                Destroy(nip.gameObject);
                if (move.ChangeTo) {
                    var newNip = Instantiate(move.ChangeTo, transform.parent);
                    newNip.MoveToClosestEmptyCell();
                }
            }
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell && Synthesis) {
            var nip = cell.GetComponentInChildren<Nip>();
            if (nip?.GetName() == GetName()) {
                Destroy(nip.gameObject);
                Destroy(this.gameObject);
                Instantiate(Synthesis, cell.transform);
                GameNextStep();
                return;
            }
        }
        
        OnDropDefault(cell);
    }

    public bool CanDrop(Cell cell)
    {
        var nip = cell.GetComponentInChildren<Nip>();
        if (Synthesis && nip?.GetName() == GetName()) return true;
        return CanDropDefault(cell);
    }
}
