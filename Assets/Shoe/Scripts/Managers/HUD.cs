using System;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour, IUIElementSound
{
    [SerializeField] private TextMeshProUGUI goldTextGUI;
    [SerializeField] private TextMeshProUGUI nextWaveTimerTextGUI;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI defeatText;
    [SerializeField] private TextMeshProUGUI gameSpeedText;

    [SerializeField] private SoundData onUIDeSelectSound;
    [SerializeField] private SoundData onUISelectSound;

    private AudioSource audioSource;
    public static event Action OnRestartButton;
    public static event Action NextWave;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Services.Get<IEconomicsManager>().GoldAmountChanged += UpdateGoldText;
        Services.Get<IGameManager>().VictoryEvent += VictoryText;
        Services.Get<IGameManager>().DefeatEvent += DefeatText;

        UpdateGoldText(Services.Get<IEconomicsManager>().GetCurrentGoldAmount());
        InvokeRepeating(nameof(UpdateWaveTimerText), 0, 1);
    }

    private void UpdateGoldText(int goldCount)
    {
        goldTextGUI.text = goldCount.ToString();
    }

    private void UpdateWaveTimerText()
    {
        int secondsTillNextWave = Mathf.FloorToInt(WaveManager.Instance.TimeUntilNextWave);
        nextWaveTimerTextGUI.text = secondsTillNextWave.ToString();
    }

    void VictoryText()
    {
        victoryText.gameObject.SetActive(true);
    }

    void DefeatText()
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
        //switch (Time.timeScale)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        Services.Get<ITimeManager>().SetTimeScale(2);
        //        break;
        //    case 2:
        //        Services.Get<ITimeManager>().SetTimeScale(3);
        //        break;
        //    case 3:
        //        Services.Get<ITimeManager>().SetTimeScale(1);
        //        break;
        //}

        //gameSpeedText.text = Time.timeScale.ToString() + "x";
        OnUIElementOpened();
    }

    void OnDisable()
    {
        Services.Get<IEconomicsManager>().GoldAmountChanged -= UpdateGoldText;
        Services.Get<IGameManager>().VictoryEvent -= VictoryText;
        Services.Get<IGameManager>().DefeatEvent -= DefeatText;
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
