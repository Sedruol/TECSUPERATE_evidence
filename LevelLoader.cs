using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    [SerializeField] private Animator animatorController;

    [HideInInspector] public UnityEvent onRestartLevel;
    [HideInInspector] public UnityEvent onExitToMenu;
    [HideInInspector] public UnityEvent onNextLevelLoad;
    [HideInInspector] public UnityEvent onLoadMinigame;
    [HideInInspector] public UnityEvent onExitGame;

    [HideInInspector] public UnityEvent onGetActiveScene;
    private string currentScene;
    private bool sceneChanged = false;
    public string CurrentScene{
        get{return currentScene;}
        set{
            currentScene = value;
            sceneChanged = true;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else{
            Destroy(this.gameObject);
        }
    }


    IEnumerator LoadScene(int index)
    {
        animatorController.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(index);
    }

    public void RestartLevel(){
        onRestartLevel?.Invoke();

        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void ExitToMainMenu(){
        onExitToMenu?.Invoke();

        StartCoroutine(LoadSceneByName("MainMapTest"));
    }

    public void CallLoadScene(int _index = -1)
    {   
        onNextLevelLoad?.Invoke();
        
        if(sceneChanged){
            StartCoroutine(LoadSceneByName(currentScene));
        }else{
            if(_index == -1){
                _index = SceneManager.GetActiveScene().buildIndex;
            }

            StartCoroutine(LoadScene(_index));
        }
    }

    public void LoadMiniGame(string name){
        onLoadMinigame?.Invoke();

        StartCoroutine(LoadSceneByName(name));
    }

    IEnumerator LoadSceneByName(string sceneName)
    {
        animatorController.SetTrigger("Start");
        
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ExitGame(){
        onExitGame?.Invoke();

        StartCoroutine(SimpleLoadExit());
    }

    IEnumerator SimpleLoadExit()
    {
        animatorController.SetTrigger("Start");
        
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}