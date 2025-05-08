using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour
{
    [Header("Objetos principales")]
    public GameObject phoneModel;
    public Animator phoneAnimator;
    public GameObject phoneUI;
    public CanvasGroup phoneCanvasGroup;
    private Coroutine notificacionCoroutine;


    [Header("Configuraciones de linterna")]
    public GameObject flashlight;

    [Header("Pantallas")]
    public GameObject pantallaRecorte;
    public GameObject panelBloqueo;
    public GameObject panelInicio;
    public GameObject panelCamara;
    public GameObject appLuces;
    public GameObject appMensajes;
    public GameObject panelSensorMovimiento;
    public GameObject panelChat1;
    public GameObject panelChat2;
    public GameObject panelChat3;
    public GameObject panelNotificacionBloqueo;


    //[Header("Control de Jugador")]
    //public PlayerController movimientoJugador;
    //public FirstPersonCamera camaraJugador;

    [Header("Cámara Física del Teléfono")]
    public Camera cameraCelular;

    [Header("Tiempos")]
    public float fadeDuration = 0.5f;
    public float unlockAnimationDuration = 0.4f;
    public float phoneMoveDuration = 0.5f;

    [Header("Animación de teléfono al desbloquear")]
    public Vector3 phoneStartPosition = new Vector3(0, -0.5f, 2f);
    public Vector3 phoneEndPosition = new Vector3(0, 0f, 0.4f);
    public Vector3 phoneStartScale = Vector3.one;
    public Vector3 phoneEndScale = new Vector3(1.8f, 1.8f, 1.8f);

    private bool isPhoneActive = false;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isAnimating = false;

    void Start()
    {
        phoneModel.SetActive(false);
        phoneUI.SetActive(false);
        phoneCanvasGroup.alpha = 0f;

        flashlight?.SetActive(false);
        pantallaRecorte?.SetActive(false);
        if (cameraCelular != null) cameraCelular.enabled = false;

        phoneModel.transform.localPosition = phoneStartPosition;
        phoneModel.transform.localScale = phoneStartScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isAnimating)
        {
            TogglePhone();
        }

        if (isPhoneActive)
        {
            DetectSwipe();
        }
    }


    void TogglePhone()
    {
        if (isAnimating) return; 
        isAnimating = true;      

        var follower = phoneModel.GetComponent<FollowPlayer>();

        if (!isPhoneActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPhoneActive = true;

            phoneModel.transform.localScale = phoneStartScale;
            phoneModel.SetActive(true);
            pantallaRecorte.SetActive(true);
            phoneUI.SetActive(true);

            phoneCanvasGroup.alpha = 1f;
            phoneCanvasGroup.interactable = true;
            phoneCanvasGroup.blocksRaycasts = true;

            if (panelBloqueo != null)
            {
                panelBloqueo.SetActive(true);
                RectTransform bloqueoTransform = panelBloqueo.GetComponent<RectTransform>();
                CanvasGroup bloqueoCanvasGroup = panelBloqueo.GetComponent<CanvasGroup>();

                if (bloqueoTransform != null)
                    bloqueoTransform.anchoredPosition = Vector2.zero;

                if (bloqueoCanvasGroup != null)
                    bloqueoCanvasGroup.alpha = 1f;
            }

            ActivateOnlyPanel(panelBloqueo);

            if (follower != null) follower.followEnabled = false;
            phoneAnimator.Play("PhoneAppear", -1, 0f);
            float animLength = phoneAnimator.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(EnableUIAfterAnimation(animLength, follower));

            //movimientoJugador.enabled = false;
            //camaraJugador.enabled = false;
        }
        else
        {
            isPhoneActive = false;
            StartCoroutine(ResetAndFadeOut(follower));
            //movimientoJugador.enabled = true;
            //camaraJugador.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }



    IEnumerator EnableUIAfterAnimation(float delay, FollowPlayer follower)
    {
        yield return new WaitForSeconds(delay);

        phoneUI.SetActive(true);
        pantallaRecorte.SetActive(true);

        if (phoneCanvasGroup != null)
        {
            phoneCanvasGroup.alpha = 1f;
            phoneCanvasGroup.interactable = true;
            phoneCanvasGroup.blocksRaycasts = true;
        }

        ActivateOnlyPanel(panelBloqueo);

        phoneModel.transform.localPosition = phoneStartPosition;
        phoneModel.transform.localScale = phoneStartScale;
        flashlight?.SetActive(false);
        cameraCelular.enabled = false;
        if (follower != null) follower.followEnabled = true;
        isAnimating = false;

    }

    IEnumerator ResetAndFadeOut(FollowPlayer follower)
    {
        ActivateOnlyPanel(panelBloqueo);

        if (follower != null)
            follower.followEnabled = false;

        phoneModel.transform.localPosition = phoneStartPosition;
        phoneModel.transform.localScale = phoneStartScale;
        flashlight?.SetActive(false);
        if (cameraCelular != null)
            cameraCelular.enabled = false;

        yield return StartCoroutine(FadeCanvas(0f));

        phoneAnimator.Play("PhoneDisappear", -1, 0f);
        float animLength = phoneAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);

        phoneUI.SetActive(false);
        pantallaRecorte.SetActive(false);
        phoneModel.SetActive(false);

       
        isAnimating = false;
    }


    IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = phoneCanvasGroup.alpha;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            phoneCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        phoneCanvasGroup.alpha = targetAlpha;
    }

    public void ToggleFlashlight() => flashlight?.SetActive(!flashlight.activeSelf);
    public void OpenCameraFromLockScreen() => ActivateOnlyPanel(panelCamara);
    public void ReturnToLockScreen() => ActivateOnlyPanel(panelBloqueo);
    public void OpenAppLuces() => ActivateOnlyPanel(appLuces);

    public void VolverAlInicioDesdeApp()
    {
        ActivateOnlyPanel(panelInicio);
        pantallaRecorte.SetActive(true);
        phoneUI.SetActive(true);
        cameraCelular.enabled = false;
    }

    void DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0)) startTouchPosition = Input.mousePosition;
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            float swipeDistance = (endTouchPosition - startTouchPosition).magnitude;
            if (swipeDistance >= 50f && Mathf.Abs(endTouchPosition.y - startTouchPosition.y) > Mathf.Abs(endTouchPosition.x - startTouchPosition.x))
            {
                if (endTouchPosition.y > startTouchPosition.y)
                    StartCoroutine(UnlockPhoneWithAnimation());
            }
        }
    }

    public void OpenAppMensajes()
    {
        ActivateOnlyPanel(appMensajes);

        if (pantallaRecorte != null)
            pantallaRecorte.SetActive(true);

        if (phoneUI != null)
            phoneUI.SetActive(true);

        if (cameraCelular != null)
            cameraCelular.enabled = false;
        if (appMensajes != null)
        {
            CanvasGroup cg = appMensajes.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }
    }

    public void OpenSensorMovimientoApp()
    {
        if (panelSensorMovimiento != null)
        {
            panelSensorMovimiento.SetActive(true);

            RectTransform rt = panelSensorMovimiento.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800);
            }

            CanvasGroup cg = panelSensorMovimiento.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        pantallaRecorte?.SetActive(true);
        phoneUI?.SetActive(true);
        cameraCelular.enabled = false;
    }

    public void AbrirChat1()
    {
        Debug.Log(">> Intentando abrir panelChat1");

        if (panelChat1 != null)
        {
            panelChat1.SetActive(false); 
            panelChat1.SetActive(true);  

            RectTransform rt = panelChat1.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800);
                rt.SetAsLastSibling(); 
            }

            CanvasGroup cg = panelChat1.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            UnityEngine.UI.Image bg = panelChat1.GetComponent<UnityEngine.UI.Image>();
            if (bg != null)
                bg.color = Color.white;
        }
        else
        {
            Debug.LogWarning("panelChat1 no está asignado en el Inspector.");
        }

        pantallaRecorte?.SetActive(true);
        phoneUI?.SetActive(true);
        cameraCelular.enabled = false;
    }



    public void VolverAMensajesDesdeChat()
    {
        Debug.Log("VolverAMensajesDesdeChat ejecutado");

        if (appMensajes != null)
        {
            appMensajes.SetActive(true);

            
            RectTransform rt = appMensajes.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800);
                rt.SetAsLastSibling(); 
            }

           
            CanvasGroup cg = appMensajes.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            
            UnityEngine.UI.Image img = appMensajes.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
                img.color = Color.white;
        }

        pantallaRecorte?.SetActive(true);
        phoneUI?.SetActive(true);
        cameraCelular.enabled = false;

        
        Transform chatBtn = appMensajes.transform.Find("Btn_Chat1"); 
        if (chatBtn != null)
        {
            chatBtn.gameObject.SetActive(true);

            CanvasGroup cgBtn = chatBtn.GetComponent<CanvasGroup>();
            if (cgBtn != null)
            {
                cgBtn.alpha = 1f;
                cgBtn.interactable = true;
                cgBtn.blocksRaycasts = true;
            }

            RectTransform rtBtn = chatBtn.GetComponent<RectTransform>();
            if (rtBtn != null)
            {
                rtBtn.localScale = Vector3.one;
                //rtBtn.anchoredPosition = Vector2.zero;
            }

            chatBtn.SetAsLastSibling();
            appMensajes.transform.SetAsLastSibling();

        }

        GameObject botonChat = GameObject.Find("BotonAbrirChat");
        if (botonChat != null)
        {
            botonChat.SetActive(true);

            CanvasGroup cg = botonChat.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            botonChat.transform.SetAsLastSibling(); 

            RectTransform rt = botonChat.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(500, 200); 
            }
        }
        else
        {
            Debug.LogWarning("BotonAbrirChat no encontrado.");
        }


    }

    public void AbrirChat2()
    {
        Debug.Log(">> Intentando abrir panelChat2");

        if (panelChat2 != null)
        {
            panelChat2.SetActive(false); // Reset visual
            panelChat2.SetActive(true);

            RectTransform rt = panelChat2.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800);
                rt.SetAsLastSibling();
            }

            CanvasGroup cg = panelChat2.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            UnityEngine.UI.Image bg = panelChat2.GetComponent<UnityEngine.UI.Image>();
            if (bg != null)
                bg.color = Color.white;
        }
        else
        {
            Debug.LogWarning("panelChat2 no está asignado en el Inspector.");
        }

        pantallaRecorte?.SetActive(true);
        phoneUI?.SetActive(true);
        cameraCelular.enabled = false;
    }

    public void VolverAMensajesDesdeChat2()
    {
        if (appMensajes != null)
        {
            appMensajes.SetActive(true);

            RectTransform rt = appMensajes.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800);
                rt.SetAsLastSibling();
            }

            CanvasGroup cg = appMensajes.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        pantallaRecorte?.SetActive(true);
        phoneUI?.SetActive(true);
        cameraCelular.enabled = false;
    }


    public void AbrirChat3()
    {
        Debug.Log(">> Intentando abrir panelChat3");

        if (panelChat3 != null)
        {
            panelChat3.SetActive(false); // Reset visual
            panelChat3.SetActive(true);

            RectTransform rt = panelChat3.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800);
                rt.SetAsLastSibling();
            }

            CanvasGroup cg = panelChat3.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            UnityEngine.UI.Image bg = panelChat3.GetComponent<UnityEngine.UI.Image>();
            if (bg != null)
                bg.color = Color.white;
        }
        else
        {
            Debug.LogWarning("panelChat3 no está asignado en el Inspector.");
        }

        pantallaRecorte?.SetActive(true);
        phoneUI?.SetActive(true);
        cameraCelular.enabled = false;
    }

    public void VolverAMensajesDesdeChat3()
    {
        if (appMensajes != null)
        {
            appMensajes.SetActive(true);

            RectTransform rt = appMensajes.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800);
                rt.SetAsLastSibling();
            }

            CanvasGroup cg = appMensajes.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        pantallaRecorte?.SetActive(true);
        phoneUI?.SetActive(true);
        cameraCelular.enabled = false;
    }




    public void VolverAlInicioDesdeMensajes()
    {
        ActivateOnlyPanel(panelInicio);
        if (pantallaRecorte != null) pantallaRecorte.SetActive(true);
        if (phoneUI != null) phoneUI.SetActive(true);
        if (cameraCelular != null) cameraCelular.enabled = false;
    }

    public void VolverAlInicioDesdeSensor()
    {
        ActivateOnlyPanel(panelInicio);

        if (pantallaRecorte != null)
            pantallaRecorte.SetActive(true);

        if (phoneUI != null)
            phoneUI.SetActive(true);

        if (cameraCelular != null)
            cameraCelular.enabled = false;
    }


    public void ForzarMostrarAppMensajes()
    {
        ActivateOnlyPanel(appMensajes);

        if (appMensajes != null)
        {
            appMensajes.SetActive(true);

            RectTransform rt = appMensajes.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(1000, 1800); 
            }

            CanvasGroup cg = appMensajes.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            Debug.Log("AppMensajes mostrada manualmente");
        }

        if (pantallaRecorte != null)
            pantallaRecorte.SetActive(true);
    }


    public void MostrarNotificacionDesdeSensor(string zona)
    {
        if (!isPhoneActive || (panelBloqueo != null && panelBloqueo.activeSelf))
        {
            Debug.Log("Mostrando notificación en pantalla de bloqueo: " + zona);

            if (panelNotificacionBloqueo != null)
            {
                panelNotificacionBloqueo.SetActive(true);

                if (notificacionCoroutine != null)
                    StopCoroutine(notificacionCoroutine);

                notificacionCoroutine = StartCoroutine(DesactivarNotificacionBloqueo());

                StartCoroutine(DesactivarNotificacionBloqueo());
            }
        }
    }

    IEnumerator DesactivarNotificacionBloqueo()
    {
        yield return new WaitForSeconds(4f); 
        if (panelNotificacionBloqueo != null)
            panelNotificacionBloqueo.SetActive(false);
    }




    IEnumerator UnlockPhoneWithAnimation()
    {
        if (panelBloqueo != null && panelInicio != null)
        {
            ActivateOnlyPanel(panelInicio);

            RectTransform bloqueoTransform = panelBloqueo.GetComponent<RectTransform>();
            CanvasGroup bloqueoCanvasGroup = panelBloqueo.GetComponent<CanvasGroup>();

            Vector2 startPos = bloqueoTransform.anchoredPosition;
            Vector2 endPos = startPos + new Vector2(0, 1000);
            float elapsedTime = 0f;

            while (elapsedTime < unlockAnimationDuration)
            {
                float t = elapsedTime / unlockAnimationDuration;
                bloqueoTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                if (bloqueoCanvasGroup != null)
                    bloqueoCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            bloqueoTransform.anchoredPosition = endPos;
            if (bloqueoCanvasGroup != null) bloqueoCanvasGroup.alpha = 0f;
            panelBloqueo.SetActive(false);

            StartCoroutine(AnimatePhoneToCenter());
        }
    }

    IEnumerator AnimatePhoneToCenter()
    {
        float elapsedTime = 0f;
        Vector3 startPos = phoneModel.transform.localPosition;
        Vector3 startScale = phoneModel.transform.localScale;
        while (elapsedTime < phoneMoveDuration)
        {
            float t = elapsedTime / phoneMoveDuration;
            phoneModel.transform.localPosition = Vector3.Lerp(startPos, phoneEndPosition, t);
            phoneModel.transform.localScale = Vector3.Lerp(startScale, phoneEndScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        phoneModel.transform.localPosition = phoneEndPosition;
        phoneModel.transform.localScale = phoneEndScale;
    }

    IEnumerator AnimatePhoneToStart()
    {
        float elapsedTime = 0f;
        Vector3 startPos = phoneModel.transform.localPosition;
        Vector3 startScale = phoneModel.transform.localScale;
        while (elapsedTime < phoneMoveDuration)
        {
            float t = elapsedTime / phoneMoveDuration;
            phoneModel.transform.localPosition = Vector3.Lerp(startPos, phoneStartPosition, t);
            phoneModel.transform.localScale = Vector3.Lerp(startScale, phoneStartScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        phoneModel.transform.localPosition = phoneStartPosition;
        phoneModel.transform.localScale = phoneStartScale;
    }

    void ActivateOnlyPanel(GameObject panelToActivate)
    {
        panelBloqueo?.SetActive(false);
        panelInicio?.SetActive(false);
        panelCamara?.SetActive(false);
        appLuces?.SetActive(false);
        appMensajes?.SetActive(false);
        panelSensorMovimiento?.SetActive(false);
        panelChat1?.SetActive(false);
        panelChat2?.SetActive(false);   
        panelChat3?.SetActive(false);   

        if (panelToActivate != null)
            panelToActivate.SetActive(true);

        if (cameraCelular != null)
            cameraCelular.enabled = (panelToActivate == panelCamara);

        if (appMensajes != null) appMensajes.SetActive(false);

        if (panelSensorMovimiento != null) panelSensorMovimiento.SetActive(false);

        if (panelChat1 != null) panelChat1.SetActive(false);

        if (panelChat2 != null) panelChat2.SetActive(false);
        if (panelChat3 != null) panelChat3.SetActive(false);




    }
}
