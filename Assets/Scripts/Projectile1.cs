using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : MonoBehaviour {
    public float lifeTime = 3;
    public float dieVelocityRatio = 0.5f;
    public float damageMultipier = 10;
    private Vector2 initialVelocity;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (lifeTime < 0) {
            DestroyProjectile();
        } else {
            lifeTime -= Time.deltaTime;
        }

        if (rb.velocity.magnitude < initialVelocity.magnitude * dieVelocityRatio) {
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        collision.gameObject.SendMessage("Damage", damageMultipier * rb.velocity.magnitude / initialVelocity.magnitude);
        DestroyProjectile();
    }

    void SetInitialSpeed(Vector2 init) {
        initialVelocity = init;
    }

    void DestroyProjectile() {
        Destroy(gameObject);
    }
}
