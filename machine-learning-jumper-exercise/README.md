# Jumper exercise met ML Agent

## Intro

Het doel van dit Unity project is om een speler (Agent) te leren om over obstakels te springen die van twee richten op hem afkomen door gebruik te maken van een belongingsysteem. In deze README wordt uitgelegd hoe de verschillende elementen, objecten en scripts werken in dit project. 

Om dit project succesvol te kunnen afronden dient er aantal dingen geïnstalleerd te zijn alvorens je kan beginnen. Hier wordt in deze handleiding niet in detail op ingegaan en wordt verwacht op voorhand in orde te zijn. Dit omvat Unity, Python, ML-Agents en TensorFlow.

### *Groep*

Groepsnaam: JB
Studenten: 
 - Brent Janssens       |S106999
 - Jana Van Goethem     |S106673

## Het spel

![Het spel](Img/spel.png)

De speler staat op het speeldveld en ziet van twee richtingen obstakels op zich afkomen. Het doel is om de speler aan te leren dat indien hij over een obstakel heen springt hij beloond wordt. Indien hij in aanraking komt met het obstakel verliest hij punten. 

## Reward System

Om de agent aan te leren om over de obstakels heen te springen gaan we hem belonen indien hij dit correct doet. Deze manier van werken wordt 'reinforcement learning' genoemd. Het doel voor de agent is om zoveel mogelijk punten te verzalen. Het doel is om hem te leren dat dit kan bereikt worden door over de obstakels heen te springen. 
We gaan de agent op twee manieren stimuleren. Eenderzijds door hem punten te geven indien hij succesvol over een obstakel heen spring, maar anderzijds door hem punten af te nemen indien hij onnodig springt of tegen het obstakel aankomt. 
We gaan de agent dus straffen of belonen bij een actie die hij uitvoerd. In onderstaande tabel staan de verschillende beloningen en straffen opgelijst. 


| Omschrijving                                      | Beloning (floats) |
| --------------------------------------------------| ----------------- |
| Aanraken van een obstakel                         | -1                |
| Aanraken van een obstakel uit de andere richting  | -1                |
| Springen                                          | -0.001            |
| Stilstaan voor één frame                          | +0.001            |
| Obstakel gaat over *DeadLine* of *DeadLineCross*  | +1                |


## Objecten

Om de agent te kunnen trainen moet we eerst een omgeving hiervoor opzetten. Het object, de omgeving, hierna vernoemd *Environment* wordt opgebouwd met onderstaande elementen.

### *Environment*

![Environment](Img/environment.png)

De *environment* is het hoofdobject van het project. Hierin zijn alle andere objecten aanwezig die de omgeving zelf zal aanmaken (spawnen) bij de start van het spel. In dit project specifiek worden de speler (Player) en het obstakel (Obstacle) gespawned bij de opstart van het spel door de *environment*. Men kan meedere *environments* tegelijk in een scène zetten om zo meerdere spelomgevingen tegelijk te laten lopen. Dit zal het leerproces van de agent aanzienlijk versnellen maar is intensiever voor het toestel waarop de omgevingen gedraaid worden. Hardware limitaties zijn hier dus wel van toepassing.

### *Player*

![Player](Img/player.png)

Het *Player* object wordt op een simpele manier voorgesteld door een bean met een sphere object als rechteroog en linkeroog. De *player* staat stil op het speelveld en heeft enkel de mogelijkheid om de "jump" actie uit te voeren.
Wanneer de "jump"-actie wordt uitgevoerd kan de *player* zelf bepalen. 
Met andere woorden wilt dit zeggen dat doorheen het leerproces van de *player* het moment waarop de "jump"-actie wordt uitegevoerd zal varieren naarmate de *agent* slimmer wordt en het spel beter begrijpt.
Om ervoor te zorgen dat de *player* geen andere bewegingen kan uitvoeren worden de waarden van X- en Y- Postition waarden op 0 en 1 respectievelijk vastgezet. Ook de X-, Y- en Z- rotation waarden worden hiervoor op 0 gehouden. Hierdoor kan de *player* niet roteren en blijft het gezichtsveld enkel wat binnen het Ray Percention veld valt gehouden.

Het Ray Perception veld wordt aangemaakt op beide ogen. Voor zowel het linker als het rechteroog wordt dit component toegevoegd. 

