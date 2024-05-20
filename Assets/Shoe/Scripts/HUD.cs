using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTextGUI;
    [SerializeField] private TextMeshProUGUI nextWaveTimerTextGUI;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI defeatText;

    void Start()
    {
        Services.Get<IEconomicsManager>().GoldAmountChanged += UpdateGoldText;
        Services.Get<IGameManager>().VictoryEvent += VictoryText;

        UpdateGoldText(Services.Get<IEconomicsManager>().GetCurrentGoldAmount());
        InvokeRepeating(nameof(UpdateWaveTimerText), 1, 1);
    }

    private void UpdateGoldText(int goldCount)
    {
        goldTextGUI.text = goldCount.ToString();
    }

    private void UpdateWaveTimerText()
    {
        //nextWaveTimerTextGUI.text = "0";
    }

    void VictoryText()
    {
        victoryText.gameObject.SetActive(true);
    }

    void DefeatText()
    {

    }

    void OnDisable()
    {
        Services.Get<IEconomicsManager>().GoldAmountChanged -= UpdateGoldText;
    }
}
