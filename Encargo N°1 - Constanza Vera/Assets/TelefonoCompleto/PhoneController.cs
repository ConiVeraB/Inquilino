using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour
{
    [Header("Objetos principales")]
    public GameObject phoneModel;
    public Animator phoneAnimator;
    public GameObject phoneUI;
    public CanvasGroup phoneCanvasGroup;

    [Header("Configuraciones de linterna")]
    public GameObject flashlight;

    [Header("Pantallas")]
    public GameObject pantallaRecorte;
    public GameObject panelBloqueo;
    public GameObject panelInicio;
    public GameObject panelCamara;
    public GameObject appLuces; 

    [Header("Control de Jugador")]
    public PlayerController movimientoJugador;
    public FirstPersonCamera camaraJugador;





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

        if (flashlight != null)
            flashlight.SetActive(false);

        if (pantallaRecorte != null)
            pantallaRecorte.SetActive(false);

        if (cameraCelular != null)
            cameraCelular.enabled = false;

        if (phoneModel != null)
        {
            phoneModel.transform.localPosition = phoneStartPosition;
            phoneModel.transform.localScale = phoneStartScale;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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
        if (isAnimating) return; // Evita spameo durante animación

        var follower = phoneModel.GetComponent<FollowPlayer>();

        if (!isPhoneActive)
        {
            isPhoneActive = true;
            isAnimating = true;

            phoneModel.transform.localScale = phoneStartScale; // Asegura escala
            phoneModel.SetActive(true);
            pantallaRecorte.SetActive(true);

            if (follower != null) follower.followEnabled = false;

            phoneAnimator.Play("PhoneAppear", -1, 0f);

            float animationLength = phoneAnimator.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(EnableUIAfterAnimation(animationLength, follower));
            isAnimating = false;

            // Bloquear o desbloquear control del jugador
            if (movimientoJugador != null)
                movimientoJugador.enabled = !isPhoneActive;

            if (camaraJugador != null)
                camaraJugador.enabled = !isPhoneActive;

            Cursor.visible = isPhoneActive;
            Cursor.lockState = isPhoneActive ? CursorLockMode.None : CursorLockMode.Locked;

            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;


        }
        else
        {
            isPhoneActive = false;
            isAnimating = true;

            StartCoroutine(ResetAndFadeOut(follower));
            isAnimating = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;


        }
    }

    public void OpenAppLuces()
    {
        Debug.Log("Abriendo App de Luces...");
        ActivateOnlyPanel(appLuces);
    }

    public void VolverAlInicioDesdeApp()
    {
        if (panelInicio != null)
            panelInicio.SetActive(true);

        if (panelCamara != null)
            panelCamara.SetActive(false);

        if (pantallaRecorte != null)
            pantallaRecorte.SetActive(true);

        if (phoneUI != null)
            phoneUI.SetActive(true);

        // Desactiva cualquier otra app abierta
        if (panelBloqueo != null) panelBloqueo.SetActive(false);
        if (panelCamara != null) panelCamara.SetActive(false);
        if (appLuces != null) appLuces.SetActive(false);

        // Apaga cámara física
        if (cameraCelular != null)
            cameraCelular.enabled = false;
    }




    IEnumerator EnableUIAfterAnimation(float delay, FollowPlayer follower)
    {
        yield return new WaitForSeconds(delay);

        if (phoneUI != null)
            phoneUI.SetActive(true);

        if (phoneCanvasGroup != null)
        {
            phoneCanvasGroup.alpha = 0f;
            StartCoroutine(FadeCanvas(1f));
        }

        if (pantallaRecorte != null)
            pantallaRecorte.SetActive(true);

        ActivateOnlyPanel(panelBloqueo);

        if (follower != null)
            follower.followEnabled = true;

        phoneModel.transform.localPosition = phoneStartPosition;
        phoneModel.transform.localScale = phoneStartScale;

        if (flashlight != null)
            flashlight.SetActive(false);

        if (cameraCelular != null)
            cameraCelular.enabled = false;
    }

    IEnumerator ResetAndFadeOut(FollowPlayer follower)
    {
        ActivateOnlyPanel(panelBloqueo);

        if (follower != null)
            follower.followEnabled = false;

        phoneModel.transform.localPosition = phoneStartPosition;
        phoneModel.transform.localScale = phoneStartScale;

        if (flashlight != null)
            flashlight.SetActive(false);

        if (cameraCelular != null)
            cameraCelular.enabled = false;

        yield return StartCoroutine(FadeCanvas(0f));

        phoneAnimator.Play("PhoneDisappear", -1, 0f);

        float animationLength = phoneAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        phoneUI.SetActive(false);
        pantallaRecorte.SetActive(false);
        phoneModel.SetActive(false);
    }

    IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = phoneCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            phoneCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        phoneCanvasGroup.alpha = targetAlpha;
    }

    public void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.SetActive(!flashlight.activeSelf);
        }
    }

    public void OpenCameraFromLockScreen()
    {
        ActivateOnlyPanel(panelCamara);
    }

    public void ReturnToLockScreen()
    {
        ActivateOnlyPanel(panelBloqueo);
    }


    void DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            float swipeDistance = (endTouchPosition - startTouchPosition).magnitude;

            if (swipeDistance >= 50f)
            {
                Vector2 direction = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && direction.y > 0)
                {
                    StartCoroutine(UnlockPhoneWithAnimation());
                }
            }
        }
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

            float startAlpha = 1f;
            float endAlpha = 0f;
            float elapsedTime = 0f;

            while (elapsedTime < unlockAnimationDuration)
            {
                float t = elapsedTime / unlockAnimationDuration;
                bloqueoTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

                if (bloqueoCanvasGroup != null)
                    bloqueoCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            bloqueoTransform.anchoredPosition = endPos;

            if (bloqueoCanvasGroup != null)
                bloqueoCanvasGroup.alpha = 0f;

            panelBloqueo.SetActive(false);

            var follower = phoneModel.GetComponent<FollowPlayer>();
            if (follower != null) follower.followEnabled = false;

            StartCoroutine(AnimatePhoneToCenterAndReactivateFollower(follower));

        }
    }

    IEnumerator AnimatePhoneToCenterAndReactivateFollower(FollowPlayer follower)
    {
        float elapsedTime = 0f;

        Vector3 startPos = phoneModel.transform.position;
        Vector3 endPos = Camera.main.transform.position + Camera.main.transform.forward * 0.4f + Camera.main.transform.up * -0.2f;

        Vector3 startScale = phoneModel.transform.localScale;
        Vector3 endScale = new Vector3(0.18f, 0.18f, 0.18f); // ajusta según tu modelo real

        while (elapsedTime < phoneMoveDuration)
        {
            float t = elapsedTime / phoneMoveDuration;

            phoneModel.transform.position = Vector3.Lerp(startPos, endPos, t);
            phoneModel.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        phoneModel.transform.position = endPos;
        phoneModel.transform.localScale = endScale;

        if (follower != null) follower.followEnabled = true;
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
        if (panelBloqueo != null) panelBloqueo.SetActive(false);
        if (panelInicio != null) panelInicio.SetActive(false);
        if (panelCamara != null) panelCamara.SetActive(false);
        if (appLuces != null) appLuces.SetActive(false); 

        if (panelToActivate != null)
            panelToActivate.SetActive(true);

        if (cameraCelular != null)
            cameraCelular.enabled = (panelToActivate == panelCamara);
    }

}
