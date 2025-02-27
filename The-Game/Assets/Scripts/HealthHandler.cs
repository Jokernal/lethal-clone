using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour, Hurtable
{
    float health = 9f;
    public void Hurt()
    {
        this.health = 0;
        if (health <= 0)
            Destroy(this.gameObject);
    }
}
