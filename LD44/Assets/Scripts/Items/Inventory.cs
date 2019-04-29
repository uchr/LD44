﻿using System.Collections;
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
    public TextMeshProUGUI listOfItems;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string itemName) {
        if (!items.ContainsKey(itemName))
            items.Add(itemName, 0);
        items[itemName]++;

        addItem.Invoke(itemName, items[itemName]);

        listOfItems.text = "<b>ITEMS<b>\n";
        foreach (var item in items) {
            listOfItems.text += "<b>" + item.Key + ":</b> " + item.Value.ToString() + "\n";
        }
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

        if (items[itemName] <= 0)
            items.Remove(itemName);
        
        removeItem.Invoke(itemName, count);
    }
}
