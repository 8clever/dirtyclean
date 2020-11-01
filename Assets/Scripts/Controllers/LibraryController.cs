using UnityEngine;
using System.Collections.Generic;

public class LibraryController : MonoBehaviour {
    [System.Serializable]
    public class Item {
        public string Text;
        public Sprite Image;
        public int Order;
    }
    public List<Item> Items;
    public GameObject Container;
    public LibraryItem Factory;
    private void Start() {
        Items.Sort((a, b) => {
            return a.Order.CompareTo(b.Order); 
        });
        foreach(var i in Items) {
            var obj = Instantiate(Factory, Container.transform);
            obj.Set(i.Image, i.Text);
        }
    }    
}