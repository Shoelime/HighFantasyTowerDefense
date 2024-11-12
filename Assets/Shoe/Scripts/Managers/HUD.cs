using System;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour, IUIElementSound
{
    [SerializeField] private TextMeshProUGUI goldTextGUI;
    [SerializeField] private TextMeshProUGUI nextWaveTimerTextGUI;
    [SerializeField] private TextMeshProUGUI WaveCountTextGUI;
    [SerializeField] private TextMeshProUGUI defeatText;
    [SerializeField] private TextMeshProUGUI gameSpeedText;

    [SerializeField] private GameObject skipToWaveButton;

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
        Services.Get<IWaveManager>().NewWaveStarted += UpdateWaveCounter;

        victoryCanvas = GetComponentInChildren<VictoryCanvas>();

        UpdateGoldText(Services.Get<IEconomicsManager>().GetCurrentGoldAmount());
        InvokeRepeating(nameof(UpdateWaveTimerText), 0, 1);

        UpdateWaveCounter();
    }

    private void UpdateGoldText(int goldCount)
    {
        goldTextGUI.text = goldCount.ToString();
    }

    private void UpdateWaveTimerText()
    {
        if (Services.Get<IWaveManager>().IsLastWave())
        {
            nextWaveTimerTextGUI.gameObject.SetActive(false);
            skipToWaveButton.SetActive(false);

            CancelInvoke(nameof(UpdateWaveTimerText));
            return;
        }

        int secondsTillNextWave = Mathf.FloorToInt(Services.Get<IWaveManager>().TimeUntilNextWave);
        nextWaveTimerTextGUI.text = secondsTillNextWave.ToString();
    }

    private void UpdateWaveCounter()
    {
        (int, int) waveCounter = Services.Get<IWaveManager>().WaveCounter();
        WaveCountTextGUI.text = waveCounter.Item1.ToString() + " / " + waveCounter.Item2.ToString();
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
        Services.Get<IWaveManager>().NewWaveStarted -= UpdateWaveCounter;

        CancelInvoke(nameof(UpdateWaveTimerText));
    }

    public void OnUIElementOpened()
    {
        Services.Get<ISoundManager>().PlaySound(audioSource, onUISelectSound, 1);
    }

    public void OnUIElementClosed()
    {
        Services.Get<ISoundManager>().PlaySound(audioSource, onUIDeSelectSound, 1);
    }
}
