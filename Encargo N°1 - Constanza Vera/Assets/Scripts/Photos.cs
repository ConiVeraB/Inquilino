using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
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
    public float detectionRadius = 0.5f;
    public float raycastDistance = 20f; // Distancia máxima para el raycast
    public LayerMask detectableLayers; // Qué capas de objetos pueden ser detectadas (ej. "Enemigo", "Pista")
    public Camera cameraJuego;
    public string tagEnemigo = "Enemy";

    [Header("Feedback de Foto")]
    [SerializeField] private RectTransform panelFotoCorrecta;
    [SerializeField] private RectTransform panelFotoIncorrecta;

    [SerializeField] private Vector2 posVisible = new Vector2(770.72f, 217.71f);
    [SerializeField] private Vector2 posOculta = new Vector2(1140f, 217.71f);
    [SerializeField] private float velocidadAnim = 0.5f;
    [SerializeField] private float duracionFeedback = 4f;

    [Header("Gestión de Misión")]
    public PhotoQuestManager questManager;

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

        if (questManager == null)
        {
            Debug.LogError("¡ERROR! No se ha asignado el PhotoQuestManager en el script Photos.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isDisplayingPhoto && questManager.IsQuestActive())
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

        Debug.Log("Paso 1 SUPERADO: Se encontraron " + enemigos.Length + " enemigos.");

        foreach (GameObject enemigo in enemigos)
        {
            Vector3 posEnViewport = cameraJuego.WorldToViewportPoint(enemigo.transform.position);
            bool enLaVista = posEnViewport.z > 0 &&
                             posEnViewport.x > 0.1f && posEnViewport.x < 0.9f && // Pequeño margen para que no valga si está justo en el borde
                             posEnViewport.y > 0.1f && posEnViewport.y < 0.9f;
            if (enLaVista)
            {
                Vector3 origen = cameraJuego.transform.position;
                Vector3 direccion = enemigo.transform.position - origen;
                float distancia = direccion.magnitude; // La distancia hasta el enemigo
                //Vector3 direccionAlEnemigo = enemigo.transform.position - cameraJuego.transform.position;
                //Debug.DrawRay(cameraJuego.transform.position, direccionAlEnemigo, Color.red, 2.0f); // Dibuja una línea roja por 2 segundos
                RaycastHit hit;

                if (Physics.SphereCast(origen, detectionRadius, direccion, out hit, distancia, detectableLayers))
                {
                    // La esfera golpeó algo en la layer correcta. ¿Es el enemigo que buscamos?
                    // Comparamos el transform del collider golpeado con el del enemigo.
                    if (hit.transform == enemigo.transform || hit.transform.IsChildOf(enemigo.transform))
                    {
                        Debug.Log("Paso 3 SUPERADO para '" + enemigo.name + "': SphereCast golpeó a '" + hit.collider.name + "'. ¡FOTO CORRECTA!");
                        // Dibuja la esfera en el punto de impacto para que veas qué golpeó
                        Debug.DrawRay(origen, direccion.normalized * hit.distance, Color.green, 2.0f);
                        return true;
                    }

                }
                else
                {
                    // La esfera NO golpeó NADA en la layer especificada.
                    Debug.DrawRay(origen, direccion, Color.magenta, 2.0f); // Dibuja rayo MAGENTA (fallo de SphereCast)
                    Debug.LogError("Paso 3 FALLIDO para '" + enemigo.name + "': El SphereCast fue lanzado pero no golpeó NADA en la LayerMask 'Enemigo'.");
                }

                Debug.Log("FIN DE VERIFICACIÓN: Ningún enemigo cumplió las condiciones.");
                return false;

                // Lanzamos un rayo desde la cámara hacia el enemigo
                /* if (Physics.Raycast(cameraJuego.transform.position, direccionAlEnemigo, out hit))
                 {
                     // Si el primer objeto que golpea el rayo tiene el tag del enemigo...
                     if (hit.collider.CompareTag(tagEnemigo))
                     {
                         // ¡Bingo! Hemos encontrado un enemigo visible.
                         Debug.Log("¡Enemigo detectado correctamente en la foto!");
                         return true; // Devolvemos 'true' y salimos de la función.
                     }
                 } 
                */
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

    private void MostrarFeedback(RectTransform panel)
    {
        if (panel == null) return;

        panel.DOKill();
        panel.anchoredPosition = posOculta;

        panel.DOAnchorPos(posVisible, velocidadAnim).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            DOVirtual.DelayedCall(duracionFeedback, () =>
            {
                panel.DOAnchorPos(posOculta, velocidadAnim).SetEase(Ease.InCubic);
            });
        });
    }


    IEnumerator CaptureAndDisplay(bool esCorrecta)
    {
        isDisplayingPhoto = true;

        // Mostramos en la consola si la foto ha sido catalogada como correcta o no.
        if (esCorrecta)
        {
            Debug.Log("CATALOGANDO FOTO COMO: CORRECTA");

            if (esCorrecta)
            {
                Debug.Log("CATALOGANDO FOTO COMO: CORRECTA");
                MostrarFeedback(panelFotoCorrecta);
            }

        }
        else
        {
            Debug.Log("CATALOGANDO FOTO COMO: INCORRECTA");
            MostrarFeedback(panelFotoIncorrecta);
        }

        if (questManager != null)
        {
            questManager.RegisterPhoto(esCorrecta);

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
}
