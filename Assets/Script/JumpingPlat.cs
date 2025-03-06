using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlat : MonoBehaviour
{
    public float jumpPower;
    
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody _rigidbody = collision.rigidbody.GetComponent<Rigidbody>();

        if(_rigidbody != null)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.y, jumpPower);
        }
    }
}
