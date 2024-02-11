using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

public class OctreeBlock
{
    public Vector3Int block_position;
    public List<GameObject> stars_in_block = new List<GameObject>();
}

public class CSV_reader : MonoBehaviour
{
    public GameObject O_prefab;
    public GameObject B_prefab;
    public GameObject A_prefab;
    public GameObject F_prefab;
    public GameObject G_prefab;
    public GameObject K_prefab;
    public GameObject M_prefab;
    int star_counter = 0;

    Dictionary<int, GameObject> starDictionary;

    public Dictionary<Vector3Int, OctreeBlock> octreeBlocks;

    void spawnStar(char spect,Vector3 cubePosition,int star_hip_id)
    {
        Debug.Log("sidlog spawnStar star_counter:"+ star_counter+"star id:"+ star_hip_id);
        /*
        if (star_counter >= 100)
        {
            return;
        }
        */
        GameObject spawnedStar = null;
        switch (spect)
        {
            case 'O':
                //Debug.Log("Value is O");
                spawnedStar  = Instantiate(O_prefab, cubePosition, Quaternion.identity);
                break;

            case 'B':
                //Debug.Log("Value is B");
                spawnedStar = Instantiate(B_prefab, cubePosition, Quaternion.identity);
                break;

            case 'A':
                //Debug.Log("Value is A");
                spawnedStar = Instantiate(A_prefab, cubePosition, Quaternion.identity);
                break;

            case 'F':
                //Debug.Log("Value is F");
                spawnedStar = Instantiate(F_prefab, cubePosition, Quaternion.identity);
                break;

            case 'G':
                //Debug.Log("Value is G");
                spawnedStar = Instantiate(G_prefab, cubePosition, Quaternion.identity);
                break;

            case 'K':
                //Debug.Log("Value is K");
                spawnedStar = Instantiate(K_prefab, cubePosition, Quaternion.identity);
                break;

            case 'M':
                //Debug.Log("Value is M");
                spawnedStar = Instantiate(M_prefab, cubePosition, Quaternion.identity);
                break;

            default:
                Debug.Log("Value is not 1, 2, or 3");
                break;
        }
        
        if (!starDictionary.ContainsKey(star_hip_id))
        {
            starDictionary.Add(star_hip_id, spawnedStar);
        }
        starMetaData starScript = spawnedStar.GetComponent<starMetaData>();

        if (starScript != null)
        {
            
            starScript.hip_id = star_hip_id;
        }

        if (spawnedStar != null)
        {
            Vector3 starPosition = spawnedStar.transform.position;
            Vector3Int blockPosition = new Vector3Int(
                Mathf.FloorToInt(starPosition.x / 20) * 20,
                Mathf.FloorToInt(starPosition.y / 20) * 20,
                Mathf.FloorToInt(starPosition.z / 20) * 20
            );

            // Create octree block if it doesn't exist
            if (!octreeBlocks.ContainsKey(blockPosition))
            {
                octreeBlocks[blockPosition] = new OctreeBlock { block_position = blockPosition };
            }

            // Add star to the corresponding octree block
            octreeBlocks[blockPosition].stars_in_block.Add(spawnedStar);

            /*
            // Disable the renderer
            Renderer objectRenderer = spawnedStar.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.enabled = false;
            }
            */

            spawnedStar.SetActive(false);
        }
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        starDictionary = new Dictionary<int, GameObject>();
        octreeBlocks = new Dictionary<Vector3Int, OctreeBlock>();
        StartCoroutine(LoadCSVFile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject getStarByID(int hip_id)
    {
        if (starDictionary.ContainsKey(hip_id))
        {
            return starDictionary[hip_id];
        }
        else
        {
            Debug.LogError("sidlog Object with ID " + hip_id + " not found.");
            return null;
        }
    }

    void DrawLinebetweenStars(Vector3 star_01Pos, Vector3 star_02Pos,Material randMaterial)
    {


        GameObject lineRendererObject = new GameObject("LineRenderer");
        LineRenderer lineRenderer = lineRendererObject.AddComponent<LineRenderer>();
        lineRenderer.material = randMaterial;
        lineRenderer.startWidth = 0.06f;
        lineRenderer.endWidth = 0.06f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, star_01Pos);
        lineRenderer.SetPosition(1, star_02Pos);
    }
    void loadConstellation()
    {
        Debug.Log("loading constallations...");
        string constellationFileName = "constellationship.txt";
        string constellationPath = Path.Combine(Application.streamingAssetsPath, constellationFileName);

        // Check if the file exists
        if (File.Exists(constellationPath))
        {
            // Open a StreamReader to read the CSV file
            using (StreamReader reader = new StreamReader(constellationPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] values = Regex.Split(line, @"\s+");

                    Debug.Log("length:" + values.Length);
                    if (values.Length > 2) //sanity check
                    {
                        // Load materials from Resources folder
                        Material aMat = Resources.Load<Material>("A_mat");
                        Material bMat = Resources.Load<Material>("B_mat");
                        Material fMat = Resources.Load<Material>("F_mat");
                        Material gMat = Resources.Load<Material>("G_mat");
                        Material kMat = Resources.Load<Material>("K_mat");
                        Material mMat = Resources.Load<Material>("M_mat");
                        Material oMat = Resources.Load<Material>("O_mat");
                        // Select a random material
                        Material[] materials = { aMat, bMat, fMat, gMat, kMat, mMat, oMat };
                        Material randomMaterial = materials[Random.Range(0, materials.Length)];

                        int num_pairs = int.Parse(values[1]);
                        Debug.Log("constellation:" + values[0] + "num of pairs:" + num_pairs); ;
                        if (true)
                        {
                            for (int i = 0; i < (num_pairs * 2) - 1; i = i + 2)
                            {
                                int id1 = int.Parse(values[2 + i]);
                                int id2 = int.Parse(values[2 + i + 1]);

                                Debug.Log("constellation pair id1:" + id1 + " id2:" + id2);

                                GameObject star_01 = getStarByID(id1);
                                GameObject star_02 = getStarByID(id2);


                                if (star_01 != null && star_02 != null)
                                {
                                    DrawLinebetweenStars(star_01.transform.position, star_02.transform.position, randomMaterial);
                                }
                            }
                        }

                    }

                
                }
            }
        }
        else
        {
            Debug.LogError($"CSV file not found at path: {constellationPath}");
        }
    }
    IEnumerator LoadCSVFile()
    {
        

        // Specify the path to your CSV file
        string csvFileName = "processed_STAR_31_m10.csv";
        string csvPath = Path.Combine(Application.streamingAssetsPath, csvFileName);
        CultureInfo culture = new CultureInfo("en-US");
        // If loading from Resources folder, modify the path
        // string csvPath = "Assets/Resources/cleaned_data.csv";

        // Check if the file exists
        if (File.Exists(csvPath))
        {
            // Open a StreamReader to read the CSV file
            using (StreamReader reader = new StreamReader(csvPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    
                    string[] values = line.Split(',');

                    float x0, y0, z0,hip_id;

                    if (float.TryParse(values[3], NumberStyles.Float, culture, out x0) &&
                        float.TryParse(values[4], NumberStyles.Float, culture, out y0) &&
                        float.TryParse(values[5], NumberStyles.Float, culture, out z0) &&
                        float.TryParse(values[1], NumberStyles.Float, culture, out hip_id)
                       )
                    {
                        Vector3 cubePosition = new Vector3(x0, z0, y0);
                        spawnStar(values[11][0], cubePosition, (int)hip_id);

                        
                        star_counter  = star_counter+1;
                        //yield return null;
                        // Now you have x0, y0, and z0 for each row
                        Debug.Log("sidlog star_counter:"+ star_counter);
                        //Debug.Log($"sidlog x0: {x0}, y0: {y0}, z0: {z0}");

                    }
                    else
                    {
                        Debug.LogError("Failed to parse float values for x0, y0, z0.");
                    }

                    //yield return null;

                }
            }
        }
        else
        {
            Debug.LogError($"CSV file not found at path: {csvPath}");
        }

        Debug.Log("sidlog done loading csv...");

        loadConstellation();

        int numberOfElements = octreeBlocks.Count;
        Debug.Log("sidlog Number of elements in the dictionary: " + numberOfElements);

        int totalStars = 0;
        int totalBlocks = octreeBlocks.Count;

        foreach (var block in octreeBlocks.Values)
        {
            totalStars += block.stars_in_block.Count;
        }

        float averageStarsPerBlock = (float)totalStars / totalBlocks;
        Debug.Log("sidlog Average number of stars in each block: " + averageStarsPerBlock);


        yield return null;

       

    }
}

