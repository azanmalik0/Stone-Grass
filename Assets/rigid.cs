using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class rigid : MonoBehaviour
{
    //public GameObject prefab1; // Assign your first prefab in the Inspector
    //public GameObject prefab2; // Assign your second prefab in the Inspector

    //void Start()
    //{
    //    Transform[] childTransforms = new Transform[transform.childCount];

    //    // Store child positions in an array
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        childTransforms[i] = transform.GetChild(i);
    //    }

    //    // Instantiate prefabs to child positions equally
    //    for (int i = 0; i < childTransforms.Length; i++)
    //    {
    //        GameObject prefabToInstantiate = (i % 2 == 0) ? prefab1 : prefab2; // Alternating between prefab1 and prefab2
    //        Instantiate(prefabToInstantiate, childTransforms[i].position, Quaternion.identity);
    //    }
    //}


    //public GameObject prefab1; // Assign your first prefab in the Inspector
    //public GameObject prefab2; // Assign your second prefab in the Inspector
    //public GameObject prefab3; // Assign your third prefab in the Inspector

    //void Start()
    //{
    //    Transform[] childTransforms = new Transform[transform.childCount];

    //    // Store child positions in an array
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        childTransforms[i] = transform.GetChild(i);
    //    }

    //    int partSize = Mathf.CeilToInt(childTransforms.Length / 3.0f);

    //    // Instantiate prefabs to child positions in sequential parts
    //    InstantiatePrefabs(prefab1, childTransforms, 0, partSize);
    //    InstantiatePrefabs(prefab2, childTransforms, partSize, partSize * 2);
    //    InstantiatePrefabs(prefab3, childTransforms, partSize * 2, childTransforms.Length);
    //}

    //void InstantiatePrefabs(GameObject prefab, Transform[] positions, int startIndex, int endIndex)
    //{
    //    for (int i = startIndex; i < endIndex; i++)
    //    {
    //        Instantiate(prefab, positions[i].position, Quaternion.identity);
    //    }
    //}





    public GameObject[] prefabs; // Assign your prefabs in the Inspector

    void Start()
    {
        Transform[] childTransforms = new Transform[transform.childCount];

        // Store child positions in an array
        for (int i = 0; i < transform.childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }

        int partSize = Mathf.CeilToInt(childTransforms.Length / (float)prefabs.Length);

        // Instantiate prefabs to child positions in sequential parts
        for (int i = 0; i < prefabs.Length; i++)
        {
            int startIndex = i * partSize;
            int endIndex = Mathf.Min((i + 1) * partSize, childTransforms.Length);

            InstantiatePrefabs(prefabs[i], childTransforms, startIndex, endIndex, this.transform);
        }
    }

    void InstantiatePrefabs(GameObject prefab, Transform[] positions, int startIndex, int endIndex, Transform transform)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            Instantiate(prefab, positions[i].position, Quaternion.identity, transform);
        }
    }


    [ContextMenu("Sort")]
    public void SortChildren()
    {
        // Get all child objects
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // Custom sorting method considering numeric values in the names
        Array.Sort(children, (t1, t2) =>
        {
            string name1 = t1.name;
            string name2 = t2.name;

            // Extract numeric parts using regular expressions
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

            // If numeric parts are the same, compare full strings
            return name1.CompareTo(name2);
        });

        // Reorder the children in the hierarchy
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
    //public void SortChildren()
    //{
    //    // Get all child objects
    //    Transform[] children = new Transform[transform.childCount];
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        children[i] = transform.GetChild(i);
    //    }

    //    // Sort the children based on their names
    //    System.Array.Sort(children, (t1, t2) => t1.name.CompareTo(t2.name));

    //    // Reorder the children in the hierarchy
    //    for (int i = 0; i < children.Length; i++)
    //    {
    //        children[i].SetSiblingIndex(i);
    //    }
    //}
}