De Ray Perception sensoren moeten met bepaalde waarden worden ingesteld. Hieronder vindt u de tabel van hoe deze voor dit project dienen ingesteld te worden. De ogen hebben een breedt gezichtsveld om zo objecten van beide kanten te kunnen zien aankomen.
De "Detectable Tags" is zeer belangrijk aangezien we hier moeten aangeven welke objecten de ogen moeten kunnen detecteren. In het geval van dit spel is het enkel een obstakel dat moet gedetecteerd dient te worden. 
| Variabele             | Waarde         |
| --------------------- | -------------- |
| Detectable Tags       | Obstacle       |
| Rays Per Direction    | 1              |
| Max Ray Degrees       | 90             |
| Sphere Cast Radius    | 3.5            |
| Ray Length            | 75             |
| Ray Layer Mask        | Mixed          |
| Stacked Raycasts      | 1              |
| Start Vertical Offset | 0              |
| End Vertical Offset   | -10            |

Verder wordt er voor de *player* een collider-component toegevoegd met interacties tegenover *Floor* en *Obstacle*. Dit is nodig om een beloning of straf toe te kennen indien er een aanraking is tussen deze elementen. 
Door het "Floor*-object aan te raken (wat wordt waargenomen met de collider) weet de*player* dat hij opnieuw kan springen. De agent verliest punten indien hij in aanraking komt met een obstakel. 

Het *Player*-object wordt verder nog het *Player*-script toegekend waarover verder in dit bestand meer informatie gegeven zal worden. Het is wel belangrijk om de waarden bij dit script correct in te vullen.
Voor dit project dient "Max Step" ingesteld te worden op 200 en "Jump Force" op 5.40.
Ook het ingebouwde script van de ML-Agents genaamd "Desicion Requester" dient toegevoegd te worden aan de *Player*. Hier hoeven geen aanpassingen aan gedaan te worden.

### *Obstacles*

![Obstacles](Img/obstacles.png)

Het *Obstacles* & *ObstaclesCross*-objecten zijn een Parent object waaronder de *Obstacle* objecten vallen. Dit wordt niet verder gebruikt buiten door het environment script.

### *Obstacle*

![Obstacle](Img/obstacle.png)

Het *Obstacle* & *ObstacleCross*-object is een rechthoek die een voorwaartse beweging maakt over het *Floor*-object. Deze rechthoek heeft een collider-component om een interactie mogelijk te maken tussen dit object en de *Player*.

Als de *Player* het *Obstacle* aanraakt verliest hij punten en wordt het *Obstacle* vernietigt.

### *ScoreBoard*

![ScoreBoard](Img/scoreboard.png)

Het *ScoreBoard* is een tekstelement dat de score (beloning) van de agent zal toonen doorheen het trainingsproces. Hierdoor kan men makkelijk visueel zien wat de huidige score van die specifieke agent is.

### *Floor*

![Floor prefab](Img/floor.png)

Het *Floor* & *Floorcross*-object zijn een simpele planes uit Unity waar de andere objecten opstaan en overheen schuiven. Dit is simpelweg een aanduiding van het speelveld. Het *Floor* en *FloorCross* object staan op 90° van elkaar om zo een overlapping te maken met een kruispunt in het midden.

### *SpawnLine*

![SpawnLine prefab](Img/spawnline.png)

Het *SpawnLine* & *SpawnlineCross*-object is een lijn die het begint van de *Floor* aangeeft waarvan andere objecten kunnen spawnen. De *SpawnLine* wordt op het *Floor*-object geplaatst met de *SpawnLineCross* op het *FloorCross*-object. Dit zorgt ervoor dat beiden *Floor*-objecten een *SpawnLine* bevatten.

### *DeadLine*

![Deadline prefab](Img/deadline.png)

Het *DeadLine* & *DeadlineCross-object* is een lijn die het einde van de vloer aangeeft en waar andere objecten bij het bereiken van deze lijn doet destroyen. Om het mogelijk te maken andere objecten te destroyen bij aanraking van de *DeadLine* wordt hieraan een collider-component toegevoegd.
De *DeadLine* wordt op het *Floor*-object geplaatst met de *DeadLineCross* op het *FloorCross*-object. Dit zorgt ervoor dat beide *Floor*-objecten een *DeadLine* bevatten en er op beide objecten kunnen gedestroyed worden.

## Scripts

