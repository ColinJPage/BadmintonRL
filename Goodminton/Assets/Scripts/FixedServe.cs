using System.Collections;
using UnityEngine;

public class FixedServe : MonoBehaviour
{
    [Header("Inspector Set Properties")]
    [Tooltip("The outermost left bound opposite to the player")]
    public Transform leftBound;
    [Tooltip("The outermost right bound opposite to the player")]
    public Transform rightBound;
    public Transform net;

    //Bounded force magnitudes
    private float xMin;
    private float xMax;
    //Currently not read from, may be useful in case a minimum offset from z position of net is relevant for bounding
    private float zMin;
    private float zMax;
    private float zNet;
    private float netHeight;
    private float netHeightMargin;

    private float groundOffset = 0.1f;
    private float maxSpeed = 15f;
    private float estBirdieRadius = 0.5f;
    private float safetyHeight = 3f;
    
    private float gravityMagnitude = 9.81f;

    //Keep this default/low for performance reasons during training
    public int maxSampleSize = 25;

    [Header("Serve pitch")]
    public float minDeviationAngle = 25f;
    public float maxDeviationAngle = 60f;

    private bool hasServed = false;
    private void Awake()
    {
        xMin = leftBound.position.x;
        xMax = rightBound.position.x;
        zNet = net.position.z;
        netHeight = net.gameObject.GetComponent<Collider>().bounds.size.y;
        zMin = Mathf.Min(zNet, leftBound.position.z);
        zMax = Mathf.Max(zNet, leftBound.position.z);
        netHeightMargin = estBirdieRadius + safetyHeight;
    }

    public void ValidForceServe(Rigidbody rb, Vector3 contactPoint) {
        contactPoint.y += 0.5f;
        for (int i = 0; i < maxSampleSize; i++)
        {
            float targetX = Random.Range(xMin, xMax);
            float targetZ = Random.Range(zMin, zMax);
            Vector3 landingSpot = new Vector3(targetX, groundOffset, targetZ);
            float netToTarget = landingSpot.z - zNet;
            float directionSign = Mathf.Sign(netToTarget);

            float minApexZ = zNet + directionSign * 0.75f;
            float apexZMargin = Mathf.Lerp(minApexZ, landingSpot.z, 0.5f);

            float minApexY = netHeightMargin + 0.5f;
            float apexY = Random.Range(minApexY, minApexY + 0.8f);
            Vector3 apexSpot = new Vector3((contactPoint.x + landingSpot.x) / 2f, apexY, apexZMargin);

            float ascentTime = Mathf.Sqrt(2f * (apexSpot.y - contactPoint.y) / gravityMagnitude);
            float descentTime = Mathf.Sqrt(2f * (apexSpot.y - landingSpot.y) / gravityMagnitude);
            float travelTime = ascentTime + descentTime;

            if (float.IsNaN(travelTime) || travelTime <= 0f)
            {
                continue; //invalid sample
            }
            
            Vector3 lateralDelta = new Vector3(landingSpot.x - contactPoint.x, 0f, landingSpot.z - contactPoint.z);
            Vector3 lateralVelocity = lateralDelta / travelTime;

            float verticalVelocity = gravityMagnitude * ascentTime;

            Vector3 serveVelocity = new Vector3(lateralVelocity.x, verticalVelocity, lateralVelocity.z);
            if (serveVelocity.magnitude > maxSpeed)
            {
                continue;
            }
            
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
            rb.linearVelocity = serveVelocity;
        }

    }
        

