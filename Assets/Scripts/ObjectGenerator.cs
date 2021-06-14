using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectGenerator : NetworkBehaviour
{
    public Sprite[] spriteFiles;
    public List<Material> outlinerMaterials;
    public List<GameObject> randomObjects;
    float timer;
    float creatingTime = 2f;
    float creatingRange = 10f;
    int objectsToCreate = 2;

    [SyncVar]
    public int playersConnected;

    public BoxState boxState;

    void Start()
    {
        boxState = new BoxState
        {
            randoms = new List<int>(new int[objectsToCreate]),
            spawnPositions = new List<Vector2>(new Vector2[objectsToCreate])
        };

        timer = creatingTime;
    }

    private void Update()
    {
        if (GameSparks.Core.GS.Authenticated)
        {
            if (isServer)
            {
                playersConnected = NetworkServer.connections.Count;
            }

            if (playersConnected == 1)
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    timer = creatingTime;
                    ProcessingBoxState();
                }
            }      
        }
    }

    public void ProcessingBoxState()
    {
        if (isServer)
        {
            for (int i = 0; i < objectsToCreate; i++)
            {
                boxState.randoms[i] = Random.Range(0, spriteFiles.Length);
                boxState.spawnPositions[i] = new Vector2(0, 0) * Time.deltaTime;
                boxState.spawnPositions[i] = this.transform.position + Random.onUnitSphere * creatingRange;
                boxState.spawnPositions[i] = new Vector2(boxState.spawnPositions[i].x, this.transform.position.y);           
                GameObject randomObject = Instantiate(randomObjects[i], boxState.spawnPositions[i], Quaternion.identity);
                randomObject.GetComponent<SpriteRenderer>().sprite = spriteFiles[boxState.randoms[i]];
                randomObject.GetComponent<SpriteRenderer>().sprite = spriteFiles[boxState.randoms[i]];
                randomObject.GetComponent<SpriteRenderer>().material = outlinerMaterials[i];

                if (NetworkServer.active)
                    NetworkServer.Spawn(randomObject);
            }
        }
    }
}
