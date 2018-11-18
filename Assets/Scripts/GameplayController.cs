using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static float reactTime = 0.8f; //Time for reation for each level
    private const int blocksIncSpeed = 5; //Blocks before
    private const int resizeFactor = 7; //  Arrange factor

    public GameObject blockPrefab; //       Base prefab
    public GameObject blockPrefabParent; // The Gameobjects Parent
    public Sprite[] blockBackgrounds; //    All backgrounds
    public Image targetImage; //            The center image frame
    public Text highScore; //               Highscore text
    public Text score; //                   Score text
    public GameObject endPopup; //          Endgame popup
    public GameObject mainMenu; //          Menu
    public GameObject quitPopup; //         Quit app popup

    private readonly System.Random randomGenerator; // Random generator
    // Device screen size
    private readonly int screenHeight; 
    private readonly int screenWidth;

    // Vectors used for setting the block's place
    private Vector2 offset;
    private Vector2 StartPoint;
    private Vector2 EndPoint;

    private float reactTimer; //                 Timer
    private int levelCount; //                   Current level count
    private int sessionsCount; //                Played games (sessions) count
    private List<Level> levels; //               Levels
    private List<GameObject> blocksInstances; // Blocks
    public bool quitState; //                    Quit state
    private bool inGame; //                      Ingame State

    public bool InGame
    {
        get {
            return inGame;
        }
        set
        {
            inGame = value;
        }
    }

    public int LevelCount
    {
        get
        {
            return levelCount;
        }

        set
        {
            levelCount = value;
        }
    }

    public bool QuitState
    {
        get
        {
            return quitState;
        }
        set
        {
            quitState = value;
        }
    }

    public List<Level> Levels
    {
        get
        {
            return levels;
        }
        set
        {
            levels = value;
        }
    }

    public float ReactTimer
    {
        get
        {
            return reactTimer;
        }
        set
        {
            reactTimer = value;
        }
    }

    public List<GameObject> BlocksInstances
    {
        get
        {
            return this.blocksInstances;
        }
        set
        {
            blocksInstances = value;
        }
    }

    public int SessionsCount
    {
        get
        {
            return sessionsCount;
        }
        set
        {
            sessionsCount = value;
        }
    }

    public GameplayController()
    {
        // Init the values
        screenHeight = UnityEngine.Screen.height;
        screenWidth = UnityEngine.Screen.width;
        Levels = new List<Level>();
        BlocksInstances = new List<GameObject>();
        randomGenerator = new System.Random();
        StartPoint = new Vector2();
        EndPoint = new Vector2();
        ReactTimer = int.MaxValue;
        LevelCount = 1;
        InGame = true;
    } 

    void Start ()
    {
        // Init the settings
        QuitState = true;
        GenerateLevel(LevelCount);
        LoadLevel(LevelCount);
        SessionsCount = PlayerPrefs.GetInt(PrefsConst.SessionsCount, 0);
        GoogleMobileAdsImplementation.instance.RequestInterstitial();
    }

    void FixedUpdate ()
    {
        // Decrease the timer and end the game if it's 0
        ReactTimer -= Time.deltaTime;
        if (ReactTimer <= 0 && InGame)
        {
            GameOver();
        }
	}

    private void GenerateLevel(int crntLevel)
    {
        // Shuffle the backgrounds
        Shuffle(blockBackgrounds);
        int procentage;
        // Set canvas and it offset
        RectTransform canvas = blockPrefabParent.GetComponent<RectTransform>();
        offset = new Vector2(canvas.position.x, canvas.position.y);

        // Set the corners points before blocks beeing init
        EndPoint.Set(screenHeight, screenWidth);
        StartPoint.Set(0, 0);

            Level level = new Level();
            // Generate the blocks
            for (int j = 0, n = (crntLevel + blocksIncSpeed - 2 )/ blocksIncSpeed; j < n; j++) 
            {
                // Generate the block's size
                procentage = randomGenerator.Next( (95 /((LevelCount/ blocksIncSpeed)+2)) + j * resizeFactor, (101/((LevelCount/ blocksIncSpeed)+2)) + j* resizeFactor);
                Block block = new Block();
                // Randomly setting the block's possition and background
                if (procentage % 2 == 0)
                {
                    block.Width = EndPoint.y-StartPoint.y;
                    block.Height = CalculateProcentage(procentage, EndPoint.x-StartPoint.x);

                    if (block.Width % 2 == 0) {
                        block.Possition = AlignTop(block.Width, block.Height, EndPoint, StartPoint, offset);
                         EndPoint.Set(EndPoint.x - block.Height, EndPoint.y);
                    }
                    else
                    {
                        block.Possition = AlignBottom(block.Width, block.Height, EndPoint, StartPoint, offset);
                        StartPoint.Set(StartPoint.x + block.Height, StartPoint.y);
                    }
                }
                else
                {
                    block.Height = EndPoint.x - StartPoint.x;
                    block.Width = CalculateProcentage(procentage, EndPoint.y-StartPoint.y);

                    if (block.Width % 2 == 0)
                    {
                        block.Possition = AlignRight(block.Width, block.Height, EndPoint, StartPoint, offset);
                        EndPoint.Set(EndPoint.x, EndPoint.y - block.Width);
                    }
                    else
                    {
                        block.Possition = AlignLeft(block.Width, block.Height, EndPoint, StartPoint, offset);
                        StartPoint.Set(StartPoint.x, StartPoint.y + block.Width);
                    }
                }
                block.Background = blockBackgrounds[j];
                block.Value = j;
                level.Blocks.Add(block); 
        }
        // Create the last block based on what size is left
        Block fillBlock = new Block();
        fillBlock.Width = EndPoint.y - StartPoint.y;
        fillBlock.Height = EndPoint.x - StartPoint.x;
        fillBlock.Possition = new Vector3(((EndPoint.y + StartPoint.y - screenWidth) / 2) + offset.x,
            ((EndPoint.x + StartPoint.x - screenHeight) / 2) + offset.y);

        int value = (crntLevel + blocksIncSpeed - 2) / blocksIncSpeed;
        fillBlock.Background = blockBackgrounds[value];
        fillBlock.Value = value;
       
        level.Blocks.Add(fillBlock);

        int randValue = randomGenerator.Next(0, value+1);
        level.LevelValue = randValue;
        targetImage.sprite = blockBackgrounds[randValue];
        Levels.Add(level);
    }

    private void LoadLevel(int level)
    {
        // Hide the manu if it's open
        if (level > 1)
        {
            mainMenu.SetActive(false);
        }
        
        for (int i = 0, n = ((level - 2 + blocksIncSpeed)/ blocksIncSpeed) + 1; i < n; i++)
        {   
            // Load te pre-generated levels
            GameObject blockInstance = Instantiate(blockPrefab, levels[level - 1].Blocks[i].Possition, Quaternion.identity);
            blockInstance.name = "BlockNumber" + (i+1).ToString();
            blockInstance.transform.parent = blockPrefabParent.transform;
            blockInstance.GetComponent<Image>().sprite = levels[level-1].Blocks[i].Background;
            RectTransform blockTranform = blockInstance.GetComponent<RectTransform>();
            blockTranform.SetSiblingIndex(2);
            blockInstance.GetComponent<BlockPrefab>().Value = levels[level - 1].Blocks[i].Value; 
            blockTranform.sizeDelta = new Vector2(levels[level-1].Blocks[i].Width, levels[level - 1].Blocks[i].Height);
            BlocksInstances.Add(blockInstance);
        }
    }

    private float CalculateProcentage(int procentage, float size)
    {
        // Calculate the procentage from size
        float result = (size * procentage) / 100 ;
        return Mathf.Floor(result);
    }
   
    private Vector3 AlignTop(float blockWidth, float blockHeight, Vector2 endPoint, Vector2 startPoint, Vector2 offSet)
    {
        // Align the block top
        float x =  (startPoint.y - (screenWidth - endPoint.y))/2;
        float y = ((screenHeight - blockHeight) / 2) - (screenHeight- endPoint.x);
        
        return new Vector3(x + offSet.x,y + offSet.y);
    }

    private Vector3 AlignBottom(float blockWidth, float blockHeight, Vector2 endPoint, Vector2 startPoint, Vector2 offSet)
    {
        // Align the block bottom
        float x = (startPoint.y - (screenWidth - endPoint.y)) / 2;
        float y = (((screenHeight - blockHeight) / 2) * -1) + startPoint.x;  

        return new Vector3(x + offSet.x, y + offSet.y);
    }

    private Vector3 AlignLeft(float blockWidth, float blockHeight, Vector2 endPoint, Vector2 startPoint, Vector2 offSet)
    {
        // Align the block left
        float x = ((( blockWidth - screenWidth ) / 2)) + (startPoint.y); 
        float y = (startPoint.x - (screenHeight - endPoint.x)) / 2; 
        
        return new Vector3(x + offSet.x, y + offSet.y);
    }
    private Vector3 AlignRight(float blockWidth, float blockHeight, Vector2 endPoint, Vector2 startPoint, Vector2 offSet)
    {
        // Align the block right
        float x = (((screenWidth - blockWidth) / 2)) - (screenWidth - endPoint.y);  
        float y = (startPoint.x - (screenHeight - endPoint.x)) / 2;

        return new Vector3(x + offSet.x, y + offSet.y);
    }

    public void onBlockClicked(BlockPrefab blockPrefab)
    {
        // Trigger when Block is clicked
        if (InGame)
        {
            if (blockPrefab.Value == Levels[levelCount - 1].LevelValue)
            {
                MusicController.Instance.rightSound.Play();
                LevelCount++;
                ReactTimer = reactTime;
                NextLevel(LevelCount);
            }
            else
            {
                MusicController.Instance.wrongSound.Play();
                GameOver();
            }
        }
    }

    private void NextLevel(int currentLevel)
    {
        // Pass to the next level
        DestroyBlocks();
        GenerateLevel(currentLevel);
        LoadLevel(currentLevel);
    }

    private void DestroyBlocks()
    {
        // Destroy the blocks
        for (int i = 0, n = BlocksInstances.Count; i < n; i++)
        {
            Destroy(BlocksInstances[i]);
        }
    }

    private void GameOver()
    {
        // Increase and merge the sessions count
        SessionsCount++;
        PlayerPrefs.SetInt(PrefsConst.SessionsCount, SessionsCount);

        // Show the fullscrean AD on every 25th session
        if (SessionsCount % 25 == 0)
        {   
            GoogleMobileAdsImplementation.instance.ShowInterstitial();
        }
        else if (SessionsCount % 24 == 0)
        {
            GoogleMobileAdsImplementation.instance.RequestInterstitial();
        }

        InGame = false;
        // Set the text and show in the endgame popup
        score.text = "Score " + (LevelCount-1).ToString();
        mainMenu.SetActive(true);
        if (PlayerPrefs.GetInt(PrefsConst.Score, 0) < LevelCount - 1)
        {
            // Set the new highscore
            PlayerPrefs.SetInt(PrefsConst.Score, LevelCount-1);
        }
        highScore.text = "Record " + PlayerPrefs.GetInt(PrefsConst.Score).ToString();
        endPopup.SetActive(true);
        QuitState = false;
        GoogleMobileAdsImplementation.instance.RequestBanner();
    }

    private void Shuffle<T>(T[] array)
    {
        // Shuffle an array
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + randomGenerator.Next(n - i);
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    public void RestartGame()
    {
        // Reset the game
        GoogleMobileAdsImplementation.instance.HideBanner();
        DestroyBlocks();
        ClosePopups();
        Levels.Clear();
        LevelCount = 1;
        ReactTimer = int.MaxValue;
        GenerateLevel(LevelCount);
        LoadLevel(LevelCount);
        QuitState = true;
        InGame = true;
    }

    private void ClosePopups()
    {
        // Close all Popups
        endPopup.SetActive(false);
        quitPopup.SetActive(false);
    }

    public void CloseQuitPopup()
    {
        // Close the quit Popup
        ClosePopups();
        endPopup.SetActive(!InGame);
        QuitState = true;
    }

    public void OpenQuitPopup()
    {
        // Open the quit Popup and show the banner AD
        ClosePopups();
        quitPopup.SetActive(true);
        QuitState = false;
        InGame = false;
        GoogleMobileAdsImplementation.instance.RequestBanner();
    }

    public void ManageBack()
    {
        // Call when back button is pressed
        // Close the popups | open the quit popup | quit the game
        if (QuitState && (!InGame || LevelCount == 1))
        {
            OpenQuitPopup();
        }
        else if (LevelCount == 1)
        {
            ClosePopups();
            QuitState = true;
            InGame = true;
        }
        else if(!InGame)
        {
            RestartGame();
        }
    }
}
