using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class Projectile : MonoBehaviour
{
    [Header("Init Settings")]
    public float speed = 5;
    public float lifeTime = 10;


    [Header("Physics")]
    public float speedRetainFac = 1f;
    [SerializeField] Vector2 acceleration;
    [SerializeField] float angularSpeed;

    [Header("Projectile Internal Variable")]
    [SerializeField] float timer = 0;
    public Vector2 velocity;
    [SerializeField] float oldAngle = 0;
    [SerializeField] new CircleCollider2D collider;
    [SerializeField] SpriteRenderer spriteRenderer;
    public LayerMask hitMask;

    public Shooter shooter;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        timer = 0;
        oldAngle = 0;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (transform.rotation.z != oldAngle)
        {
            velocity = AngPosUtil.GetAngularPos(transform.rotation.eulerAngles.z, 1) * velocity.magnitude;
            oldAngle = transform.rotation.eulerAngles.z;
        }

        // apply movement
        velocity *= speedRetainFac;
        velocity += acceleration * Time.fixedDeltaTime;
        Vector3 pos = transform.position;
        pos.x += velocity.x * Time.fixedDeltaTime;
        pos.y += velocity.y * Time.fixedDeltaTime;
        transform.position = pos;

        // apply rotation
        transform.Rotate(Vector3.forward, angularSpeed * Time.fixedDeltaTime);

        // life time
        if (timer > lifeTime)
        {
            DestroyProjectile();
        }
    }

    public virtual bool IsMaskMatched(LayerMask mask, GameObject target)
    {
        return ((mask.value & (1 << target.layer)) > 0);
    }

    protected virtual void DestroyProjectile()
    {
        StartCoroutine(DestroyProjectileIE());
    }

    protected virtual IEnumerator DestroyProjectileIE()
    {
        // disable
        spriteRenderer.enabled = false;
        collider.enabled = false;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;

        yield return new WaitForSeconds(2);

        // Destroy(gameObject);
        Destroy();
    }

    void Destroy()
    {
        StopAllCoroutines();
    }
}
