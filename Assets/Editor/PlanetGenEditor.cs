using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetGenerator)), CanEditMultipleObjects]
public class PlanetGeneratorEditor : Editor
{
    Editor noiseEditor;
    Editor colourEditor;
    Editor shapeEditor;
    public override void OnInspectorGUI()
    {
        PlanetGenerator planetGen = (PlanetGenerator)target;
        if (DrawDefaultInspector())
        {
            if (planetGen.autoUpdate)
            {
                planetGen.initMesh();
            }
        };
        if (GUILayout.Button("Generate"))
        {
            planetGen.initMesh();
        };
        DrawSettingsEditor(planetGen.NoiseSettings, planetGen.onNoiseSettingsUpdate, ref planetGen.showNoiseSettings, ref noiseEditor);
        DrawSettingsEditor(planetGen.colourSettings, planetGen.onColorChange, ref planetGen.showcolourSettings, ref colourEditor);
        DrawSettingsEditor(planetGen.shapeSettings, planetGen.onUpdateShapeSettings, ref planetGen.showShapeSettings, ref colourEditor);
    }
    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

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
}
