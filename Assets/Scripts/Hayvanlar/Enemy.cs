using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " hasar aldı! Kalan Can: " + health); // Test için

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " öldü!");
        Destroy(gameObject);
    }
}
