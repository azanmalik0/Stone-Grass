using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaker : MonoBehaviour
{
    public GameObject reqObject;
    public GameObject reqObject1;
    public GameObject reqObject2;
    public GameObject reqObject3;
    public Shape myLevelShape;
    public enum Shape
    {
        Single,
        Double,
        Triple,
        Quad
    }

    IEnumerator Start()
    {

        if (myLevelShape == Shape.Single)
        {

            for (int i = 0; i < transform.childCount; i++)
            {

                //Instantiate(reqObject, transform.GetChild(i).position, transform.GetChild(i).rotation;
                if (transform.GetChild(i).CompareTag("Level"))
                    Instantiate(reqObject, transform.GetChild(i));
                yield return null;
            }
        }
        else if (myLevelShape == Shape.Double)
        {
            for (int i = 0; i < transform.childCount; i++)
            {

                //Instantiate(reqObject, transform.GetChild(i).position, transform.GetChild(i).rotation;
                Instantiate(reqObject, transform.GetChild(i));
                Instantiate(reqObject1, transform.GetChild(i + 2));
                yield return null;
            }
        }
        else if (myLevelShape == Shape.Triple)
        {
            for (int i = 0; i < transform.childCount; i++)
            {

                //Instantiate(reqObject, transform.GetChild(i).position, transform.GetChild(i).rotation;
                Instantiate(reqObject2, transform.GetChild(i + 3));
                yield return null;
            }
        }
        else if (myLevelShape == Shape.Quad)
        {
            for (int i = 0; i < transform.childCount; i++)
            {

                //Instantiate(reqObject, transform.GetChild(i).position, transform.GetChild(i).rotation;
                Instantiate(reqObject, transform.GetChild(i + 4));
                yield return null;
            }
        }

    }
}
