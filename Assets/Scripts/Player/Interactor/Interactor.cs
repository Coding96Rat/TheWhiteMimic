
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


interface IInteractable
{
    public void Interact();
    //public void Detected(bool isOn);
}


public class Interactor : MonoBehaviour
{

    [Header("Interact Parameters")]
    [SerializeField]
    private float detectCanInteractRange;
    [SerializeField]
    private float interactRangeMin;
    [SerializeField]
    private float interactRangeMax;
    private bool detectObject = false;
    private IInteractable detectedObject = null;
    [SerializeField]
    private LayerMask interactableMask;
    [SerializeField]
    private Transform interactorSource;

    [Space(15)]
    [Header("Player Cursor")]
    [SerializeField]
    private Image interactCursor;

    private Collider[] hitColliders = new Collider[9];
    private HashSet<Collider> trackColliders = new HashSet<Collider>();
    private List<Collider> newColliders = new List<Collider>();
    private HashSet<Collider> exitColliders = new HashSet<Collider>();

    private Vector3 characterCenter;

    private float angleMultiplier = 0.03271f;
    private float baseInteractRange = 0.0098f;
    private void Start()
    {
        if (TryGetComponent<CharacterController>(out CharacterController charController))
        {
            characterCenter = charController.center;
        }
    }


    private void FixedUpdate()
    {

        if (interactorSource == null)
        {
            Debug.LogError("Not Assgin interactor source");
            return;
        }

        //FindCanInteractObject();

        //PlayerInteract();
    }

    //private void FindCanInteractObject()
    //{
    //    int detectColliderNums = Physics.OverlapSphereNonAlloc(transform.position + characterCenter, detectCanInteractRange, hitColliders, interactableMask);
    //    if (detectColliderNums > 0)
    //    {
    //        newColliders.Clear();
    //        for (int i = 0; i < detectColliderNums; i++)
    //        {
    //            newColliders.Add(hitColliders[i]);
    //            if (trackColliders.Add(hitColliders[i]))
    //            {
    //                if (hitColliders[i].TryGetComponent<IInteractable>(out IInteractable interactable))
    //                {
    //                    interactable.Detected(true);
    //                }
    //            }
    //        }

    //        exitColliders.Clear();
    //        exitColliders.UnionWith(trackColliders);
    //        exitColliders.ExceptWith(newColliders);

    //        foreach (Collider collider in exitColliders)
    //        {
    //            if (collider.TryGetComponent<IInteractable>(out IInteractable interactable))
    //            {
    //                interactable.Detected(false);
    //            }
    //            trackColliders.Remove(collider);
    //        }
    //    }
    //    else
    //    {

    //        foreach (Collider collider in trackColliders)
    //        {
    //            if (collider.TryGetComponent<IInteractable>(out IInteractable interactable))
    //            {
    //                interactable.Detected(false);
    //            }
    //        }
    //        trackColliders.Clear();
    //    }
    //}

    private void PlayerInteract()
    {
        //Debug.Log(interactorSource.eulerAngles.x);
        //float xRotation = interactorSource.eulerAngles.x;

        //if (xRotation > 180f)
        //    xRotation = 360f - xRotation;


        //float finalRange = Mathf.Clamp(baseInteractRange + (xRotation * angleMultiplier), interactRangeMin, interactRangeMax);

        //Debug.Log(finalRange);
        //Ray r = new Ray(interactorSource.position, interactorSource.forward);
        //if (Physics.Raycast(r, out RaycastHit hitInfo, finalRange))
        //{
        //    if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
        //    {
        //        IsDetectObject(true, interactObj);
        //    }
        //    else
        //    {
        //        IsDetectObject(false, null);
        //    }
        //}
        //else
        //{
        //    IsDetectObject(false, null);
        //}
    }

    //private void IsDetectObject(bool isDetect, IInteractable interactObject)
    //{
    //    detectObject = isDetect;
    //    detectedObject = interactObject;


    //    if (isDetect)
    //    {
    //        interactCursor.color = Color.yellow;
    //    }
    //    else
    //    {
    //        interactCursor.color = Color.white;
    //    }
    //}

    public void CanInteract(Item item)
    {
        detectedObject = item;
    }

    public void TryInteract()
    {
        if (detectedObject != null)
        {
            detectedObject.Interact();
            Debug.Log("Interact");
        }
    }

    #region Gizmos
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;

    //    float radius = detectCanInteractRange;

    //    Gizmos.DrawWireSphere(transform.position + characterCenter, radius);



    //    // Gizmos.color = new Color(0, 1, 0, 0.2f); // ������ ���
    //    // Gizmos.DrawSphere(position, radius);
    //}
    #endregion
}