    public void ValidForceServeB(Rigidbody rb, Vector3 contactPoint)
    {
        contactPoint.y += 0.5f;
        for (int i = 0; i < maxSampleSize; i++)
        {
            //contactPoint.y = Mathf.Max(rb.position.y, 0.5f);
            float maxPitch = Mathf.Atan((netHeight - contactPoint.y) / Mathf.Abs(zMax - contactPoint.z));
            float clampedMaxPitch = Mathf.Min(maxPitch, maxDeviationAngle);
            //Arc height range
            float pitch = Mathf.Deg2Rad * Random.Range(minDeviationAngle, clampedMaxPitch * Mathf.Rad2Deg);
            float cosPitch = Mathf.Cos(pitch);
            float sinPitch = Mathf.Sin(pitch);

            float depthFromNet = Mathf.Abs(zNet - contactPoint.z);
            float netClearance = netHeight + netHeightMargin;
            float heightFromNet = netClearance  - contactPoint.y - (Mathf.Tan(pitch) * depthFromNet);
            //Range of pitch doesn't have an angle to pass the net on serve, find another sample
            
            if (heightFromNet <= 0f)
            {
                Debug.Log("Bad pitch");
                continue;
            }
            
            //Required force magnitude is beyond max birdie speed
            float minVelocity = Mathf.Sqrt((gravityMagnitude * depthFromNet * depthFromNet) / (2f * cosPitch * cosPitch * heightFromNet));
            if (float.IsNaN(minVelocity) || minVelocity > maxSpeed)
            {
                Debug.Log("Out of bound speed");
                continue;
            }
            float zMinOffset = Mathf.Abs(zMin - contactPoint.z);
            float zMaxOffset = Mathf.Abs(zMax - contactPoint.z);
            float yZMaxOffset = contactPoint.y - groundOffset - (Mathf.Tan(pitch) * zMinOffset);

            //Out of bounds landing
            if (yZMaxOffset <= 0f)
            {
                Debug.LogWarning($"Invalid arc: yZMaxOffset={yZMaxOffset}, contactY={contactPoint.y}, zMaxOffset={zMaxOffset}, pitchDeg={pitch * Mathf.Rad2Deg}");
                Debug.Log("Out of bound landing");
                continue;
            }

            float maxVelocity = Mathf.Sqrt((gravityMagnitude * zMaxOffset * zMaxOffset) / (2f * cosPitch * cosPitch * yZMaxOffset));
            if (float.IsNaN(maxVelocity))
            {
                Debug.Log("Bad max");
                continue;
            }

            maxVelocity = Mathf.Min(maxVelocity, maxSpeed);


            if (minVelocity > maxVelocity)
            {
                Debug.Log("Impossible velocity bounds");
                continue;
            }

            float sampleVelocity = Random.Range(minVelocity, maxVelocity);
            float travelTime = (2f * sampleVelocity * sinPitch) / gravityMagnitude;
            if (travelTime <= 0f)
            {
                Debug.Log("Bad travel time");
            }
            float spinVelocity = sampleVelocity * cosPitch;

            float xMinOffset = xMin - contactPoint.x;
            float xMaxOffset = xMax - contactPoint.x;

            float maxLateralAngle = Mathf.Min(30f * Mathf.Deg2Rad, Mathf.PI / 6);
            float spinMin = Mathf.Asin(Mathf.Clamp(xMinOffset / (spinVelocity * travelTime), -1f, 1f));
            float spinMax = Mathf.Asin(Mathf.Clamp(xMaxOffset / (spinVelocity * travelTime), -1f, 1f));

            //Prevent the shot from going too wide and losing verticality
            spinMin = Mathf.Clamp(spinMin, -maxLateralAngle, maxLateralAngle);
            spinMax = Mathf.Clamp(spinMax, -maxLateralAngle, maxLateralAngle);

            //Swap the two to enforce spinMax >= spinMin
            if (spinMin > spinMax)
            {
                float temp = spinMin;
                spinMin = spinMax;
                spinMax = temp;
            }

            float sampleSpin = Random.Range(spinMin, spinMax);

            Vector3 serveDirection = new Vector3(Mathf.Sin(sampleSpin) * cosPitch, sinPitch, Mathf.Cos(sampleSpin) * cosPitch);
            Vector3 serveVelocity = serveDirection * sampleVelocity;

            Debug.Log("Should be applying this velocity: " + serveVelocity);
            Debug.Log($"Using net clearance margin of {netClearance}. Height from net: {heightFromNet}");
            //Time.timeScale = 0;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
            rb.linearVelocity = serveVelocity;
            Debug.DrawRay(rb.position, serveVelocity.normalized * 3f, Color.red, 2f);
            return;
        }
        Debug.Log("None found");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasServed)
        {
            return;
        }
        Birdie hitBirdie = collision.transform.GetComponentInParent<Birdie>();
        if (hitBirdie != null)
        {
            Rigidbody birdieRB = collision.transform.GetComponentInParent<Rigidbody>();
            if (birdieRB != null)
            {
                Collider birdieCol = collision.collider;
                Collider racketCol = GetComponent<Collider>();
                Physics.IgnoreCollision(birdieCol, racketCol,  true);
                hasServed = true;
                Vector3 contactPoint = collision.contacts[0].point;
                StartCoroutine(ServeAtFixedUpdate(birdieRB, birdieCol, racketCol, contactPoint));
            }
        } 
    }

    IEnumerator ServeAtFixedUpdate(Rigidbody rb, Collider birdCol, Collider rackCol, Vector3 contact)
    {
        yield return new WaitForFixedUpdate();
        ValidForceServe(rb, contact);
        yield return new WaitForFixedUpdate();
        Physics.IgnoreCollision(birdCol, rackCol, false);
        hasServed = false;
    }
}
