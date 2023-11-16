using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shooter Setting", menuName = "Projectiles/Shooter Setting", order = 1)]
public class ShooterSetting : ScriptableObject
{
    [Header("General")]
    public float cooldown = 1.2f;
    public float angle = 90;

    [Header("Multi")]
    public int projPerShot = 1;
    public float timePerProjectile = 0.1f;
    public float multiSpanAngle = 0;
    public float multiSpanDist = 0;

    [Header("Effects")]
    public GameObject shootParticle;
    public string shootSFX;
}
