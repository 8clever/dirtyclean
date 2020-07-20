using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mayka : Item, IItem
{
    public static string resourcePath = "Item/Mayka";
    public void OnDrop(GameObject cell)
    {
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
