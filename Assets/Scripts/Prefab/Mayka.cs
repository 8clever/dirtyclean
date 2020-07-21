using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mayka : Item, IItem
{
    public static string resourcePath = "Item/Mayka";
    public void OnDrop(GameObject cell)
    {
        var c = cell.GetComponent<Cell>();
        if (c.isPrison) {
            MoveToItemField();
            return;
        }

        if (cell && cell.transform.childCount == 0) {
            Destroy(this.gameObject);
            Instantiate(Resources.Load<Musorka>(Musorka.resourcePath), cell.transform);
            NextStep();
            return;
        }

        var dog = cell.GetComponentInChildren<StreetDog>();
        if (dog) {
            Destroy(this.gameObject);
            Destroy(dog.gameObject);
            Instantiate(Resources.Load<HomeDog>(HomeDog.resourcePath), cell.transform);
            NextStep();
            return;   
        }

        var bomj = cell.GetComponentInChildren<Bomj>();
        if (bomj) {
            Destroy(this.gameObject);
            Destroy(bomj.gameObject);
            Instantiate(Resources.Load<Dvornik>(Dvornik.resourcePath), cell.transform);
            NextStep();
            return;
        }

        var musorka = cell.GetComponentInChildren<Musorka>();
        if (musorka) {
            Destroy(this.gameObject);
            Destroy(musorka.gameObject);
            Instantiate(Resources.Load<Svalka>(Svalka.resourcePath), cell.transform);
            NextStep();
            return;
        }

        MoveToItemField();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
