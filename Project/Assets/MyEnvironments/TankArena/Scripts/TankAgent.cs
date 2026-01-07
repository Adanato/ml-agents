using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TankAgent : Agent
{
    [Header("Tank Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;
    public float bulletSpeed = 20f;
    public float fireCooldown = 0.5f;
    
    [Header("References")]
    public Transform turret;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public TankAgent opponent;
    
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;
    private float lastFireTime;
    
    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    public override void OnEpisodeBegin()
    {
        // Reset tank
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentHealth = maxHealth;
        lastFireTime = -fireCooldown;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Own state (6 values)
        sensor.AddObservation(transform.localPosition.x / 10f);
        sensor.AddObservation(transform.localPosition.z / 10f);
        sensor.AddObservation(transform.localRotation.eulerAngles.y / 360f);
        sensor.AddObservation(rb.linearVelocity.x / moveSpeed);
        sensor.AddObservation(rb.linearVelocity.z / moveSpeed);
        sensor.AddObservation(currentHealth / maxHealth);
        
        // Opponent state (5 values)
        if (opponent != null)
        {
            Vector3 toOpponent = opponent.transform.localPosition - transform.localPosition;
            sensor.AddObservation(toOpponent.x / 20f);
            sensor.AddObservation(toOpponent.z / 20f);
            sensor.AddObservation(opponent.transform.localRotation.eulerAngles.y / 360f);
            sensor.AddObservation(opponent.currentHealth / maxHealth);
            sensor.AddObservation(Vector3.Angle(transform.forward, toOpponent) / 180f);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
        
        // Cooldown state (1 value)
        float cooldownRemaining = Mathf.Max(0, fireCooldown - (Time.time - lastFireTime));
        sensor.AddObservation(cooldownRemaining / fireCooldown);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Continuous actions: move, rotate
        float moveInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float rotateInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        
        // Discrete action: fire
        int fireAction = actions.DiscreteActions[0];
        
        // Apply movement
        Vector3 move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
        
        // Apply rotation
        float rotation = rotateInput * rotateSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
        
        // Fire if action is 1 and cooldown is ready
        if (fireAction == 1 && Time.time - lastFireTime >= fireCooldown)
        {
            Fire();
            lastFireTime = Time.time;
        }
        
        // Small time penalty to encourage action
        AddReward(-0.001f);
    }

    private void Fire()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = firePoint.forward * bulletSpeed;
            }
            
            // Add bullet component to track ownership
            TankBullet tankBullet = bullet.AddComponent<TankBullet>();
            tankBullet.owner = this;
            
            Destroy(bullet, 3f); // Auto-destroy after 3 seconds
        }
    }

    public void TakeDamage(float damage, TankAgent attacker)
    {
        currentHealth -= damage;
        
        // Reward attacker for hitting
        if (attacker != null)
        {
            attacker.AddReward(1.0f);
        }
        
        // Penalty for getting hit
        AddReward(-0.5f);
        
        if (currentHealth <= 0)
        {
            // Attacker gets kill bonus
            if (attacker != null)
            {
                attacker.AddReward(5.0f);
            }
            
            // Death penalty
            AddReward(-1.0f);
            
            // End episode for both
            EndEpisode();
            if (opponent != null)
            {
                opponent.EndEpisode();
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Vertical");
        continuousActions[1] = Input.GetAxis("Horizontal");
        
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}

// Bullet helper class
public class TankBullet : MonoBehaviour
{
    public TankAgent owner;
    public float damage = 25f;
    
    private void OnCollisionEnter(Collision collision)
    {
        TankAgent hitTank = collision.gameObject.GetComponent<TankAgent>();
        if (hitTank != null && hitTank != owner)
        {
            hitTank.TakeDamage(damage, owner);
        }
        Destroy(gameObject);
    }
}
