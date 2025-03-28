using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Photos : MonoBehaviour
{
    public RawImage photoDisplay; // Referencia a la RawImage en la UI para mostrar la foto.
    public float displayDuration = 2.0f; // Duración en segundos que la foto se muestra.
    public int photoWidth = 512; // Ancho de la foto. 
    public int photoHeight = 512; // Alto de la foto
    public string albumName = "PhonePhotos";
    public GameObject gallery;
    public GameObject galleryImage;
    private Texture2D photoTexture;
    private bool isDisplayingPhoto = false;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDisplayingPhoto)
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
        // 1. Crea una nueva textura para la foto.
        photoTexture = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);

        // 2. Renderiza la escena a la textura.
        Rect regionToRead = new Rect(0, 0, photoWidth, photoHeight);
        RenderTexture renderTexture = new RenderTexture(photoWidth, photoHeight, 24); // 24 bits de profundidad.
        Camera.main.targetTexture = renderTexture; // Asigna la RenderTexture a la cámara.

        //Renderiza la escena en la RenderTexture
        Camera.main.Render();

        //Activa la RenderTexture para lectura
        RenderTexture.active = renderTexture;

        photoTexture.ReadPixels(regionToRead, 0, 0);
        photoTexture.Apply();

        //Limpia y restaura
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // 3. Muestra la foto en la RawImage.
        photoDisplay.texture = photoTexture;
        photoDisplay.gameObject.SetActive(true);

        // 4. Espera un tiempo.
        yield return new WaitForSeconds(displayDuration);

        // 5. Oculta la foto y destruye la textura.
        photoDisplay.gameObject.SetActive(false);
        Destroy(photoTexture);
        photoTexture = null; // Importante para evitar errores si se toma otra foto rápidamente.
        isDisplayingPhoto = false; // Permite tomar otra foto.
    }



}
