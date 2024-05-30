using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    public float jumpPower;
    private void OnCollisionEnter(Collision coll)
    {
        coll.rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
}
