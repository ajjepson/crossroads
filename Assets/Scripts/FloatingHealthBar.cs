using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;

        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }
    }

    void Update()
    {
        
    }


    public void UpdateBar(float currentValue, float maxValue)//update damage bar
    {
        if (slider == null) return;

        slider.value = currentValue / maxValue;

    }
    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
        transform.LookAt(Camera.main.transform);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
