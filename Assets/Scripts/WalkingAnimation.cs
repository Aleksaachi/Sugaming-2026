using UnityEngine;

public class WalkingAnimation : MonoBehaviour
{
    Vector3 lastPosition;
    public float maxWobbleDistance;
    public float angleChangeAtMaxWobble;
    float angle;
    int wobbleDir = 1;
    public float angleLimit;
    public ParticleSystem walkPart;
    int counter = 0;
    void Start()
    {
        lastPosition = transform.position;
    }
    void Update()
    {
        counter++;
        if (counter > 15){
            counter = 0;
            Vector3 currentPosition = transform.position;

            float distanceFactor = Mathf.Clamp01(Vector3.Distance(currentPosition, lastPosition) / maxWobbleDistance);
            float wobbleStrenght = distanceFactor;

            angle += angleChangeAtMaxWobble * wobbleStrenght * wobbleDir * Time.deltaTime;
            if (wobbleStrenght != 0 && !walkPart.isEmitting)
            {
                walkPart.Play();
            }
            else if (wobbleStrenght == 0)
            {
                walkPart.Stop();
            }

            if (Mathf.Abs(angle) > Mathf.Abs(angleLimit))
            {
                angle = angleLimit * (angle / Mathf.Abs(angle));
                wobbleDir *= -1;
            }
            if (wobbleStrenght == 0 && angle != 0)
            {
                if (angle < 0) wobbleDir = 1;
                else wobbleDir = -1;
                angle += wobbleDir * angleChangeAtMaxWobble/3 * Time.deltaTime;
                if (Mathf.Abs(angle) < 1) angle = 0;
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);

            lastPosition = currentPosition;
        }
    }
}
