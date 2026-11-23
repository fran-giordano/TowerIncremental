using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] HealthTarget target;
    [SerializeField] Image fill;
    [SerializeField] bool hideWhenFull = true;
    [SerializeField] Gradient colors;        // verde → rojo
    [SerializeField] float margin = 0.3f;
    [SerializeField] float worldScale = 0.01f; 

    Renderer body;
    Camera cam;
    Canvas canvas;

    void Awake()
    {
        cam = Camera.main;
        canvas = GetComponent<Canvas>();
        if (!target) target = GetComponentInParent<HealthTarget>();
        target.Changed += OnChanged;
        body = transform.parent.GetComponent<Renderer>();
    }

    void OnDestroy() { if (target) target.Changed -= OnChanged; }

    void Start() => OnChanged(target.Current, target.Max);

    void OnChanged(float current, float max)
    {
        float t = max > 0 ? current / max : 0;
        fill.fillAmount = t;
        fill.color = colors.Evaluate(t);
        canvas.enabled = !(hideWhenFull && t >= 1f);
    }

    void LateUpdate()
    {
        var parent = transform.parent;

        // Posición: justo encima del modelo, sea del tamaño que sea
        float top = body ? body.bounds.max.y : parent.position.y + 1f;
        transform.position = new Vector3(parent.position.x, top + margin, parent.position.z);

        // Rotación: siempre de frente a la cámara (billboard)
        transform.rotation = cam.transform.rotation;

        // Escala: compensar la escala del padre para que todas midan igual
        var s = parent.lossyScale;
        transform.localScale = new Vector3(worldScale / s.x, worldScale / s.y, worldScale / s.z);
    }
}