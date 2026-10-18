using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class NPCTextBox : MonoBehaviour
{
    public List<string> lines;
    int lineIndex;
    float timer = 3.5f;
    public TextMeshProUGUI textBox;
    public SquashAndStretch squashAndStretch;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3.5f && lineIndex < lines.Count)
        {
            timer = 0;
            textBox.text = lines[lineIndex];
            lineIndex++;
            squashAndStretch.Play();
        }
    }
}
