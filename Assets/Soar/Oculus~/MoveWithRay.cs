#if SOAR_HAS_OCULUS
using Oculus.Interaction;
using UnityEngine;

public class MoveWithRay : MonoBehaviour
{

    public GameObject rightRayCursor;

    public GameObject leftRayCursor;

    public RayInteractor rightInteractor;

    public RayInteractor leftInteractor;

    public Transform target;

    private bool setInitialScale;

    private Vector3 initialScale;

    private float initialDistance;

    private bool beenGrabbedOnce;

    private GameObject scaleInteractable;

    public GameObject rightInteractable;

    public GameObject leftInteractable;

    private void Update()
    {

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            Oculus.Interaction.Surfaces.SurfaceHit rightSurfaceHit = (Oculus.Interaction.Surfaces.SurfaceHit)rightInteractor.CollisionInfo;
            Oculus.Interaction.Surfaces.SurfaceHit leftSurfaceHit = (Oculus.Interaction.Surfaces.SurfaceHit)leftInteractor.CollisionInfo;

            if (rightInteractor.Interactable.name == leftInteractor.Interactable.name)
            {
                scaleInteractable = rightInteractor.Interactable.gameObject;

                scaleInteractable.transform.SetParent(null);

                if (!setInitialScale)
                {
                    initialScale = scaleInteractable.transform.localScale;
                    initialDistance = Vector3.Distance(rightSurfaceHit.Point, leftSurfaceHit.Point);
                    setInitialScale = true;
                }

                float distance = Vector3.Distance(rightSurfaceHit.Point, leftSurfaceHit.Point);
                if (Mathf.Approximately(initialDistance, 0))
                {
                    return;
                }

                float factor = distance / initialDistance;
                scaleInteractable.transform.localScale = factor * initialScale;

            }
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x != 0)
        {
            if (rightInteractable != null)
            {
                float angle = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
                rightInteractable.transform.GetChild(0).Rotate(Vector3.down, (angle * 100) * Time.deltaTime);
                //rightInteractable.transform.Rotate(Vector3.down, (angle * 100) * Time.deltaTime);
            }
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) && OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).y != 0)
        {
            if (leftInteractable != null)
            {
                float value = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).y;
                float zPos = leftInteractable.transform.localPosition.z + (value / 20.0f);
                leftInteractable.transform.localPosition = new Vector3(leftInteractable.transform.localPosition.x, leftInteractable.transform.localPosition.y, zPos);
            }
        }

        if (rightInteractable != null)
        {
            Vector3 targetPostition = new Vector3(target.position.x,
                               rightInteractable.transform.position.y,
                               target.position.z);
            rightInteractable.transform.LookAt(targetPostition);
            rightInteractable.transform.localRotation *= Quaternion.Euler(0, -180, 0);
        }

        if (leftInteractable != null)
        {
            Vector3 targetPostition = new Vector3(target.position.x,
                               leftInteractable.transform.position.y,
                               target.position.z);
            leftInteractable.transform.LookAt(targetPostition);
            leftInteractable.transform.localRotation *= Quaternion.Euler(0, -180, 0);
        }
    }

    public void GetHitPoint()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Oculus.Interaction.Surfaces.SurfaceHit rightSurfaceHit = (Oculus.Interaction.Surfaces.SurfaceHit)rightInteractor.CollisionInfo;
            if (rightSurfaceHit.Point != null)
            {
                if (rightInteractable == null)
                {
                    rightInteractable = rightInteractor.Interactable.gameObject;
                    rightInteractable.transform.SetParent(rightRayCursor.transform);
                    if (!beenGrabbedOnce)
                    {
                        beenGrabbedOnce = true;
                    }
                }
            }
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            Oculus.Interaction.Surfaces.SurfaceHit leftSurfaceHit = (Oculus.Interaction.Surfaces.SurfaceHit)leftInteractor.CollisionInfo;
            if (leftSurfaceHit.Point != null)
            {
                if (leftInteractable == null)
                {
                    leftInteractable = leftInteractor.Interactable.gameObject;
                    leftInteractable.transform.SetParent(leftRayCursor.transform);
                    if (!beenGrabbedOnce)
                    {
                        beenGrabbedOnce = true;
                    }
                }
            }
        }
    }

    public void RemoveAttatchPoint()
    {
        setInitialScale = false;
        if (leftInteractable != null)
        {
            leftInteractable.transform.SetParent(null);
            leftInteractable = null;
        }
        if (rightInteractable != null)
        {
            rightInteractable.transform.SetParent(null);
            rightInteractable = null;
        }
        if (scaleInteractable != null)
        {
            scaleInteractable = null;
        }
    }
}
#endif