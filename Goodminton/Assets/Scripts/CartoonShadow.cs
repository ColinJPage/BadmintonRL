using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CartoonShadow : MonoBehaviour
{
    [SerializeField] DecalProjector projector;
    [SerializeField] float maxDist = 50f;

    [Tooltip("The shadow will only extend as far as the first object hit downward")]
    [SerializeField] bool useRaycast = true;
    [SerializeField] LayerMask layers;

    [SerializeField] Transform shadowPlane;

    private float shadowRadius;
    private float shadowAspect = 1f;

    private void Awake()
    {
        shadowRadius = projector.size.x;
    }
    private void OnEnable()
    {
        shadowAspect = projector.size.x / projector.size.y;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.parent ? transform.parent.rotation.eulerAngles.y : 0f, 0f);
        RaycastHit hit;
        float distance;

        bool didHit = Physics.Raycast(transform.position, -transform.up, out hit, maxDist, layers, QueryTriggerInteraction.Ignore);

        if(didHit)
        {
            distance = hit.distance + 0.3f; //go a little bit farther than the surface to ensure shadow doesnt clip weird
        }
        else
        {
            distance = maxDist;
        }

        float projectDistance = useRaycast ? distance : maxDist;

        var radius = shadowRadius * DistanceToRadius(distance, maxDist);
        projector.size = new Vector3(radius, radius/shadowAspect, projectDistance);
        projector.pivot = Vector3.forward* projectDistance * 0.5f;

        if (shadowPlane)
        {
            shadowPlane.gameObject.SetActive(didHit);
            if (didHit)
            {
                shadowPlane.position = hit.point;
                shadowPlane.localScale = new Vector3(radius, 1f, radius / shadowAspect);
            }
        }
    }
    public static float DistanceToRadius(float distance, float maxDist)
    {
        return Mathf.Clamp(1f - distance / maxDist, 0f, 1f);
    }
}
