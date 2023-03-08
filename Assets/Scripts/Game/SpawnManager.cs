using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private Transform[] spawn_point;
    void Start()
    {
        PlayerBox local = PlayerBox.localPlayer;
        int index_spawn = Random.Range(0, spawn_point.Length);
        local.transform.position = spawn_point[index_spawn].position;
    }

    public void ReSpawn()
    {
        PlayerBox local = PlayerBox.localPlayer;
        int index_spawn = Random.Range(0, spawn_point.Length);
        local.transform.position = spawn_point[index_spawn].position;
    }

}
