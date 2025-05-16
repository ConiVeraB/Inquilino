using UnityEngine;
using DG.Tweening;//LIBRERIA DOTWEEN
using UnityEngine.UI; 

public class DoTweenTest : MonoBehaviour
{
    bool isOpen;
    public Ease curveAnimation;
    RectTransform testUI;
    Image imageTest;

    //AudioSource SFXSource;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
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
                transform.DOLocalRotate(new Vector3(0, 90, 0), 1f).SetEase(curveAnimation);

                break;

            case false:
                //CERRAR
                transform.DOLocalRotate(new Vector3(0, 0, 0), 1f).SetEase(curveAnimation);
                break;
        }
    }
}
