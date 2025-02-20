using UnityEngine;

public class PlayerBouncing : MonoBehaviour
{
    public float floatForce = 5f; // Gaya untuk mengapung
    public float floatHeight = 1f; // Tinggi target di permukaan air
    public float bounceSpeed = 2f; // Kecepatan naik-turun
    public float bounceAmplitude = 0.3f; // Amplitudo gerakan naik-turun

    private Rigidbody rb;
    private float originalY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalY = transform.position.y;
    }

    void FixedUpdate()
    {
        ApplyBuoyancy();
    }

    void ApplyBuoyancy()
    {
        // Tambahkan gaya apung ke Rigidbody
        Vector3 targetPosition = new Vector3(transform.position.x, originalY + floatHeight, transform.position.z);

        // Naik turun buat efek mengambang
        float bounceOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceAmplitude;

        float forceY = Mathf.Clamp(targetPosition.y + bounceOffset - transform.position.y, -1f, 1f);
        rb.AddForce(new Vector3(0f, forceY * floatForce, 0f), ForceMode.Acceleration);
    }
}
