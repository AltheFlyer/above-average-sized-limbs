using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YUtil;

public class Shooter : MonoBehaviour
{
    public bool auto = true;

    private List<Projectile> projectiles = new List<Projectile>();
    public ShooterSetting setting;
    public GameObject projectilePrefab;
    private float cooldownTimer = 0f;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        cooldownTimer -= Time.fixedDeltaTime;

        if (auto && cooldownTimer < 0)
            Fire();
    }

    public void Fire()
    {
        StartCoroutine(FireIE());
    }

    IEnumerator FireIE()
    {
        // CameraShake.instance.Shake(screenShakeFac);

        cooldownTimer = setting.cooldown;

        // spawn position and angle will change for multiSpanDist Setting
        Vector2 shootPos = Vector2.zero;
        float shootAngle = setting.angle;
        if (setting.projPerShot > 1)
        {
            shootPos.x -= (setting.multiSpanDist / 2);
            shootAngle -= (setting.multiSpanAngle / 2);
        }

        for (int i = 0; i < setting.projPerShot; i++)
        {
            // for non first proj of multi proj, add offset to spawn dist
            if (i > 0)
            {
                shootPos.x += setting.multiSpanDist / (setting.projPerShot - 1);
                shootAngle += setting.multiSpanAngle / (setting.projPerShot - 1);
            }

            // GameObject instance = Instantiate(setting.projectile, shootPos, Quaternion.Euler(0, 0, shootAngle));
            Projectile p = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            p.velocity = AngPosUtil.GetAngularPos(shootAngle, 1) * p.speed;

            if (setting.shootParticle != null)
                Instantiate(setting.shootParticle, shootPos, Quaternion.identity);

            if (setting.timePerProjectile != 0)
            {
                yield return new WaitForSeconds(setting.timePerProjectile);
            }
        }
    }
}
