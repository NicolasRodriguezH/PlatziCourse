using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "Iteme", menuName = "Tools/ScriptablesItems")] 

public class ItemScriptableObject : ScriptableObject
{
    [SerializeField] public Item item;
    private string path => Application.dataPath + "/itemPreset.json";
    [ContextMenu("ToJson")]

    private void SerializaOnJson() 
    {
        var json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
        //Actualizams ddbb de Unity - using UnityEditor;
        AssetDatabase.Refresh();
    }
    [ContextMenu("Load Json")]
    private void LoadJson()
    {
        if(File.Exists(path)) 
        {
            var json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
[System.Serializable]

public struct Item 
{
    public string displayname;
    public ItemType item;
    public int price;
    public string description;
    public Sprite icon;
    public float damage;
    [Min(0)]public int restoreAmount;
    public RandonClass rand;
}

public enum ItemType
{
    WEAPON,
    POTION
}

[System.Serializable]
public class RandonClass
{
    // Se serializan los publicos mas no los privados
    [SerializeField] private int min;
    [SerializeField] private int max;
    public bool value;

}