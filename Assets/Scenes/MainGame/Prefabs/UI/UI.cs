using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private GameObject start;
    private Animator startAni;
    private GameObject friends;
    private Animator friendsAni;
    private GameObject settings;
    private Animator settingsAni;
    private GameObject resetButton;
    private Animator resetAni;

    private GameObject joystick;
    private Image handle;
    public Transform handleTransform;

    private GameObject cam;
    private Animator camAnimation;

    private Text score;
    private Text scoreRestart;
    public int restartCount, resetCount;
    private int scoreCount;
    private Text highScore;
    private Text timer;

    private float currentTime;
    private float startingTime;

    private bool cityTimer;
    private bool startTimer = true;

    private Spawner2 spawner2Script;
    private GameObject spawnerElement;
    private GameObject toys;

    private bool startTutorial;
    private bool tutorialScreen;

    private GameObject mainMenu;
    private GameObject tutorial;

    private Text bedText;
    private Text rampText;
    private Text cityText;

    private GameObject cam2, cam3;
    private Animator camAni2, camAni3;

    private bool mainMenuIdle, camSwitch;

    private void Start()
    {
        //buttons animator
        start = GameObject.Find("Start");
        startAni = start.GetComponent<Animator>();

        friends = GameObject.Find("Friends");
        friendsAni = friends.GetComponent<Animator>();

        settings = GameObject.Find("Settings");
        settingsAni = settings.GetComponent<Animator>();

        resetButton = GameObject.Find("RestartButton");
        resetButton.GetComponent<Image>().enabled = false;
        resetAni = resetButton.GetComponent<Animator>();

        //Joystick
        joystick = GameObject.Find("Fixed Joystick"); //pakt joystick
        SetJoyStickUnactive();  //zet gameobject joystick op niet actief

        //camera
        cam = GameObject.Find("Camera"); //pakt camera
        camAnimation = cam.GetComponent<Animator>(); //pakt van de cam de animator

        //zoekt naar text
        score = GameObject.Find("Score").GetComponent<Text>();
        scoreRestart = GameObject.Find("ScoreRestart").GetComponent<Text>();
        scoreRestart.gameObject.GetComponent<Text>().enabled = false;
        
        //highscore text
        highScore = GameObject.Find("HighScoreCount").GetComponent<Text>();
        //timerText
        timer = GameObject.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);

        //Canvas
        mainMenu = GameObject.Find("MainMenuCanvas");
        tutorial = GameObject.Find("Tutorial");

        //camera's
        cam2 = GameObject.Find("cam2");
        cam3 = GameObject.Find("cam3");

        camAni2 = cam2.GetComponent<Animator>();
        camAni3 = cam3.GetComponent<Animator>();

        //zet score naar 0
        scoreCount = 0;

        highScore.text = PlayerPrefs.GetInt("HighscoreCount").ToString();

        startingTime = 18f;

        tutorial.gameObject.SetActive(false);

        resetCount = PlayerPrefs.GetInt("RestartGame", resetCount);

        mainMenuIdle = true;
        camSwitch = true;
    }

    private void Update()
    {
        if (resetCount < 1)
        {
            startTutorial = true;
        }
        else if (resetCount >= 1)
        {
            startTutorial = false;
        }

        PlayerPrefs.SetInt("RestartGame",restartCount);

        //dit word de scoretext text
        score.text = "Score " + scoreCount;
        scoreRestart.text = "Score " + scoreCount;

        //highscore
        if (scoreCount > PlayerPrefs.GetInt("HighscoreCount", 0))
        {
            PlayerPrefs.SetInt("HighscoreCount", scoreCount);
            highScore.text = scoreCount.ToString();
        }

        //timer
        currentTime = startingTime;

        if (timer.gameObject.activeSelf)
        {
            currentTime = startingTime -= 1 * Time.deltaTime; //zet de timer aan dat er 1 er afgehaalt wordt
            timer.text = currentTime.ToString("Time left: 0"); // zet de timer text neer

            if (startingTime <= 0) //dit regeld dat de timer niet verder zou tellen dan 0 dus hij kan niet de min in gaan
            {
                startingTime = 0;
            }
        }

        if (startTimer && startingTime == 0)
        {
            GameObject[] spawnElements = GameObject.FindGameObjectsWithTag("SpawnElement");
            foreach (GameObject spawnElement in spawnElements)
                GameObject.Destroy(spawnElement);

            GameObject[] toys = GameObject.FindGameObjectsWithTag("Toy");
            foreach (GameObject toy in toys)
                GameObject.Destroy(toy);

            startTimer = false;
        }

        if (cityTimer && startingTime == 0)
        {
            ShowRestartMenu();
        }

        //tutorial screen
        if (tutorialScreen == false)
        {
            mainMenu.gameObject.SetActive(true);
            tutorial.gameObject.SetActive(false);
        }
        else if (tutorialScreen)
        {
            mainMenu.gameObject.SetActive(false);
            tutorial.gameObject.SetActive(true);
        }

        //skip tutorial
        if (Input.GetMouseButtonDown(0) && tutorialScreen)
        {
            Invoke("EndTutorial", 1f);
        }

        //cycle between cams
        if (mainMenuIdle && camSwitch)
        {
            SetCam2Active();
        }
        else if (mainMenuIdle == false)
        {
            Destroy(cam2);
            Destroy(cam3);
        }
    }

    public void StartAnimation()
    {
        if (startTutorial)
        {
            tutorialScreen = true;
            print("tutorial");
            Invoke("BeginTutorial", 1.2f);
            cam.SetActive(true);
            cam2.SetActive(false);
            cam3.SetActive(false);
            //Speelt buttonanimations af
            startAni.SetBool("startAni", true);
            friendsAni.SetBool("startAni", true);
            settingsAni.SetBool("startAni", true);
        }
        if (startTutorial == false)
        {
            mainMenuIdle = false;
            StartGameNormal();
        }
    }

    private void StartGameNormal()
    {
        cam.SetActive(true);
        cam2.SetActive(false);
        cam3.SetActive(false);
        camAnimation.SetBool("animationStart", true); //zet de bool op true
                                                      //Kleuren van Image
        start.GetComponent<Image>().color = new Color32(90, 90, 90, 225); //zet kleur van de image anders voor feedback naar speler toe
        friends.GetComponent<Image>().color = new Color32(90, 90, 90, 225); //zet kleur van de image anders voor feedback naar speler toe
        settings.GetComponent<Image>().color = new Color32(90, 90, 90, 225); //zet kleur van de image anders voor feedback naar speler toe

        //Speelt buttonanimations af
        startAni.SetBool("startAni", true);
        friendsAni.SetBool("startAni", true);
        settingsAni.SetBool("startAni", true);

        //joystick
        Invoke("SetJoystickActive", 1f); //gaat naar metode
    }



    public void RestartButton()
    {
        restartCount++;
        resetButton.GetComponent<Image>().color = new Color32(90, 90, 90, 225);
        SceneManager.LoadScene("Test");
    }




    private void BeginTutorial()
    {
        mainMenuIdle = false;
        camAnimation.SetBool("animationTutorial", true);
        Invoke("EndTutorial", 22f);
        Invoke("CityTextEnabled", 11.2f);
        Invoke("RampTextEnabled", 5.2f);
    }

    private void RampTextEnabled()
    {
        bedText = GameObject.Find("BedText").GetComponent<Text>();
        bedText.GetComponent<Text>().enabled = false;

        rampText = GameObject.Find("RampText").GetComponent<Text>();
        rampText.GetComponent<Text>().enabled = true;
    }

    private void CityTextEnabled()
    {
        rampText = GameObject.Find("RampText").GetComponent<Text>();
        rampText.GetComponent<Text>().enabled = false;

        cityText = GameObject.Find("CityText").GetComponent<Text>();
        cityText.GetComponent<Text>().enabled = true;
    }

    private void EndTutorial()
    {
        tutorialScreen = false;
        start.gameObject.SetActive(false);
        friends.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        camAnimation.SetBool("animationTutorial", false);
        Invoke("SetJoystickActive", 1f); //gaat naar metode
        camAnimation.SetBool("animationStart", true); //zet de bool op true
    }




    private void SetJoystickActive() //zet het gameobject joystick actief
    {
        joystick.gameObject.SetActive(true);
        timer.gameObject.SetActive(true); //zet het gameobject timer aan zie void update voor werking
    }

    //Score add
    public void ScoreAdd()
    {
        scoreCount++;
    }

    public void ScoreAdd5()
    {
        scoreCount += 5;
    }

    public void ScoreAdd7()
    {
        scoreCount += 7;
    }


    //Joystick uit
    public void SetJoyStickUnactive()
    {
        handleTransform.localPosition = new Vector3(0, 0, 0) * Time.fixedTime;
        joystick.gameObject.SetActive(false);
    }
    
    //Timer
    public void SetTimerUnactive()
    {
        timer.gameObject.SetActive(false);
    }
    //city timer
    public void CityTimerSetActive()
    {
        cityTimer = true;
        startingTime = 5f;
        timer.gameObject.SetActive(true);
    }


    //Restart menu
    void ShowRestartMenu()
    {
        print("AnimationRestartButton");
        resetButton.GetComponent<Image>().enabled = true;
        scoreRestart.GetComponent<Text>().enabled = true;

        score.GetComponent<Text>().enabled = false;
        highScore.GetComponent<Text>().enabled = false;
        timer.GetComponent<Text>().enabled = false;
    }

    private void SetCam2Active()
    {
        if (mainMenuIdle)
        {
            cam.SetActive(false);
            cam2.SetActive(true);
            cam3.SetActive(false);
            Invoke("SetCam3Active", 10f);
            camSwitch = false;
        }
    }

    private void SetCam3Active()
    {
        if (mainMenuIdle)
        {
            cam2.SetActive(false);
            cam3.SetActive(true);
            cam.SetActive(false);
            Invoke("SetCam1Active", 10f);
        }
    }

    private void SetCam1Active()
    {
        if (mainMenuIdle)
        {
            cam.SetActive(true);
            cam2.SetActive(false);
            cam3.SetActive(false);
            Invoke("SetCam2Active", 5f);
        }
    }
}
