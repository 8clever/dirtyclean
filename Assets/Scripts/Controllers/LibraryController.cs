using UnityEngine;
using System.Collections.Generic;

public class LibraryController : MonoBehaviour {
    [System.Serializable]
    public class Item {
        public string Text;
        public Sprite Image;
    }
    public List<Item> Items;
    public GameObject Container;
    public LibraryItem Factory;
    private void Start() {
        foreach(var i in Items) {
            var obj = Instantiate(Factory, Container.transform);
            obj.Set(i.Image, i.Text);
        }
    }    
}