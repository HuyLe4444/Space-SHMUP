using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Main : MonoBehaviour
{
    static private Main S;
    static private Dictionary<eWeaponType, WeaponDefinition> WEAP_DICT;
    [Header("Inscribed")]
    public TextMeshProUGUI timeText;
    public bool spawnEnemies = true;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyInsetDefault = 1.5f;
    public float gameRestartDelay = 2;
    public WeaponDefinition[] weaponDefinitions;
    public float spawnIncreaseInterval = 5f; // Time interval to increase spawn rate
    public float spawnIncreaseFactor = 0.1f;  // Factor to increase spawn rate
    public float minSpawnRate = 0.1f;         // Optional: Minimum limit for spawn rate
    public float currTime;
    
    private float elapsedTime = 0f;
    private BoundsCheck bndCheck;
    void Start() {
        timeText.text = "Time: 00s";
    }
    void Update() {
        // Track time for spawn rate increase
        elapsedTime += Time.deltaTime;
        currTime += Time.deltaTime;
        timeText.text = "Time: " + Mathf.Max(0, currTime).ToString("F0") + "s";

        // Every spawnIncreaseInterval seconds, increase the spawn rate
        if (elapsedTime >= spawnIncreaseInterval) {
            IncreaseSpawnRate();
            elapsedTime = 0f; // Reset the timer
        }
    }
    void IncreaseSpawnRate() {
        // Decrease the time between enemy spawns, increasing spawn rate
        enemySpawnPerSecond += spawnIncreaseFactor;

        // Ensure spawn rate doesn't go beyond a certain minimum threshold
        if (enemySpawnPerSecond > 1f / minSpawnRate) {
            enemySpawnPerSecond = 1f / minSpawnRate;  // Clamp to minimum spawn rate
        }

        // Re-schedule enemy spawning with the new spawn rate
        CancelInvoke(nameof(SpawnEnemy)); 
        Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);
    }
    void Awake(){
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);

        WEAP_DICT = new Dictionary<eWeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions) {
            WEAP_DICT[def.type] = def;
        }
    }
    public void SpawnEnemy() {
        if(!spawnEnemies) {
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
            return;
        }
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        float enemyInset = enemyInsetDefault;
        if(go.GetComponent<BoundsCheck>() != null){
            enemyInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = Random.Range(xMin,xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;

        Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);
    }

    void DelayedRestart() {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart(){
        SceneManager.LoadScene("__Scene_0");
    }

    static public void HERO_DIED() {
        S.DelayedRestart();
    }

    static public WeaponDefinition GET_WEAPON_DEFINITION(eWeaponType wt) {
        if(WEAP_DICT.ContainsKey(wt)) {
            return(WEAP_DICT[wt]);
        }
        return(new WeaponDefinition());
    }
}
