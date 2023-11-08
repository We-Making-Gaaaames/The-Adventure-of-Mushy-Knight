using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject target;
    Vector3 targetVelocity;
    Vector3 fOffSet;
    Vector3 offset;
    Vector3 velocity;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.FindWithTag("Player"))
        {
            target = GameObject.FindWithTag("Player");
        }
        else
        {
            target = null;
        }
        fOffSet = new Vector3(0, 1f, -10f);
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            offset.y += Input.GetAxis("Vertical");
            if(offset.y > 1f)
            {
                offset.y = 1f;
            }
            if (offset.y < -1f)
            {
                offset.y = -1f;
            }
        }
        else
        {
            offset = fOffSet;
        }

        targetVelocity = target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetVelocity, ref velocity, .1f);
    }

}
