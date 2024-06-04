using UnityEngine;

public class HealthBarPool : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        WaveManager.SpawnedEnemy += CreateHealthBar;
    }

    private void CreateHealthBar(EnemyCharacter obj)
    {
        HealthBar instansiatedHealthbar = healthBar.Get<HealthBar>(Vector3.zero, Quaternion.identity);
        instansiatedHealthbar.SetCanvas(canvas);
        instansiatedHealthbar.HealthbarTarget(obj.transform);
        obj.AssignHealthBar(instansiatedHealthbar);
    }

    private void OnDestroy()
    {
        WaveManager.SpawnedEnemy -= CreateHealthBar;
    }
}