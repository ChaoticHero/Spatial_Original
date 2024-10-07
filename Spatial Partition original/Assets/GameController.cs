using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpatialPartitionPattern{
    public class GameController : MonoBehaviour{
        [SerializeField] private Text timetext;
        public GameObject friendlyObj;
        public GameObject enemyObj;
        public Material enemyMaterial;
        public Material closestEnemyMaterial;
        public Transform enemyParent;
        public Transform friendlyParent;
        public Text counterText;
        private int numberOfSoldiers = 0;
        private bool isSpatial;
        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();
        List<Soldier> closestEnemies = new List<Soldier>();
        float mapWidth = 50f;
        int cellSize = 10;
        Grid grid;

        void Start(){
            isSpatial = false;
            grid = new Grid((int)mapWidth, cellSize);
            UpdateText();
            SpawnAll();
        }

        void SpawnAll(){
            for(int i = 0; i < numberOfSoldiers; i++){
                Vector3 randomPos = new Vector3(Random.Range(0f,mapWidth), 0.5f, Random.Range(0f,mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));
                newEnemy.transform.parent = enemyParent;
                randomPos = new Vector3(Random.Range(0f,mapWidth), 0.5f, Random.Range(0f,mapWidth));
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;
                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));
                newFriendly.transform.parent = friendlyParent;
            }
        }

        void Update(){
            float startTime = Time.realtimeSinceStartup;

            for (int i = 0; i < enemySoldiers.Count; i++){
                enemySoldiers[i].Move();
            }
            for(int i = 0; i < closestEnemies.Count; i++){
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }
            closestEnemies.Clear();
            for(int i = 0; i < friendlySoldiers.Count; i++){
                Soldier closestEnemy = null;
                if(!isSpatial){
                    closestEnemy = FindClosestEnemySlow(friendlySoldiers[i]);
                }else if(isSpatial){
                    closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);
                }
                if(closestEnemy != null){
                    closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;
                    closestEnemies.Add(closestEnemy);
                    friendlySoldiers[i].Move(closestEnemy);
                }
            }
            float elapsedTime = (Time.realtimeSinceStartup - startTime) * 1000f;
            timetext.text = "Time:" + elapsedTime;
            Debug.Log(elapsedTime + "ms");
        }

        Soldier FindClosestEnemySlow(Soldier soldier){
            Soldier closestEnemy = null;
            float bestDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemySoldiers.Count; i++){
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;
                if (distSqr < bestDistSqr){
                    bestDistSqr = distSqr;
                    closestEnemy = enemySoldiers[i];
                }
            }
            return closestEnemy;
        }

        public void OnToggleSpatialButton(){
            isSpatial = !isSpatial;
        }

        public void OnResetButton(){
            SceneManager.LoadScene(0);
        }

        public void AddCountButton(int counterToAdd){
            numberOfSoldiers += counterToAdd;
            UpdateText();
            SpawnAll();
        }

        void UpdateText(){
            counterText.text = "Number: " + numberOfSoldiers;
        }
    }
}