using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class ReadTXT : MonoBehaviour
{
    public List<LevelsMG2> listLevelsMG2;//lista con los scriptableobjects
    [SerializeField] private UnityEngine.Tilemaps.Tile tileWin;//tile usado para ganar
    public List<UnityEngine.Tilemaps.Tile> tiles;
    [SerializeField] private Tilemap tilemapObjects;//tilemap para objetos
    [SerializeField] private string keyName;
    //[SerializeField] private string keyName62;
    [SerializeField] private bool deleteKey;
    [SerializeField] private TextMeshProUGUI countTMP;
    [SerializeField] private MiniGameSixControllerTwoSO miniGameTwoScriptableObject;
    [SerializeField] private AudioClipSO MG2Background;
    [HideInInspector] public LevelsMG2 levelActual;
    [HideInInspector] public int[,] matrix = new int[6, 7];//se crea matriz de 6x7
    [HideInInspector] public static ReadTXT instance;//se crea instancia para el singleton
    [HideInInspector] public int cantUses = 0;//almacena cantidad de usos
    private string mins;
    private string secs;
    private float time = 0;//almacena los segundos transcurridos
    private LevelLoader loaderInstance;

    public void ReadFromTheFile()
    {//leemos la matriz del txt
        string[] lines = levelActual.GetTextAsset.text.Split('\n');//almacena las lineas
        for (int i = 0; i < matrix.GetLength(0); i++)
        {//separamos los numeros por espacios
            string[] vector = lines[i].Split(',');
            for (int j = 0; j < matrix.GetLength(1); j++)
            {//convertimos los valores del txt a int y los asignamos a la matriz
                int.TryParse(vector[j], out matrix[i, j]);
                //dependiendo el valor de la matriz le asignamos su tile respectivo
                if (matrix[i, j] == 2)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[0]);
                else if (matrix[i, j] == 3)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[1]);
                else if (matrix[i, j] == 4)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[2]);
                else if (matrix[i, j] == 5)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[3]);
                else if (matrix[i, j] == 6)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[4]);
                else if (matrix[i, j] == 7)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[5]);
                //Debug.Log("matriz[" + i + ", " + j + "] = " + matrix[i, j]);
            }
        }
        if (SceneManager.GetActiveScene().name == "MiniGame2Test")
        {
            Debug.Log("nueva matriz");
            string[] linesss = levelActual.GetNewTextAsset.text.Split('\n');//almacena las lineas
            for (int i = 0; i < matrix.GetLength(0); i++)
            {//separamos los numeros por espacios
                string[] vector = linesss[i].Split(',');
                for (int j = 0; j < matrix.GetLength(1); j++)
                {//convertimos los valores del txt a int y los asignamos a la matriz
                    int.TryParse(vector[j], out matrix[i, j]);
                    Debug.Log("matriz[" + i + ", " + j + "] = " + matrix[i, j]);
                }
            }
        }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;//preparamos la habilitacion del singleton
        
        loaderInstance = LevelLoader.instance;

        //if (SceneManager.GetActiveScene().name == "MiniGame2Test")
        //    keyName = keyName62;
        if(deleteKey){
            PlayerPrefs.DeleteKey(keyName);
        }

        LoadMap();
    }

    private void OnEnable() {
        if(loaderInstance == null) return;

        loaderInstance.onNextLevelLoad.AddListener(IncreaseLevel);
        loaderInstance.onExitToMenu.AddListener(DeleteSaveKey);
    }

    private void OnDisable() {
        if(loaderInstance == null) return;

        loaderInstance.onNextLevelLoad.RemoveListener(IncreaseLevel);
        loaderInstance.onExitToMenu.RemoveListener(DeleteSaveKey);
    }

    private void Start()
    {
        bool isFIrst = PlayerPrefs.GetInt(keyName, 0) == 0;
        MenuGui.instance.SetUpButtons(isFIrst);
        MG2Background?.PlayLoop();
    }
    private void ChangeCountColor(Color _color)
    {
        countTMP.color = _color;
    }
    private void Contador()
    {
        time++;
        mins = Mathf.Floor(time / 60).ToString("00");
        secs = Mathf.Floor(time % 60).ToString("00");
        countTMP.text = mins + ":" + secs;
        if (time <= 45)
            ChangeCountColor(Color.green);
        else if (time > 45 && time <= 120)
            ChangeCountColor(Color.yellow);
        else if (time > 120 && time <= 180)
            ChangeCountColor(new Color(255 / 255f, 128 / 255f, 0 / 255f));
        else
            ChangeCountColor(Color.red);
        Debug.Log("segundos transcurridos: " + time);
    }
    public int CalculateScore()
    {
        CancelInvoke();
        if (time <= 45)
        {
            if (cantUses <= levelActual.GetMinMovsThreeStars)
                return 3;
            if (cantUses <= levelActual.GetMinMovsTwoStars)
                return 2;
            if (cantUses <= levelActual.GetMinMovsOneStar)
                return 1;
            else return 0;
        }
        else if (time <= 120)
        {
            if (cantUses <= levelActual.GetMinMovsTwoStars)
                return 2;
            if (cantUses <= levelActual.GetMinMovsOneStar)
                return 1;
            else return 0;
        }
        else if (time <= 180)
        {
            if (cantUses <= levelActual.GetMinMovsOneStar)
                return 1;
            else return 0;
        }
        else return 0;
    }
    private void LoadMap(){
        //PlayerPrefs.SetInt(keyName, 0);
        levelActual = listLevelsMG2[PlayerPrefs.GetInt(keyName,0)];

        List<int> temp = levelActual.GetCodeValues;
        for (int i = 0; i < levelActual.GetNamesValues.Count; i++)
        {//asignamos el code values en base al nombre del valor del objeto editable escogido
            if (levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Ganar)
                temp[i] = 2;
            else if (levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Perder)
                temp[i] = 3;
            else if (levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Detiene)
                temp[i] = 4;
            else if(levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Empujable)
                temp[i] = 5;
            else if(levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Flotar)
                temp[i] = 6;
            else if (levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Destruir)
                temp[i] = 7;
        }
        levelActual.SetCodeValues(temp);
        ReadFromTheFile();
        cantUses = 0;
        countTMP.text = "00:00";
        //segunda opcion del contador
        InvokeRepeating("Contador", 0f, 1f);
    }

    private void IncreaseLevel()
    {
        /*int newLevelValue = Mathf.Clamp(PlayerPrefs.GetInt(keyName) + 1,0,listLevelsMG2.Count-1);
        PlayerPrefs.SetInt(keyName,newLevelValue);*/

        if(miniGameTwoScriptableObject != null){
            int newLevelValue = Mathf.Clamp(PlayerPrefs.GetInt(keyName) + 1, 0, listLevelsMG2.Count);
            PlayerPrefs.SetInt(keyName, newLevelValue);
            GetCurrentNameScene();
        }
        else
        {
            int newLevelValue = Mathf.Clamp(PlayerPrefs.GetInt(keyName) + 1, 0, listLevelsMG2.Count - 1);
            PlayerPrefs.SetInt(keyName, newLevelValue);
        }
    }

    private void DeleteSaveKey(){
        PlayerPrefs.DeleteKey(keyName);
    }
    private void GetCurrentNameScene()
    {
        string sceneName = "";
        Debug.Log("CHANGING SCENE");
        if (PlayerPrefs.GetInt(keyName, 0) == miniGameTwoScriptableObject.Count)
        {
            Debug.Log("cambio de mg");
            sceneName = miniGameTwoScriptableObject.NextMiniGameScene;
        }
        else
        {
            Debug.Log("cambio de level");
            Debug.Log("PP: " + PlayerPrefs.GetInt(keyName, 0) + ", C: " + miniGameTwoScriptableObject.Count);
            sceneName = miniGameTwoScriptableObject.CurrentMiniGameScene;
        }
        loaderInstance.CurrentScene = sceneName;
    }
}