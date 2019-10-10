using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor  //This is being written so that we can edit the planet#s appearance from the planet selection itself
{
    Planet planet;  //Reference to planet,

    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI() //Override Gui, so aprt from base it shows shaoe and color settings as well
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)  //foldout bool used to determine if it can be folded or not from the inspector , needs to be passed by reference so that oit can be accesed from planet class
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);   //Shows the settings separately so it's easier for the user

            using (var check = new EditorGUI.ChangeCheckScope())
            {

                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor); //Create new editor only when we have to, othereise refernce the one creart3ed in the begining of the file
                                                                    //Editor editor = CreateEditor(settings);
                    editor.OnInspectorGUI();    //Makes GUI show on inspector



                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }

    }

    private void OnEnable()
    {
        planet = (Planet)target;    //CAst the target object to p[lanet]
    }

}
