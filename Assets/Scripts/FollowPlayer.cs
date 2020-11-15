using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class FollowPlayer : MonoBehaviour
{
    //place this script on the player gameobject
 
    public GameObject player; // in the inspector drag the gameobject the will be following the player to this field
    public int followDistance;
    private List<Vector3> storedPositions;
 
 
    void Awake()
    {
        storedPositions = new List<Vector3>(); //create a blank list
 
        if (!player)
        {
            Debug.Log("The FollowingMe gameobject was not set");
        }
 
        if (followDistance == 0)
        {
            Debug.Log("Please set distance higher then 0");
        }
    }
 
    void Start()
    {
 
    }
 
    void Update()
    {
        if(storedPositions.Count == 0)
        {
            Debug.Log("blank list");
            storedPositions.Add(player.transform.position); //store the players currect position
            return;
        }else if (storedPositions[storedPositions.Count -1] != player.transform.position)
        {
            //Debug.Log("Add to list");
            storedPositions.Add(player.transform.position); //store the position every frame
        }
 
        if (storedPositions.Count > followDistance)
        {
            transform.position = storedPositions[0]; //move
            storedPositions.RemoveAt(0); //delete the position that player just move to
        }
    }
}