using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float AmmoSpeed = 10f; // Kecepatan projectile
    [HideInInspector] public float TimeLimit = 5f; // Waktu hidup projectile
    [HideInInspector] public float Damage = 10f; // Damage yang diberikan

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Mengatur arah gerakan projectile
        rb.velocity = transform.forward * AmmoSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Mengurangi waktu hidup projectile
        TimeLimit -= Time.deltaTime;
        if (TimeLimit <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth PH = col.gameObject.GetComponent<PlayerHealth>();
            if (PH != null)
            {
                PH.OnHealthDecrease(Damage);
            }
            Destroy(this.gameObject);
        }
    }
}
