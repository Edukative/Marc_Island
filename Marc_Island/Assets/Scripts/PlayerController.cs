using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool hasPowerup;
    public float powerupStrength = 15;
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

   /* IEnumerator PowerupCountdownRoutine()
    {


    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Power Up"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRB = other.gameObject.GetComponent<Rigidbody>();
            Vector3 pushAwayEnemy = (other.gameObject.transform.position - transform.position);
            Debug.Log("Player collied with" + other.gameObject + " with powerup set to " + hasPowerup);
            enemyRB.AddForce(pushAwayEnemy * powerupStrength, ForceMode.Impulse);
        }
    }
}
