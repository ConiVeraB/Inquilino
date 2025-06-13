using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
public class Photos : MonoBehaviour
{

    public RawImage photoDisplay; // Referencia a la RawImage en la UI para mostrar la foto.
    public float displayDuration = 2.0f; // Duración en segundos que la foto se muestra.
    public int photoWidth = 512; // Ancho de la foto. 
    public int photoHeight = 512; // Alto de la foto
    public string albumName = "PhonePhotos";
    private Texture2D photoTexture;
    private bool isDisplayingPhoto = false;
    public List<RawImage> galeria = new List<RawImage>();
    public PhoneController phoneController;

    private void Start()
    {
        if (photoDisplay != null)
        {
            photoDisplay.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("No se ha asignado la RawImage en el Inspector.");
        }

        if (phoneController == null)
        {
            Debug.LogError("No se ha asignado el PhoneSystem en el Inspector del script Photos.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isDisplayingPhoto )
        {
            TakePhoto();
        }
    }

   

    void TakePhoto()
    {
        isDisplayingPhoto = true; // Evita tomar fotos mientras se muestra una.
        StartCoroutine(CaptureAndDisplay());
    }

    IEnumerator CaptureAndDisplay()
    {
        
        photoTexture = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        for (int i = 0; i < galeria.Count; i++)
        {
            if (galeria[i].texture == null)
            {
                galeria[i].texture = photoTexture;
                break;
            }
        }

        Rect regionToRead = new Rect(0, 0, photoWidth, photoHeight);
        RenderTexture renderTexture = new RenderTexture(photoWidth, photoHeight, 24); // 24 bits de profundidad.
        Camera.main.targetTexture = renderTexture; 


        Camera.main.Render();

        RenderTexture.active = renderTexture;

        photoTexture.ReadPixels(regionToRead, 0, 0);
        photoTexture.Apply();

       
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

       
        photoDisplay.texture = photoTexture;
        photoDisplay.gameObject.SetActive(true);
        phoneController.SavePhoto(photoTexture);

        
        yield return new WaitForSeconds(displayDuration);

       
        photoDisplay.gameObject.SetActive(false);
       // Destroy(photoTexture);
       //   photoTexture = null; 
        isDisplayingPhoto = false; 

    }
}
