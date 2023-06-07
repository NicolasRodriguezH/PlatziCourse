using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemEditorWindow : EditorWindow 
{
    public List<ItemScriptableObject> items;
    ItemScriptableObject currentItem;

    [MenuItem("Tools/ItemEditorWindow")]
    static void OpenWindow()
    {
        var window = GetWindow<ItemEditorWindow>("Editor de items");
        window.position = new Rect(Screen.width/2f ,Screen.height/2f, 500, 500);
        //window.titleContent = new GUIContent("ItemOfWindow");
        //window.Show();
    }

    private void OnEnable() 
    {
        var guids = AssetDatabase.FindAssets("t:ItemScriptableObject");
        items = new List<ItemScriptableObject>();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
            items.Add(asset);
        }
    }

    /* [MenuItem("PlatziCourse/ItemWindow")]
    private static void ShowWindow() {
        var window = GetWindow<ItemEditorWindow>();
        window.titleContent = new GUIContent("ItemWindow");
        window.Show();
    } */

    Vector2 scrollPosition;
    private void OnGUI() 
    {
        var scrollRect = new Rect(0, 0, 220, Screen.height);    
        GUILayout.BeginArea(scrollRect);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
        foreach (var item in items)
        {
            Texture iconText = null;
            if (item.item.icon != null) 
            {
                iconText = item.item.icon.texture;
            }
            if (GUILayout.Button(new GUIContent(item.item.displayname, iconText), GUILayout.Width(200), GUILayout.Height(50)))
            {
                currentItem = item;
                GUI.FocusControl(currentItem.item.description);
            }            
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        if(currentItem == null) return;

        var itemEditRect = new Rect();
        itemEditRect.position = new Vector2(scrollRect.xMax, scrollRect.yMin);
        itemEditRect.size = new Vector2(Screen.width - scrollRect.width - Screen.width / 3f, Screen.height - 200);

        //GUI.Button(itemEditRect, "Test");

        GUILayout.BeginArea(itemEditRect);

        var so = new SerializedObject(currentItem);
        GUI.enabled = false;
        EditorGUILayout.PropertyField(so.FindProperty("item"));
        GUI.enabled = true;
        EditorGUI.BeginChangeCheck();
        var price = EditorGUILayout.IntSlider("Price:", currentItem.item.price, 100, 1000);
        GUILayout.Label("Decription");
        var desc = EditorGUILayout.TextArea(currentItem.item.description, EditorStyles.textArea);
        if(EditorGUI.EndChangeCheck()) 
        {
            Undo.RecordObject(currentItem, "La propiedad ha cambiado");
            currentItem.item.price = price;
            currentItem.item.description = desc;
            hasUnsavedChanges = true;

            EditorUtility.SetDirty(currentItem);
        }

        GUILayout.EndArea();
    }
}