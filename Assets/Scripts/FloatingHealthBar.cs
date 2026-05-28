using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;// damage slider
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f);
    [SerializeField] private Camera mainCamera;

    void Start()
    {

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }



    }

    void Update()
    {
        
    }


    public void UpdateBar(float currentValue, float maxValue)//update damage bar
    {
        slider.value = currentValue / maxValue;
    }
}
