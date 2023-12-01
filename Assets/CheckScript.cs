using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckScript : MonoBehaviour
{
    [SerializeField] GameObject feedCell;
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(feedCell);

        }

    }

}