We bepalen het gedrag van de objecten door deze scripts toe te kennen. De inhoud van deze scripts wordt hieronder verklaard. 

### *Environment script*

Het environment script zorgt voor het intialiseren van de verschillende objecten in de scene. Ook het spawnen van de *Obstacles* zal in het environment script worden bepaald.

Declaratie van de variabelen.

```
public class Environment : MonoBehaviour
{
    public Obstacle obstacle;
    public Player player;
    public float maxTime = 2f;
    public float minTime = 1f;

    private SpawnLine spawnLine;
    private SpawnLineCross spawnLineCross;

    private Player p;
    private TextMeshPro scoreBoard;

    private GameObject obstacles;
    private GameObject obstaclesCross;

    private Vector3 spawnLineLocation;
    private Vector3 spawnLineLocationCross;

    private GameObject floor;
    private GameObject floorCross;

    private Quaternion spawnLineRotation;
    private Quaternion spawnLineRotationCross;

    //current time
    private float currentTimer;

    //The time to spawn the object
    private float randomTimed;
``` 
Starten van de ``` currentTimer ``` met maximale en minimale tijd startend van 0.

```
    private void Start()
    {
        currentTimer = 0f;
        SetRandomTimed();
    }

    private void SetRandomTimed()
    {
        randomTimed = Random.Range(minTime, maxTime);
    }
```
Hier worden alle private variabelen geinitialiseerd
```
    public void OnEnable()
    {
        p = transform.GetComponentInChildren<Player>();
        scoreBoard = transform.GetComponentInChildren<TextMeshPro>();

        //for default
        obstacles = transform.Find("Obstacles").gameObject;             
        spawnLine = transform.GetComponentInChildren<SpawnLine>();        
        floor = transform.Find("Floor").gameObject;      

        spawnLineLocation = spawnLine.transform.position;
        spawnLineRotation = spawnLine.transform.rotation;

        //for cross
        obstaclesCross = transform.Find("ObstaclesCross").gameObject;
        spawnLineCross = transform.GetComponentInChildren<SpawnLineCross>();
        floorCross = transform.Find("FloorCross").gameObject;

        spawnLineLocationCross = spawnLineCross.transform.position;
        spawnLineRotationCross = spawnLineCross.transform.rotation;
    }
```
Methode die wordt aangeroepen bij het eindigen van de episode en zorgt voor het vernietigen van het *obstacle*-object.

    public void ClearEnvironment()
    {
        foreach (Transform obstacle in obstacles.transform)
        {
            Destroy(obstacle.gameObject);
        }
    }
Deze methode ``` SpawnPlayer() ``` zorgt voor het genereren van de speler en wordt aangeroepen in het "Player"-script.
```

    public void SpawnPlayer()
    {
        Player newPlayer = Instantiate(player, new Vector3(0, 1.5f, 42.2f), new Quaternion(0f, 180f, 0f, 0f));
        newPlayer.transform.SetParent(gameObject.transform);
    }
```
Deze methode ``` SpawnObstacle() ``` & ``` SpawnObstacleCross() ``` zorgen voor het genereren van de *Obstacles* langs beiden richtingen.
```

    private void SpawnObstacle()
    {
        ResetTimer();
        //for default
        Obstacle newObstacle = Instantiate(obstacle, new Vector3(spawnLineLocation.x, floor.transform.position.y + 0.5f, spawnLineLocation.z), spawnLineRotation);
        newObstacle.transform.SetParent(obstacles.transform);
        
    }

    private void SpawnObstacleCross()
    {
        ResetTimer();
        //for cross
        Obstacle newObstacleCross = Instantiate(obstacle, new Vector3(spawnLineLocationCross.x, floorCross.transform.position.y + 0.5f, spawnLineLocationCross.z), spawnLineRotationCross);
        newObstacleCross.transform.SetParent(obstaclesCross.transform);
    }
```
In deze functies wordt op basis van een random tijdsbepaling bepaald wanneer de *Obstacles* mogen spawnen. Er wordt ook voor gezorgd er dat geen twee *Obstacles* op dezelfde moment gespawnt worden en elkaar dus niet kunnen raken op het kruispunt.
```
    private void ResetTimer()
    {
        currentTimer = 0;
    }
    private void FixedUpdate()
    {
        currentTimer += Time.deltaTime;
        int randomValue = Random.Range(1, 3);

        if (CanSpawn())
        {
            switch (randomValue)
            {
                case 1:
                    SpawnObstacle();
                    break;
                case 2:
                    SpawnObstacleCross();
                    break;
                default:
                    break;
            }

            RestartCurrentTimer();
        }

        scoreBoard.text = p.GetCumulativeReward().ToString("f3");
    }

    private bool CanSpawn()
    {
        return randomTimed <= currentTimer;
    }

    private void RestartCurrentTimer()
    {
        currentTimer = 0f;
    }
}
```

