using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public PlayerVars playerVars;
    // Start is called before the first frame update
    void Start()
    {
        playerVars.Alive = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            Debug.Log("Got 'em!");
            playerVars.Alive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerVars.Alive)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, (3.0f * Time.deltaTime));
            
        }
    }
}
