using UnityEngine;
using TMPro;
public class QuestManager : MonoBehaviour
{
     public static QuestManager Instance;
    public TextMeshProUGUI questText;
    private int generatorsFixed = 0;
    private int totalGenerators = 3;

    void Awake()
    {
        Instance = this;
        UpdateQuestText();
    }

    public void AddGeneratorFixed()
    {
        generatorsFixed++;
        UpdateQuestText();

        if (generatorsFixed >= totalGenerators)
        {
            CompleteQuest();
        }
    }

    void UpdateQuestText()
    {
        questText.text = $"Ativar Geradores: {generatorsFixed}/{totalGenerators}";
    }

    void CompleteQuest()
    {
        questText.text = "✅ Todos os geradores foram ativados!";
        Debug.Log("Quest completa!");
    }
}