### *Player script*
Het "Player"-script bepaald de eigenschappen van de *Agent*. In dit script wordt de actie van "Jump" bepaald en de collision met een *Obstacle*. Het reward system wordt ook in dit script uitgewerkt en hier worden de waardes die verdient of verloren kunnen worden door de *Agent* bepaald. Verder vindt je in dit script een aantal basis ML-Agents funties zoals ``` OnEpisodeBegin() ```, ``` OnCollisionEnter() ``` en ``` OnActionReceived() ```.

```
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : Agent
    {
        public float jumpForce = 5.40f;

        private Rigidbody body;
        private Environment environment;
        private bool isJumpReady = true;

        public override void CollectObservations(VectorSensor sensor)
        {
        }
```
In dit gedeelte zorgen we ervoor dat we onze *Player* ook kunnen doen springen met het manueel indrukken van de spatiebalk voor testingsdoeleinden.

        public override void Heuristic(float[] actionsOut)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }

In de methode ``` Jump() ``` zorgen we dat de *Player* kan springen. Hij verliest -0.001f per keer de *Player* springt. 
```
        private void Jump()
        {
            if (isJumpReady)
            {
                body.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
                isJumpReady = false;
                AddReward(-0.001f);
            }
        }

```
In de methode ``` FixedUpdate() ``` wordt de basisfunctie ``` RequestDecision() ``` van de ML-Agents library aangeroepen. Dit zal dan op basis van Machine Learning bepalen of de *Player* zal springen of niet. Indien de *Player* ervoor kiest om niet te springen omdat hij dit onnodig vindt wordt een kleine rewards toegekend. Op deze manier wordt de *Player* niet alleen afgestraft voor fouten maar ook aangemoedigt voor correcte handelingen.

        private void FixedUpdate()
        {
            if (isJumpReady)
            {
                RequestDecision();
            }

            Transform floor = environment.transform.Find("Floor");

            if (transform.position.y - floor.position.y <= 1)
            {
                AddReward(0.001f);
            }
        }

Hier wordt het starten en eindigen van het learning-proces van de *Agent* bepaald.
```
        public override void Initialize()
        {
            base.Initialize();
            body = GetComponent<Rigidbody>();
            environment = GetComponentInParent<Environment>();
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (Mathf.FloorToInt(vectorAction[0]) == 1)
            {
                Jump();
            }
        }

        public override void OnEpisodeBegin()
        {
            body.angularVelocity = Vector3.zero;
            body.velocity = Vector3.zero;
            body.useGravity = true;

            environment.ClearEnvironment();
            if (environment.GetComponentInChildren<Player>() == null)
            {
                environment.SpawnPlayer();
            }
        }
```
Hier bepalen we de score die de *Player* verliest indien hij in aanraking komt met een *Obstacle*. Er is te zien hoe de player 1 punt verliest voor zowel het aanraken van het *Obstacle* als *ObstacleCross* dat uit de andere richting komt. Hij moet dus leren over de *Obstacles* van beide kanten heen te springen.

        private void OnCollisionEnter(Collision collision)
        {
            Collider collider = collision.collider;
            if (collider.CompareTag("Obstacle"))
            {
                AddReward(-1f);
                EndEpisode();
            }
            else if (collider.CompareTag("ObstacleCross"))
            {
                AddReward(-1f);
                EndEpisode();
            }
            else if (collider.CompareTag("Floor"))
            {
                isJumpReady = true;
            }
        }
    }


### *Obstacle script*
Het "Obstacle"-script zorgt voor de beweging van de *Obstacles* en *ObstaclesCross*. De snelheid van de *Obstacles* wordt hier bepaald en vanaf de *Obstacles* mogen worden vernietigd.

