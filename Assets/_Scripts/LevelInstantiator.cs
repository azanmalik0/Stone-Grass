using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelInstantiator : MonoBehaviour
{
    public GameObject[] cropCellPrefabs;

    void Start()
    {
        Transform[] childTransforms = new Transform[transform.childCount-1];

        for (int i = 0; i < transform.childCount-1; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }

        int partSize = Mathf.CeilToInt(childTransforms.Length / (float)cropCellPrefabs.Length);

        for (int i = 0; i < cropCellPrefabs.Length; i++)
        {
            int startIndex = i * partSize;
            int endIndex = Mathf.Min((i + 1) * partSize, childTransforms.Length);

                InstantiatePrefabs(cropCellPrefabs[i], childTransforms, startIndex, endIndex, this.transform);
        }
    }

    void InstantiatePrefabs(GameObject prefab, Transform[] positions, int startIndex, int endIndex, Transform transform)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            Instantiate(prefab, positions[i].position, Quaternion.identity, transform);
        }
    }
    [ContextMenu("Instant")]
    public void Azan()
    {
        Transform[] childTransforms = new Transform[transform.childCount - 1];

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }

        int partSize = Mathf.CeilToInt(childTransforms.Length / (float)cropCellPrefabs.Length);

        for (int i = 0; i < cropCellPrefabs.Length; i++)
        {
            int startIndex = i * partSize;
            int endIndex = Mathf.Min((i + 1) * partSize, childTransforms.Length);

            InstantiatePrefabs(cropCellPrefabs[i], childTransforms, startIndex, endIndex, this.transform);
        }
    }

    [ContextMenu("Sort")]
    public void SortChildren()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        Array.Sort(children, (t1, t2) =>
        {
            string name1 = t1.name;
            string name2 = t2.name;

            string pattern = @"\d+";
            MatchCollection matches1 = Regex.Matches(name1, pattern);
            MatchCollection matches2 = Regex.Matches(name2, pattern);

            // Compare numeric parts as integers
            for (int i = 0; i < Math.Min(matches1.Count, matches2.Count); i++)
            {
                int num1 = int.Parse(matches1[i].Value);
                int num2 = int.Parse(matches2[i].Value);

                if (num1 != num2)
                {
                    return num1.CompareTo(num2);
                }
            }

            return name1.CompareTo(name2);
        });

        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
}
