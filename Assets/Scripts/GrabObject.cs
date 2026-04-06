using UnityEngine;

public class GrabObject : MonoBehaviour, IGrabbable
{
    protected GrabManager grabManager;
    protected BoxCollider boxCollider;
    protected Vector3 spawnerPosition;
    protected Quaternion spawnerRotation;

    public AudioClip soundGrab;
    public AudioClip soundPlace;

    [SerializeField] public string type = "Objeto";
    [SerializeField] public GameObject spawner;

    protected AudioSource audioSource;  // Cambiado de player a audioSource para consistencia

    protected virtual void Start()
    {
        // Configurar AudioSource (lo crea si no existe)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (soundGrab != null || soundPlace != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.Log("AudioSource creado automáticamente en " + gameObject.name);
        }

        if (spawner != null)
        {
            spawnerPosition = spawner.transform.position;
            spawnerRotation = spawner.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Spawner no asignado en " + gameObject.name);
        }

        boxCollider = GetComponent<BoxCollider>();
        grabManager = GrabManager.Instance;
        if (grabManager == null)
            Debug.LogError("GrabManager no encontrado en la escena");
    }

    public virtual void Grab()
    {
        if (audioSource != null && soundGrab != null)
            audioSource.PlayOneShot(soundGrab);

        if (grabManager.heldItem != null)
        {
            GrabObject current = grabManager.heldItem.GetComponent<GrabObject>();
            if (current != null) current.Drop();
        }

        grabManager.heldItem = gameObject;
        if (boxCollider != null) boxCollider.enabled = false;
        Debug.Log("Objeto agarrado: " + gameObject.name);
    }

    public virtual void Drop()
    {
        transform.position = spawnerPosition;
        transform.rotation = spawnerRotation;
        grabManager.heldItem = null;
        if (boxCollider != null) boxCollider.enabled = true;
    }

    public virtual void Delete()
    {
        transform.position = spawnerPosition;
        transform.rotation = spawnerRotation;
        grabManager.heldItem = null;
        if (boxCollider != null) boxCollider.enabled = true;
        gameObject.SetActive(false);
    }

    public virtual void Respawn()
    {
        transform.position = spawnerPosition;
        transform.rotation = spawnerRotation;
        if (boxCollider != null) boxCollider.enabled = true;
        gameObject.SetActive(true);
    }

    public virtual void Place(Vector3 position)
    {
        if (audioSource != null && soundPlace != null)
            audioSource.PlayOneShot(soundPlace);
        transform.position = position;
        grabManager.heldItem = null;
        if (boxCollider != null) boxCollider.enabled = true;
    }

    // Método para compatibilidad con el sistema anterior (SendMessage)
    public void OnPointerClickXR()
    {
        Grab();
    }
}