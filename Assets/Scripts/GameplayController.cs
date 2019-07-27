using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    private const float reactTime = 0.8f; //     Time for reation for each level
    private const int blocksIncSpeed = 5; //     Blocks before
    private const int resizeFactor = 7; //       Arrange factor
    private const int adSessionsToShow = 25;//   Amount of sessions played after which the full screen ad is shown

    private readonly System.Random randomGenerator; // Random generator
    // Device screen size
    private readonly int screenHeight;
    private readonly int screenWidth;

    [Header("Referances")]
    [SerializeField]
    private GameObject blockPrefab; //            Base prefab

    [SerializeField]
    private GameObject blockPrefabParent; //      The Gameobjects Parent

    [SerializeField]
    private Sprite[] blockBackgrounds; //         All backgrounds

    [SerializeField]
    private Image targetImage; //                 The center image frame

    [SerializeField]
    private Text highScore; //                    Highscore text

    [SerializeField]
    private Text score; //                        Score text

    [SerializeField]
    private GameObject endPopup; //               Endgame popup

    [SerializeField]
    private GameObject mainMenu; //               Menu

    [SerializeField]
    private GameObject quitPopup; //              Quit app popup

    // Vectors used for setting the block's place
    private Vector2 offset;
    private Vector2 StartPoint;
    private Vector2 EndPoint;

    private bool inGame; //                      Ingame State
    private int levelCount; //                   Current level count
    private bool quitState; //                   Quit state
    private List<Level> levels; //               Levels
    private float reactTimer; //                 Timer
    private List<GameObject> blocksInstances; // Blocks
    private int sessionsCount; //                Played games (sessions) count

    public GameplayController()
    {
        // Init the values
        screenHeight = UnityEngine.Screen.height;
        screenWidth = UnityEngine.Screen.width;
        levels = new List<Level>();
        blocksInstances = new List<GameObject>();
        randomGenerator = new System.Random();
        StartPoint = new Vector2();
        EndPoint = new Vector2();
        reactTimer = int.MaxValue;
        levelCount = 1;
        inGame = true;
    } 

    void Start ()
    {
        // Init the settings
        quitState = true;
        GenerateLevel(levelCount);
        LoadLevel(levelCount);
        sessionsCount = PlayerPrefs.GetInt(PrefsConst.SessionsCount, 0);
        GoogleMobileAdsImplementation.instance.RequestInterstitial();
    }

    void FixedUpdate ()
    {
        // Decrease the timer and end the game if it's 0
        reactTimer -= Time.deltaTime;
        if (reactTimer <= 0 && inGame)
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
                procentage = randomGenerator.Next( (95 /((levelCount/ blocksIncSpeed)+2)) + j * resizeFactor, (101/((levelCount/ blocksIncSpeed)+2)) + j* resizeFactor);
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
        levels.Add(level);
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
            blocksInstances.Add(blockInstance);
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
        if (inGame)
        {
            if (blockPrefab.Value == levels[levelCount - 1].LevelValue)
            {
                MusicController.Instance.rightSound.Play();
                levelCount++;
                reactTimer = reactTime;
                NextLevel(levelCount);
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
        for (int i = 0, n = blocksInstances.Count; i < n; i++)
        {
            Destroy(blocksInstances[i]);
        }
    }

    private void GameOver()
    {
        // Increase and merge the sessions count
        sessionsCount++;
        PlayerPrefs.SetInt(PrefsConst.SessionsCount, sessionsCount);

        // Show the fullscrean AD on every 25th session
        if (sessionsCount % adSessionsToShow == 0)
        {   
            GoogleMobileAdsImplementation.instance.ShowInterstitial();
        }
        // Prepare the add to be shown
        else if (sessionsCount % adSessionsToShow - 1 == 0)
        {
            GoogleMobileAdsImplementation.instance.RequestInterstitial();
        }

        inGame = false;
        // Set the text and show in the endgame popup
        score.text = "Score " + (levelCount-1).ToString();
        mainMenu.SetActive(true);
        if (PlayerPrefs.GetInt(PrefsConst.Score, 0) < levelCount - 1)
        {
            // Set the new highscore
            PlayerPrefs.SetInt(PrefsConst.Score, levelCount-1);
        }
        highScore.text = "Record " + PlayerPrefs.GetInt(PrefsConst.Score).ToString();
        endPopup.SetActive(true);
        quitState = false;
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
        levels.Clear();
        levelCount = 1;
        reactTimer = int.MaxValue;
        GenerateLevel(levelCount);
        LoadLevel(levelCount);
        quitState = true;
        inGame = true;
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
        endPopup.SetActive(!inGame);
        quitState = true;
    }

    public void OpenQuitPopup()
    {
        // Open the quit Popup and show the banner AD
        ClosePopups();
        quitPopup.SetActive(true);
        quitState = false;
        inGame = false;
        GoogleMobileAdsImplementation.instance.RequestBanner();
    }

    public void ManageBack()
    {
        // Call when back button is pressed
        // Close the popups | open the quit popup | quit the game
        if (quitState && (!inGame || levelCount == 1))
        {
            OpenQuitPopup();
        }
        else if (levelCount == 1)
        {
            ClosePopups();
            quitState = true;
            inGame = true;
        }
        else if(!inGame)
        {
            RestartGame();
        }
    }
}
