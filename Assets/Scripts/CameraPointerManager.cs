using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager Instance;

    [SerializeField] private GameObject pointer;
    [SerializeField] private float maxDistancePointer = 4.5f;
    [Range(0, 1)] [SerializeField] private float disPointerObject = 0.95f;

    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;
    private readonly string interactableTag = "Interactable";
    private float scaleSize = 0.025f;
    [HideInInspector] public Vector3 hitPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        if (GazeManager.Instance != null)
            GazeManager.Instance.OnGazeSelection += GazeSelection;
    }

    private void GazeSelection()
    {
        ExecuteClickOnCurrentObject();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            hitPoint = hit.point;

            if (_gazedAtObject != hit.transform.gameObject)
            {
                _gazedAtObject?.SendMessage("OnPointerExitXR", SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnterXR", SendMessageOptions.DontRequireReceiver);
                if (GazeManager.Instance != null) GazeManager.Instance.StartGazeSelection();
            }

            bool isInteractable = hit.transform.CompareTag(interactableTag) || hit.transform.GetComponent<IGrabbable>() != null;
            if (isInteractable)
                PointerOnGaze(hit.point);
            else
                PointerOutGaze();
        }
        else
        {
            _gazedAtObject?.SendMessage("OnPointerExitXR", SendMessageOptions.DontRequireReceiver);
            _gazedAtObject = null;
            PointerOutGaze();
        }

        // Clic físico del botón de Cardboard
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            ExecuteClickOnCurrentObject();
        }
    }

    private void ExecuteClickOnCurrentObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            // Prioridad 1: Objeto agarrable mediante interfaz
            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if (grabbable != null)
            {
                grabbable.Grab();
                Debug.Log("Agarre por interfaz: " + hit.transform.name);
                return;
            }

            // Prioridad 2: Resto de objetos (UI, puertas, teletransportes)
            hit.transform.gameObject.SendMessage("OnPointerClickXR", SendMessageOptions.DontRequireReceiver);
            Debug.Log("SendMessage a: " + hit.transform.name);
        }
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);
        pointer.transform.localScale = Vector3.one * scaleFactor;
        pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, disPointerObject);
    }

    private void PointerOutGaze()
    {
        pointer.transform.localScale = Vector3.one * 0.1f;
        pointer.transform.parent.localPosition = new Vector3(0, 0, maxDistancePointer);
        pointer.transform.parent.parent.rotation = transform.rotation;
        if (GazeManager.Instance != null) GazeManager.Instance.CancelGazeSelection();
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x + t * (p1.x - p0.x);
        float y = p0.y + t * (p1.y - p0.y);
        float z = p0.z + t * (p1.z - p0.z);
        return new Vector3(x, y, z);
    }
}