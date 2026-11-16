using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] HealthTarget target;
    [SerializeField] Image fill;
    [SerializeField] bool hideWhenFull = true;
    [SerializeField] Gradient colors;        // verde → rojo

    Camera cam;
    Canvas canvas;

    void Awake()
    {
        cam = Camera.main;
        canvas = GetComponent<Canvas>();
        if (!target) target = GetComponentInParent<HealthTarget>();
        target.Changed += OnChanged;
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

    void LateUpdate() => transform.rotation = cam.transform.rotation;   // siempre mirando a cámara
}