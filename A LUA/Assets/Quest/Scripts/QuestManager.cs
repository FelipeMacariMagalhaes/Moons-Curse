using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public int totalGenerators = 3;
    public int completedGenerators = 0;

    public TextMeshProUGUI questText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateQuestText();
    }

    public void AddGeneratorCompleted()
    {
        completedGenerators++;
        UpdateQuestText();
    }

    void UpdateQuestText()
    {
        if (questText != null)
            questText.text = completedGenerators + " / " + totalGenerators;
    }
}
