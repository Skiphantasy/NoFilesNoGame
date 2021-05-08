using System.Collections.Generic;
using UnityEngine;

public class CreatorObjects : MonoBehaviour
{
    public float creatingTime = 2f;
    public float creatingRange = 2f;
    public Sprite[] spriteFiles;
    private int rand;
    public List<Material> outlinerMaterials;
    public List<GameObject> randomObjects;
    private int objectsToCreate = 2;

    void Start()
    {
        InvokeRepeating("Creating", 0.0f, creatingTime);
    }

    public void Creating()
    {
        for (int i = 0; i < objectsToCreate; i++)
        {
            rand = Random.Range(0, spriteFiles.Length);
            Vector2 spawnPosition = new Vector2(0, 0);
            spawnPosition = this.transform.position + Random.onUnitSphere * creatingRange;
            spawnPosition = new Vector2(spawnPosition.x, this.transform.position.y);
            GameObject randomObject = Instantiate(randomObjects[0], spawnPosition, Quaternion.identity);
            randomObject.GetComponent<SpriteRenderer>().sprite = spriteFiles[rand];
            randomObject.tag = "ObjectFalling" + (i + 1);
            randomObject.name = "FilesPlayer" + (i + 1);
            randomObject.GetComponent<SpriteRenderer>().material = outlinerMaterials[i];
        }
    }
}
