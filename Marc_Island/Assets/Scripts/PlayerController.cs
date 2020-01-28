using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    public float speed;
    GameObject focalPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRB.AddForce(Vector3.forward * speed * forwardInput * Time.deltaTime);
        // now we need the player to move in the direction of the camera so..."
        playerRB.AddForce(focalPoint.transform.forward * forwardInput * speed * Time.deltaTime);

    }
}
