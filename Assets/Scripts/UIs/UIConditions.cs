using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Condition Health;
    public Condition Hunger;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.Player.cond.uiCondition = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
