using DG.Tweening;
using UnityEngine;

public class DoTweenControlAnim : MonoBehaviour
{
    public float openAngle, closeAngle;
    bool isOpen;
    public Ease curveAnimation;
    
    void Start()// Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        
    }

    
    private void Update()// Update is called once per frame
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetOpenDoor();
        }
    }
    void SetOpenDoor()
    {
        isOpen = !isOpen;
        switch (isOpen)
        {
            case true:
                //ABRIR
                transform.DOLocalRotate(new Vector3(0, openAngle, 0), 1f, RotateMode.FastBeyond360).SetEase(curveAnimation);//positivo


                break;

            case false:
                //CERRAR
                transform.DOLocalRotate(new Vector3(0, closeAngle, 0), 1f, RotateMode.FastBeyond360).SetEase(curveAnimation);//.fast es negativo
                break;
        }
    }
}
