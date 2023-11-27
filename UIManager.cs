using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;



    public Canvas gameCanvas;
    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
        
    }
    private void OnEnable()
    {
        CharacterEvents.characterDamaged += CharaterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }
    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharaterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    public void CharaterTookDamage(GameObject charactor, int damagedReceive)
    {
        // Create text at character hit 
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(charactor.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = damagedReceive.ToString();
    }

    public void CharacterHealed(GameObject charactor, int healthRestore)
    {
        // Create text at character hit 
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(charactor.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = healthRestore.ToString();

    }

    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
                #if (UNITY_EDITOR || DEVELOPMENT_BUILT)
                            Debug.Log(this.name + ":" + this.GetType() + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                #endif

                #if (UNITY_EDITOR)
                            UnityEditor.EditorApplication.isPlaying = false;
                #elif (UNITY_STANDALONE)
                        Application.Quit();
                #elif (UNITY_WEBGL)
                        SceneManager.LoadScene("Quit Scene");
                #endif
        }
    }
}
