using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FindMissingScripts
{

    [MenuItem("Tools / Missing Scripts / " + "Find")]
    static void FindMissingScriptsMenuItem()
    {
        foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
        {
            foreach (Component component in gameObject.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    Debug.Log($"Gameobject found with missing script {gameObject.name}", gameObject);
                    break;
                }

            }

        }
    }

    [MenuItem("Tools / Missing Scripts / " + "Delete")]
    static void DeleteMissingScriptsMenuItem()
    {
        foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
        {
            foreach (Component component in gameObject.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                    break;
                }

            }

        }
    }

}
