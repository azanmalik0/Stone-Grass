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

            InstantiatePrefabs(prefabs[i], childTransforms, startIndex, endIndex);
        }
    }

    void InstantiatePrefabs(GameObject prefab, Transform[] positions, int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            Instantiate(prefab, positions[i].position, Quaternion.identity);
        }
    }
}