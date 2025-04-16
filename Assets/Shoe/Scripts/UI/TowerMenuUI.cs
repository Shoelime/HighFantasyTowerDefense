using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuUI : MonoBehaviour, IUIElementSound
{
    [SerializeField] private Button TowerButtonPrefab;
    [SerializeField] private float menuOffset;
    [SerializeField] private SoundData onUIDeSelectSound;
    [SerializeField] private SoundData onUISelectSound;

    private Button[] towerButtons;
    private TowerButtonUI[] towerButtonUIArray;
    public TowerPlate SelectedTowerPlate { get; private set; }
    private TowerData[] towers;
    private RectTransform rectTransform;

    private AudioSource audioSource;
    private TowerRangeDisplayer towerRangeDisplayer;
    public bool Upgrading { get; private set; }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        audioSource = GetComponentInParent<AudioSource>();
        towerRangeDisplayer = GetComponent<TowerRangeDisplayer>();
        towerRangeDisplayer.Initialize(this);

        CreateTowerDatabase();
        SetButtons();
        HideButtons();

        Services.Get<IEconomicsManager>().GoldAmountChanged += UpdateButtons;
    }

    void CreateTowerDatabase()
    {
        var obj = Resources.LoadAll("", typeof(TowerData));
        towers = new TowerData[obj.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            towers[i] = (TowerData)obj[i];
        }

        towers = towers.OrderBy(tower => tower.name).ToArray();
    }

    void SetButtons()
    {
        towerButtons = new Button[4];
        towerButtonUIArray = new TowerButtonUI[4];

        for (int i = 0; i < 4; i++)
        {
            int index = i;

            towerButtons[index] = Instantiate(TowerButtonPrefab, Vector3.zero, Quaternion.identity, this.transform);
            towerButtonUIArray[index] = towerButtons[index].GetComponent<TowerButtonUI>();
            towerButtonUIArray[index].SetTowerData(towers[index]);

            TowerButtonUI buttonUI = towerButtonUIArray[index];

            towerButtons[index].onClick.AddListener(() => OnTowerButtonClick(buttonUI.TowerData));
            buttonUI.ButtonSelected += DisplayTowerRange;
            buttonUI.ButtonUnselected += towerRangeDisplayer.HideTowerRange;
        }
    }

    private void DisplayTowerRange(TowerData data)
    {
        if (SelectedTowerPlate.ContainsTower())
            towerRangeDisplayer.DisplayTowerRange(SelectedTowerPlate.PlacedTower.TowerData);
        else towerRangeDisplayer.DisplayTowerRange(data);
    }

    void OnTowerButtonClick(TowerData data)
    {
        bool success;

        if (SelectedTowerPlate.ContainsTower())
            SelectedTowerPlate.UpgradeTower(out success);
        else
            SelectedTowerPlate.BuildTower(data, out success);

        OnUIElementClosed();

        if (success)
            TryHideButtons(SelectedTowerPlate);
    }

    void UpdateButtons(int goldCount)
    {
        if (SelectedTowerPlate == null)
            return;

        for (int i = 0; i < towerButtonUIArray.Length; i++)
        {
            towerButtonUIArray[i].SetCostTextColor(Upgrading, SelectedTowerPlate);

            if (Upgrading)
            {
                break;
            }
        }
    }

    void TryHideButtons(TowerPlate plate)
    {
        if (SelectedTowerPlate == plate)
        {
            HideButtons();
        }
    }

    void HideButtons()
    {
        SelectedTowerPlate = null;

        foreach (var towerButton in towerButtons)
        {
            towerButton.gameObject.SetActive(false);
        }

        towerRangeDisplayer.HideTowerRange();
        SelectedTowerPlate = null;

        OnUIElementClosed();
    }

    void DisplayButtons(TowerPlate plate)
    {
        Upgrading = false;

        if (plate.ContainsTower())
        {
            if (plate.PlacedTower.IsMaxLevel() || plate.PlacedTower.IsBeingBuilt)
                return;

            Upgrading = true;
            towerButtons[0].gameObject.SetActive(true);

            ButtonsOpened(plate);
            return;
        }

        foreach (var towerButton in towerButtons)
        {
            towerButton.gameObject.SetActive(true);
        }

        ButtonsOpened(plate);
    }

    void ButtonsOpened(TowerPlate plate)
    {
        PositionMenu();
        SelectedTowerPlate = plate;
        UpdateButtons(Services.Get<IEconomicsManager>().GetCurrentGoldAmount());
        OnUIElementOpened();
    }

    private void PositionMenu()
    {
        Vector2 mousePosition = Services.Get<IInputManager>().MousePosition;

        // Convert screen position to Canvas/RectTransform position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent.GetComponent<RectTransform>(),
            mousePosition,
            null,
            out Vector2 localPoint);

        // Apply offset to the position
        localPoint.y += menuOffset;

        // Set the panel's position
        rectTransform.anchoredPosition = localPoint;
    }

    private void OnEnable()
    {
        TowerPlate.PlateSelected += DisplayButtons;
        TowerPlate.PlateUnSelected += TryHideButtons;
    }

    private void OnDisable()
    {
        Upgrading = false;

        TowerPlate.PlateSelected -= DisplayButtons;
        TowerPlate.PlateUnSelected -= TryHideButtons;
        Services.Get<IEconomicsManager>().GoldAmountChanged -= UpdateButtons;

        foreach (var buttonUI in towerButtonUIArray)
        {
            buttonUI.ButtonSelected -= towerRangeDisplayer.DisplayTowerRange;
            buttonUI.ButtonUnselected -= towerRangeDisplayer.HideTowerRange;
        }
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
