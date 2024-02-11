using UnityEngine;

public class StarVisibilityController : MonoBehaviour
{
    public float x_extent = 10f;
    public float y_extent = 10f;
    public float z_extent = 10f;
    private Vector3 boxSize;
    // Adjust the radius as needed
    public GameObject boxCenter;
    private Collider[] visibleStars;
    public GameObject starloader;
    private Vector3Int currentOcttreeBlockPosition;
    void Start()
    {
        currentOcttreeBlockPosition = new Vector3Int(-1, -1, -1);
        boxSize = new Vector3(x_extent, y_extent, z_extent);
        // Call CheckStarVisibility every 1 second and repeat the call every 1 second
        //InvokeRepeating("CheckStarVisibility", 1f, 1f);
        InvokeRepeating("CheckOctreeActivation", 2f, 2f);
        visibleStars = new Collider[0];
    }
    void CheckOctreeActivation()
    {
        Vector3 shipPosition = boxCenter.transform.position;
        //get the position of the head/ship
        Debug.Log("current position of camera:" + boxCenter.transform.position);
        //get the cuurent octtree to search

        // Calculate the block position that contains the ship
        Vector3Int blockPosition = new Vector3Int(
            Mathf.FloorToInt(shipPosition.x / 20) * 20,
            Mathf.FloorToInt(shipPosition.y / 20) * 20,
            Mathf.FloorToInt(shipPosition.z / 20) * 20
        );
        CSV_reader csvReader = starloader.GetComponent<CSV_reader>();

        if (csvReader != null)
        {
            // Check if the octree block exists in the dictionary
            if (csvReader.octreeBlocks.ContainsKey(blockPosition))
            {
                // Activate the octree block containing the ship
                OctreeBlock block = csvReader.octreeBlocks[blockPosition];
                Debug.Log("sidlogblock Activate octree block at position: " + block.block_position+"stars present:"+ block.stars_in_block.Count);
                // do something compute intensice with the bloack here
                if (blockPosition != currentOcttreeBlockPosition)
                {
                    Debug.Log("Perform compute-intensive task for octree block at position: " + block.block_position);
                    foreach (GameObject star in block.stars_in_block)
                    {
                        // Activate the star or perform any other desired logic
                        star.SetActive(true);
                    }
                    currentOcttreeBlockPosition = blockPosition;
                }
            }
            else
            {
                Debug.LogWarning("sidlogblock No octree block found for the ship's position");
            }
        }
    }
    void CheckStarVisibility()
    {

        // Get all colliders within the detection sphere
        Collider[] starsInBox= Physics.OverlapBox(boxCenter.transform.position, boxSize, boxCenter.transform.rotation);
        Debug.Log("sidlog number of stars inside:" + starsInBox.Length);
        // Make previously visible stars invisible
        foreach (Collider starCollider in visibleStars)
        {
            starCollider.gameObject.SetActive(false);
            /*
            Renderer starRenderer = starCollider.GetComponent<Renderer>();
            if (starRenderer != null)
            {
                starRenderer.enabled = false;
            }
            */
        }

        // Update the list of currently visible stars
        visibleStars = starsInBox;
        Debug.Log("sidlog number of visibleStars: " + visibleStars.Length);
        // Iterate through the colliders and set their renderers to visible
        foreach (Collider starCollider in starsInBox)
        {
            starCollider.gameObject.SetActive(true);
            /*
            Renderer starRenderer = starCollider.GetComponent<Renderer>();
            if (starRenderer != null)
            {
                starRenderer.enabled = true;
            }
            */
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Set the Gizmo color
        Gizmos.color = Color.yellow;

        // Draw the cube using Gizmos.DrawWireCube


        Gizmos.matrix = Matrix4x4.TRS(boxCenter.transform.position, boxCenter.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
