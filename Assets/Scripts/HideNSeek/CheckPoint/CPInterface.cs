using UnityEngine;

public class CPInterface : MonoBehaviour
{

    [SerializeField]
    private CPFloatingPoint cpAnimation;

    private GameObject collisionTarget;
    private GameObject lastCollisionTarget;
    private bool hidden = false;
    private MeshRenderer[] meshes;

    private void Awake()
    {
        meshes = cpAnimation.GetComponentsInChildren<MeshRenderer>();
    }

    public GameObject IsTouching
    {
        get { return collisionTarget; }
    }

    public GameObject LastCollisionWith
    {
        get { return lastCollisionTarget; }
    }
    public bool IsHidden
    {
        get { return hidden; }
        set
        {
            if (value != hidden)
            {
                OnStatusChange(value);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionTarget = other.gameObject;
        lastCollisionTarget = collisionTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        collisionTarget = null;
    }

    private void OnStatusChange(bool status)
    {
        foreach (var mesh in meshes)
        {
            mesh.enabled = !status;
        }
        cpAnimation.enabled = !status;
        hidden = status;
    }
}
