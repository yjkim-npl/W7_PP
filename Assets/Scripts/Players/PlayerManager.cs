using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance 
    {
        get 
        {
            if(instance == null)
                instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            return instance; 
        } 
    }
    private Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
