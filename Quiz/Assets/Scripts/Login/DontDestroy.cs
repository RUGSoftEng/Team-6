/*
 * DontDestroy makes sure that the zeeguu data is not destroyed whenever
 * a new scene is loaded.
 */
using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
