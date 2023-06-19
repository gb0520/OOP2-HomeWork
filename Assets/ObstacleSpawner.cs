using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] ZB.Object.ObjectsScrolling scroll;
    [SerializeField] ZB.Object.ObjectPool pool;
    [SerializeField] Transform spawnSetsFold;
    [SerializeField] float spawnDelay;

    [Space]
    [SerializeField] SpawnSet[] spawnSets;
    WaitForSeconds wfs_spawnDelay;

    public void SpawnCycleStart()
    {
        SpawnCycle_C = SpawnCycle();
        StartCoroutine(SpawnCycle_C);
    }
    public void SpawnCycleStop()
    {
        StopCoroutine(SpawnCycle_C);
        scroll.DetachAll();
    }

    private void RandomPattern()
    {
        int randomIndex = Random.Range(0, spawnSets.Length);
        spawnSets[randomIndex].Spawn(pool, scroll);
    }
    private void Awake()
    {
        wfs_spawnDelay = new WaitForSeconds(spawnDelay);
        List<SpawnSet> tempList = new List<SpawnSet>();
        for (int i = 0; i < spawnSetsFold.childCount; i++)
        {
            if (spawnSetsFold.GetChild(i).gameObject.activeSelf) 
                tempList.Add(new SpawnSet(spawnSetsFold.GetChild(i)));
        }
        spawnSets = new SpawnSet[tempList.Count];
        for (int i = 0; i < tempList.Count; i++)
        {
            spawnSets[i] = tempList[i];
        }
    }
    IEnumerator SpawnCycle_C;
    IEnumerator SpawnCycle()
    {
        while (true)
        {
            yield return wfs_spawnDelay;
            RandomPattern();
        }
    }

    [System.Serializable]
    public class SpawnSet
    {
        [SerializeField] Transform[] spawnPoses_CanBreak;
        [SerializeField] Transform[] spawnPoses_CantBreak;

        public void Spawn(ZB.Object.ObjectPool pool, ZB.Object.ObjectsScrolling scrolling)
        {
            for (int i = 0; i < spawnPoses_CanBreak.Length; i++)
            {
                scrolling.AttachObj(
                    pool.Spawn<ZB.Object.FlexbieObject>("ObstacleCanBreak", spawnPoses_CanBreak[i].position));
            }
            for (int i = 0; i < spawnPoses_CantBreak.Length; i++)
            {
                scrolling.AttachObj(
                    pool.Spawn<ZB.Object.FlexbieObject>("ObstacleCantBreak", spawnPoses_CantBreak[i].position));
            }
        }
        public SpawnSet(Transform target)
        {
            List<Transform> list_CanBreak = new List<Transform>();
            List<Transform> list_CantBreak = new List<Transform>();
            for (int i = 0; i < target.childCount; i++)
            {
                string obstacleType = target.GetChild(i).name.Split('_')[0];
                switch (obstacleType)
                {
                    case "ObstacleCanBreak":
                        list_CanBreak.Add(target.GetChild(i));
                        break;
                    case "ObstacleCantBreak":
                        list_CantBreak.Add(target.GetChild(i));
                        break;
                }
            }

            spawnPoses_CanBreak = new Transform[list_CanBreak.Count];
            for (int i = 0; i < list_CanBreak.Count; i++)
            {
                spawnPoses_CanBreak[i] = list_CanBreak[i];
            }

            spawnPoses_CantBreak = new Transform[list_CantBreak.Count];
            for (int i = 0; i < list_CantBreak.Count; i++)
            {
                spawnPoses_CantBreak[i] = list_CantBreak[i];
            }
        }
    }
}
