using TMPro;
using UnityEngine;

public class GameScreen : ScreenBase
{
    public const string PATH = "Prefabs/UI/Screens/GameScreen";
    public const string LEVEL_PREFIX = "LEVEL";

    [SerializeField] private TMP_Text m_levelNumber;

    private void Start()
    {
        int levelNumber = GameManager.CurrentLevelIndex;
        levelNumber++;
        m_levelNumber.text = $"{LEVEL_PREFIX} {levelNumber}";
    }
}