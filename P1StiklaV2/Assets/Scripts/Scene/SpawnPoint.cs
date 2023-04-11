using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject Player;
    void Start()
    {
        Player.transform.position=transform.position;
    }
}
