using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CrystalPuzzleUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public TextMeshProUGUI orbTitleText;
    public TextMeshProUGUI selectedLetterText;

    [Header("Puzzle Settings")]
    public string targetWord = "ESCAPE";

    private CrystalOrbInteractable currentOrb;
    private int currentLetterIndex = 0;
    private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private char[] selectedLetters;
    private bool isOpen = false;

    [Header("Orb Names")]
    public string[] orbNames = new string[]
    {
        "Sloth",
        "Anger",
        "Envy",
        "Gluttony",
        "Lust",
        "Pride"
    };

    private void Awake()
    {
        targetWord = targetWord.ToUpper();

        selectedLetters = new char[targetWord.Length];
        for (int i = 0; i < selectedLetters.Length; i++)
        {
            selectedLetters[i] = 'A';
        }
    }

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    private void Update()
    {
        if (!isOpen) return;

        bool closePressed = false;

        if (Keyboard.current != null)
            closePressed = Keyboard.current.escapeKey.wasPressedThisFrame;
        else
            closePressed = Input.GetKeyDown(KeyCode.Escape);

        if (closePressed)
            ClosePanel();
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public void OpenPanel(CrystalOrbInteractable orb)
    {
        if (orb == null) return;

        currentOrb = orb;

        char currentLetter = GetLetter(orb.orbIndex);
        int foundIndex = alphabet.IndexOf(currentLetter);
        currentLetterIndex = foundIndex >= 0 ? foundIndex : 0;

        if (panel != null)
            panel.SetActive(true);

        RefreshUI();

        isOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePanel()
    {
        if (panel != null)
            panel.SetActive(false);

        currentOrb = null;
        isOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void NextLetter()
    {
        currentLetterIndex++;
        if (currentLetterIndex >= alphabet.Length)
            currentLetterIndex = 0;

        RefreshUI();
    }

    public void PreviousLetter()
    {
        currentLetterIndex--;
        if (currentLetterIndex < 0)
            currentLetterIndex = alphabet.Length - 1;

        RefreshUI();
    }

    public void SaveLetter()
    {
        if (currentOrb == null) return;

        char chosen = alphabet[currentLetterIndex];
        SetLetter(currentOrb.orbIndex, chosen);
        ClosePanel();
    }

    public char GetLetter(int index)
    {
        if (index < 0 || index >= selectedLetters.Length)
            return 'A';

        return selectedLetters[index];
    }

    public void SetLetter(int index, char letter)
    {
        if (index < 0 || index >= selectedLetters.Length)
            return;

        selectedLetters[index] = char.ToUpper(letter);
    }

    public string GetCurrentWord()
    {
        return new string(selectedLetters);
    }

    public bool IsCorrectWord()
    {
        return GetCurrentWord() == targetWord;
    }

    public int GetLetterCount()
    {
        return selectedLetters.Length;
    }

    private void RefreshUI()
    {
        if (currentOrb != null && orbTitleText != null)
        {
            string orbName = "Unknown";

            if (currentOrb.orbIndex >= 0 && currentOrb.orbIndex < orbNames.Length)
                orbName = orbNames[currentOrb.orbIndex];

            orbTitleText.text = "Orb of " + orbName;
        }

        if (selectedLetterText != null)
            selectedLetterText.text = alphabet[currentLetterIndex].ToString();
    }
}