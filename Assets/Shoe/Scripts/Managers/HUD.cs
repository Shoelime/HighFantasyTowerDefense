using System;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour, IUIElementSound
{
    [SerializeField] private TextMeshProUGUI goldTextGUI;
    [SerializeField] private TextMeshProUGUI nextWaveTimerTextGUI;
    [SerializeField] private TextMeshProUGUI defeatText;
    [SerializeField] private TextMeshProUGUI gameSpeedText;

    [SerializeField] private SoundData onUIDeSelectSound;
    [SerializeField] private SoundData onUISelectSound;

    private AudioSource audioSource;
    private VictoryCanvas victoryCanvas;    

    public static event Action OnRestartButton;
    public static event Action NextWave;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Services.Get<IEconomicsManager>().GoldAmountChanged += UpdateGoldText;
        Services.Get<IGameManager>().VictoryEvent += DisplayVictoryCanvas;
        Services.Get<IGameManager>().DefeatEvent += DisplayDefeatText;

        victoryCanvas = GetComponentInChildren<VictoryCanvas>();

        UpdateGoldText(Services.Get<IEconomicsManager>().GetCurrentGoldAmount());
        InvokeRepeating(nameof(UpdateWaveTimerText), 0, 1);
    }

    private void UpdateGoldText(int goldCount)
    {
        goldTextGUI.text = goldCount.ToString();
    }

    private void UpdateWaveTimerText()
    {
        int secondsTillNextWave = Mathf.FloorToInt(Services.Get<IWaveManager>().TimeUntilNextWave);
        nextWaveTimerTextGUI.text = secondsTillNextWave.ToString();
    }

    void DisplayVictoryCanvas()
    {
        victoryCanvas.TriggerCanvas();
    }

    void DisplayDefeatText()
    {
        defeatText.gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        OnRestartButton?.Invoke();
    }

    public void GameSpeedButton()
    {
        NextWave?.Invoke();
        OnUIElementOpened();
    }

    void OnDisable()
    {
        Services.Get<IEconomicsManager>().GoldAmountChanged -= UpdateGoldText;
        Services.Get<IGameManager>().VictoryEvent -= DisplayVictoryCanvas;
        Services.Get<IGameManager>().DefeatEvent -= DisplayDefeatText;

        CancelInvoke(nameof(UpdateWaveTimerText));
    }

    public void OnUIElementOpened()
    {
        SoundManager.Instance.PlaySound(audioSource, onUISelectSound, 1);
    }

    public void OnUIElementClosed()
    {
        SoundManager.Instance.PlaySound(audioSource, onUIDeSelectSound, 1);
    }
}
