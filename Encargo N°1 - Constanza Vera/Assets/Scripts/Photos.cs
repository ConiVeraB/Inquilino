using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using NUnit.Framework;
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
   // public List<PhotoData> capturedPhotos = new List<PhotoData>();
    public List<RawImage> galeria = new List<RawImage>();
    public PhoneController phoneController;

    [Header("Configuración de Detección")]
    public float raycastDistance = 20f; // Distancia máxima para el raycast
    public LayerMask detectableLayers; // Qué capas de objetos pueden ser detectadas (ej. "Enemigo", "Pista")
    public Camera cameraJuego;
    public string tagEnemigo = "Enemy";
  


    private void Start()
    {
        if (cameraJuego == null)
        {
            cameraJuego = Camera.main;
            Debug.LogWarning("No se asigno cámara");
        }

        if (photoDisplay != null)
        {
            photoDisplay.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("No se ha asignado la RawImage en el Inspector.");
        }

        foreach (RawImage image in galeria)
        {
            image.gameObject.SetActive(false);
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
            bool fotoEsCorrecta = VerificarEnemigo();

            StartCoroutine(CaptureAndDisplay(fotoEsCorrecta));
           // TakePhoto();
        }
    }

   private bool VerificarEnemigo()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag(tagEnemigo);

        if (enemigos.Length == 0 )
        {
            Debug.LogWarning("No se encontraron cosas con el tag" + tagEnemigo);
            return false;
        }

        foreach(GameObject enemigo in enemigos)
        {
            Vector3 posEnViewport = cameraJuego.WorldToViewportPoint(enemigo.transform.position);
            bool enLaVista = posEnViewport.z > 0 &&
                             posEnViewport.x > 0.1f && posEnViewport.x < 0.9f && // Pequeño margen para que no valga si está justo en el borde
                             posEnViewport.y > 0.1f && posEnViewport.y < 0.9f;
            if (enLaVista)
            {
                // --- COMPROBACIÓN 2: ¿Hay algo bloqueando la vista? (Raycast) ---
                Vector3 direccionAlEnemigo = enemigo.transform.position - cameraJuego.transform.position;
                Debug.DrawRay(cameraJuego.transform.position, direccionAlEnemigo, Color.red, 2.0f); // Dibuja una línea roja por 2 segundos
                RaycastHit hit;

                // Lanzamos un rayo desde la cámara hacia el enemigo
                if (Physics.Raycast(cameraJuego.transform.position, direccionAlEnemigo, out hit))
                {
                    // Si el primer objeto que golpea el rayo tiene el tag del enemigo...
                    if (hit.collider.CompareTag(tagEnemigo))
                    {
                        // ¡Bingo! Hemos encontrado un enemigo visible.
                        Debug.Log("¡Enemigo detectado correctamente en la foto!");
                        return true; // Devolvemos 'true' y salimos de la función.
                    }
                }
            }
        }

        Debug.Log("Ningún enemigo fue detectado correctamente en la foto.");
        return false; // La foto no es correcta.
    }

    /*void TakePhoto()
    {
        isDisplayingPhoto = true; // Evita tomar fotos mientras se muestra una.
        StartCoroutine(CaptureAndDisplay());
    }
    */

    IEnumerator CaptureAndDisplay(bool esCorrecta)
    {
        isDisplayingPhoto = true;

        // Mostramos en la consola si la foto ha sido catalogada como correcta o no.
        if (esCorrecta)
        {
            Debug.Log("CATALOGANDO FOTO COMO: CORRECTA");
        }
        else
        {
            Debug.Log("CATALOGANDO FOTO COMO: INCORRECTA");
        }

        photoTexture = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        for (int i = 0; i < galeria.Count; i++)
        {
            if (galeria[i].texture == null)
            {
                galeria[i].texture = photoTexture;
                galeria[i].gameObject.SetActive(true);
                break;
            }
        }

        Rect regionToRead = new Rect(0, 0, photoWidth, photoHeight);
        RenderTexture renderTexture = new RenderTexture(photoWidth, photoHeight, 24); // 24 bits de profundidad.
        cameraJuego.targetTexture = renderTexture; 

        cameraJuego.Render();

        RenderTexture.active = renderTexture;

        photoTexture.ReadPixels(regionToRead, 0, 0);
        photoTexture.Apply();

       
        cameraJuego.targetTexture = null;
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
