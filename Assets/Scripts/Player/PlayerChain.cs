using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPlayer : MonoBehaviour
{
    public float jointFrequency = 1f;
    public float jointDampingRatio = 0.5f;

    private GameObject[] players;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        
        // Assurez-vous que vous avez au moins deux joueurs pour créer un joint.
        if (players.Length >= 2)
        {
            lastPlayer = players[0];
        }
    }

    GameObject lastPlayer;

    void Update()
    {
        foreach (GameObject player in players)
        {
            if (player != lastPlayer)
            {
                CreateJoint(lastPlayer, player);
                lastPlayer = player;
            }
        }
    }

    void CreateJoint(GameObject lPlayerOne, GameObject lPlayerTwo)
    {
        if (lPlayerOne != null && lPlayerTwo != null)
        {
            SpringJoint2D joint = lPlayerOne.GetComponent<SpringJoint2D>();
            if (joint == null)
            {
                joint = lPlayerOne.AddComponent<SpringJoint2D>();
                joint.frequency = jointFrequency;
                joint.dampingRatio = jointDampingRatio;
                joint.connectedBody = lPlayerTwo.GetComponent<Rigidbody2D>();
            }
        }
    }
}