Declaratie van de variabelen.
```
using UnityEngine;

namespace Assets.Scripts
{
    public class Obstacle : MonoBehaviour
    {
        public float givenSpeed = 4f;
        public bool constantGivenSpeed = true;
        public float minSpeed = 1f;
        public float maxSpeed = 10;

        private float randomizedSpeed = 0f;
        private Environment environment;

        public Player ply;
```
Hier bepalen we de snelheid van het *Obstacle*
```
        public void Start()
        {
            if (!constantGivenSpeed)
            {
                randomizedSpeed = Random.Range(minSpeed, maxSpeed);
            }
            else
            {
                randomizedSpeed = givenSpeed;
            }
        }
```
Hier doen we het *Obstacle* bewegen met de random bepaalde snelheid.

        private void FixedUpdate()
        {
            if (randomizedSpeed > 0f)
            {
                Move();
            }
        }

        private void Move()
        {
            if (environment == null)
            {
                environment = GetComponentInParent<Environment>();
            }

            transform.Translate(Vector3.forward * Time.deltaTime * randomizedSpeed);
        }

Hier gaan we kijken of een *Obstacle* over de *DeadLine* gaat en indien dit het geval is wordt het *Obstacle* vernietigd. 
Ook bepalen we hier dat de *Player* een punt toegekend krijgt indien het *Obstacle* over de *DeadLine* gaat omdat dit dan wilt zeggen dat de *Player* over het *Obstacle* is gesprongen. 
```

        private void OnTriggerEnter(Collider other)
        {
            bool isDeadLineCollission = other.CompareTag("DeadLine");

            bool isDeadLineCollissionCross = other.CompareTag("DeadLineCross");

            if (isDeadLineCollission || isDeadLineCollissionCross)
            {
                Destroy(gameObject);
                GameObject pl = GameObject.Find("Player");
                Player ply = (Player)pl.GetComponent(typeof(Player));
                ply.AddReward(1f);
            }
        }
    }
}
```

### *DeadLine script*

Een simpel script nodig om de klasse te kunnen koppelen aan het juiste spelobject.  In dit geval het *DeadLine*-object

```
using UnityEngine;

namespace Assets.Scripts
{
    public class DeadLine : MonoBehaviour
    {
    }
}
```

### *SpawnLine script*

Een simpel script nodig om de klasse te kunnen koppelen aan het juiste spelobject. In dit geval het *SpawnLineCross*-object

```
using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnLine : MonoBehaviour
    {
    }
}
```


### *SpawnLineCross script*

Een simpel script nodig om de klasse te kunnen koppelen aan het juiste spelobject. In dit geval het *SpawnLineCross*-object

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLineCross : MonoBehaviour
{

}
```

## Scènes

Een scène kan in Unity geopend worden om een bepaalde omgeving te gebruiken. Voor dit project hebben we een *MLScene* opgebouwd waarin alle elementen van dit project in terug te vinden zijn.

### *MLScene*

![MLScene](Img/scene.png)

## De training

Van zodra bovenstaande stappen zijn afgewerkt en de omgeving klaar is kan je beginnen met de agent te trainen.

Om te beginnen open je het net gemaakte Unity-project en een terminal venster in de "learning"-map van het project.

In het terminal-venster typ je dit commando: ``` mlagents-learn Player.yml --run-id Player-01 ```. De benaming "Player-01" kan aangepast worden per trainingssessie. 

Vervolgens keer je terug naar Unity, en druk je op de "Play"-knop. Het spel zal beginnen en de agent zal beginnen leren. 

Om het verloop van het leerproces visueel op te volgen open je een nieuw terminal venster in de map Learning waarin je het commando ``` tensorboard --logdir results ``` typt.

Vervolgens zal je via http://localhost:6006 de grafieken van Tensorflow kunnen raadplegen die beschrijven hoe goed de agent aan het leren is aan de hand van hoeveel rewards deze heeft gecumuleerd.


## Opmerkingen

Voor dit project hebben wij ons voor bepaalde onderdelen gebaseerd op het project van de groep van Dana.
We hebben er wel voor gezorgd dat het project uniek is door een andere uitbreiding voor het project te kiezen, namelijk de kruising met obstakels die van twee richtingen komen. 
We hebben vele nieuwe elementen toegevoegd en drastische wijzigingen aangebracht aan de overgenomen elementen. 
Hierdoor zijn we van mening dat ons project voldoende uniek is en we enkel de overgenomen elementen als richtlijn gebruikt hebben.