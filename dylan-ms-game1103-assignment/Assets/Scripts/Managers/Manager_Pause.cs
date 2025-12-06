using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Manager_Pause : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject quitConfirmationUI;

    public static bool IsPaused { get; private set; }

    private InputSystem_Actions _inputActions;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        _inputActions.UI.Enable();
        _inputActions.Player.Enable();
    }

    void OnDisable()
    {
        _inputActions.UI.Disable();
        _inputActions.Player.Disable();
    }

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        if (quitConfirmationUI != null)
        {
            quitConfirmationUI.SetActive(false);
        }
        IsPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (_inputActions.Player.Pause.WasPressedThisFrame())
        {
            if (quitConfirmationUI != null && quitConfirmationUI.activeInHierarchy)
            {
                CancelQuit();
                return;
            }
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (quitConfirmationUI != null && quitConfirmationUI.activeInHierarchy)
        {
            return;
        }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void ShowQuitConfirmation()
    {
        if (quitConfirmationUI != null)
        {
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
            quitConfirmationUI.SetActive(true);
        }
    }

    public void CancelQuit()
    {
        if (quitConfirmationUI != null)
        {
            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
            quitConfirmationUI.SetActive(false);
        }
    }

    public void ConfirmQuit()
    {
        Time.timeScale = 1f;
        Manager_Transition.Instance.LoadScene("Menu_Main");
    }
}