using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class AddItemEvent : UnityEvent<string, int> {
}

public class RemoveItemEvent : UnityEvent<string, int> {
}

public class Inventory : MonoSingleton<Inventory> {
    [Header("Items")]
    public AddItemEvent addItem = new AddItemEvent();
    public RemoveItemEvent removeItem = new RemoveItemEvent();

    [Header("UI")]
    public GameObject detail0;
    public GameObject detail1;
    public GameObject detail2;
    public GameObject disk;
    public TextMeshProUGUI diskCount;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string itemName) {
        if (!items.ContainsKey(itemName))
            items.Add(itemName, 0);
        items[itemName]++;

        if (itemName == "Detail0")
            detail0.SetActive(true);
        if (itemName == "Detail1")
            detail1.SetActive(true);
        if (itemName == "Detail2")
            detail2.SetActive(true);
        if (itemName == "Disk") {
            disk.SetActive(true);
            diskCount.text = items["Disk"].ToString();
        }

        addItem.Invoke(itemName, items[itemName]);
    }

    public int Contains(string itemName) {
        if (!items.ContainsKey(itemName))
            return 0;

        return items[itemName];
    }

    public void RemoveItem(string itemName) {
        int count = 0;
        if (items.ContainsKey(itemName)) {
            items[itemName]--;
            count = items[itemName];
        }

        if (itemName == "Detail0")
            detail0.SetActive(false);
        if (itemName == "Detail1")
            detail1.SetActive(false);
        if (itemName == "Detail2")
            detail2.SetActive(false);
        if (itemName == "Disk")
        {
            if (count == 0)
                disk.SetActive(false);
            else 
                diskCount.text = items["Disk"].ToString();
        }

        if (items[itemName] <= 0)
            items.Remove(itemName);

        removeItem.Invoke(itemName, count);
    }
}
