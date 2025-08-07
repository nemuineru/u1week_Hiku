using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luminus : MonoBehaviour
{
    public GameObject LuminusParticlePrefab;
    Rigidbody rBody;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject InitParticle = Instantiate(LuminusParticlePrefab, transform.position, Quaternion.identity);
            InitParticle.SetActive(true);
            GameSystem.self.CurrentLuminus++;
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.velocity = new Vector3(Random.Range(-1f,1f), 0f ,Random.Range(-1f,1f)).normalized * Random.Range(4.0f, 8.0f);
        transform.rotation = transform.rotation * Quaternion.Euler(0f, Random.Range(-180f,180f), 0f);
        rotateSpeed = Random.Range(0.8f, 10f);
    }

    float rotateSpeed = 0f;

    float PlayerDetectionDist = 16.0f;
    float minPlayerDetectionDist = 8.0f;

    public float speed = 3.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dist = (GameSystem.Player.transform.position - transform.position);
        if(dist.magnitude < PlayerDetectionDist && GameSystem.Player.gameObject.activeSelf)
        {
            rBody.AddForce(dist.normalized * speed * Mathf.Min(2.0f, minPlayerDetectionDist / dist.magnitude));
        }
        transform.rotation = transform.rotation * Quaternion.Euler(0f, rotateSpeed, 0f);
    }
}
