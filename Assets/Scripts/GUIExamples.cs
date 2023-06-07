using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIExamples : MonoBehaviour
{
    private int currentIndexGUI;
    private void OnGUI() {

        var toolbarRect = new Rect(Screen.width/2f, Screen.height/2f, 200, 15);

        currentIndexGUI = GUI.Toolbar(toolbarRect, currentIndexGUI, new string[2] {
            "GUI",
            "GUIlayout"
        });

        switch(currentIndexGUI) {
            case 0:
                var topLeft = new Rect(0, 0, 100, 50);
                GUI.Box(topLeft, "Arriba izqiuerda");

                var buttonLeft = new Rect(0, Screen.height-50, 100, 50);
                GUI.Box(buttonLeft, "Abajo izquierda");

                var topRight = new Rect(Screen.width-100, 0, 100, 50);
                GUI.Box(topRight, "Arriba derevcha");

                var buttonRight = new Rect(Screen.width-100, Screen.height-50, 100, 50);
                GUI.Box(buttonRight, "Abajo derecha");
                break;
            case 1:
                var textButton = new GUIContent("Hola Mundo");
                if (GUILayout.Button(textButton)) {
                    Debug.Log("Boton presionado");
                };
                break;
        };

        /* 
        var textButton = new GUIContent("Hola Mundo");
        var rectButton = new Rect(Screen.height % 2f, Screen.width % 2f, 100, 50);
        if (GUI.Button(rectButton, textButton)) {
            Debug.Log("Boton presionado");
        }; */
    }
}