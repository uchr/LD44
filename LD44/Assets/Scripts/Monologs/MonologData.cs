using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Replica {
    public string key;
    public string text;
}

[CreateAssetMenu(fileName = "MonologData", menuName = "SmallRocket/Monolog", order = 1)]
public class MonologData : ScriptableObject {
    public CharacterType character;
    public List<Replica> replics;
}
