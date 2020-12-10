using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float lifeTime = 3;
    public float dieVelocityRatio = 0.5f;
    public float damageMultipier = 10;
    private Vector2 initialVelocity;
    private float lateSpeed;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        lateSpeed = rb.velocity.magnitude;
    }

    // Update is called once per frame
    void Update() {
        if (lifeTime < 0) {
            DestroyProjectile();
        } else {
            lifeTime -= Time.deltaTime;
        }

        if (rb.velocity.magnitude < initialVelocity.magnitude * dieVelocityRatio) {
            if (lifeTime > 2)
                lifeTime = 2;
        }
    }

    private void LateUpdate() {
        lateSpeed = rb.velocity.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.name.StartsWith("playerShootBetter")) {
            collision.gameObject.SendMessage("Damage", damageMultipier * lateSpeed / initialVelocity.magnitude);
            DestroyProjectile();
        } else {
            GetComponent<AudioSource>().Play();
        }
    }

    void SetInitialSpeed(Vector2 init) {
        initialVelocity = init;
    }

    void DestroyProjectile() {
        Destroy(gameObject);
    }
}
