using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

[RequireComponent(typeof(LineRenderer))]
public class LineCaster : MonoBehaviour
{
    public float shaderVisibility;
    public LayerMask raycastMask;
    public LineRenderer LineRenderer;

    [HideInInspector] public Vector2 hitPos;

    void Update()
    {
        float angle = transform.eulerAngles.z;
        Vector2 pos = transform.position;
        Vector2 dir = AngPosUtil.GetAngularPos(angle, 1);

        RaycastHit2D hit = Physics2D.Raycast(pos, dir, 100, raycastMask);

        if (hit.collider != null)
        {
            hitPos = hit.point;
        }
        else
        {
            hitPos = pos + dir * 100;
        }

        LineRenderer.SetPosition(0, pos);
        LineRenderer.SetPosition(1, hitPos);

        LineRenderer.material.SetFloat("_Visibility", shaderVisibility);
    }

    public float GetDistance()
    {
        return Vector2.Distance(transform.position, hitPos);
    }
}
