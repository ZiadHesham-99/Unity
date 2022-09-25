using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] private Transform groundcheck = null;
    [SerializeField] private LayerMask playerMask;
    private Rigidbody rigidBody; 
    private bool jumpKeyIsPressed = false;
    private float horizontalAxis = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    } 

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        if (Physics.OverlapSphere(groundcheck.position,0.1f,playerMask).Length == 0)
        {
            return;
        }
        //check for space key is pressed to jump up 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyIsPressed  = true ;
        }
        
    }
    //updated every hysics engine update 100 Hz
    private void FixedUpdate()
    {
        if (jumpKeyIsPressed)
        {
            rigidBody.AddForce(Vector3.up * 6.5f, ForceMode.VelocityChange);
            jumpKeyIsPressed = false ;
        }
        rigidBody.velocity = new Vector3(horizontalAxis* (1.5f), rigidBody.velocity.y, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
        }
    }
}
