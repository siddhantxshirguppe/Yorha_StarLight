using UnityEngine;
using System.Collections.Generic;
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
    CSV_reader csvReader;
    void Start()
    {
        csvReader  = starloader.GetComponent<CSV_reader>();
        currentOcttreeBlockPosition = new Vector3Int(-1, -1, -1);
        boxSize = new Vector3(x_extent, y_extent, z_extent);
        // Call CheckStarVisibility every 1 second and repeat the call every 1 second
        //InvokeRepeating("CheckStarVisibility", 1f, 1f);
        InvokeRepeating("CheckOctreeActivation", 1f, 1f);
        visibleStars = new Collider[0];
    }
    void CheckOctreeActivation()
    {
        Vector3 shipPosition = boxCenter.transform.position;
        //get the position of the head/ship
        Debug.Log("current position of camera:" + shipPosition);
        //get the cuurent octtree to search

        // Calculate the block position that contains the ship
        Vector3Int ship_blockPosition = new Vector3Int(
            Mathf.FloorToInt(shipPosition.x / 20) * 20,
            Mathf.FloorToInt(shipPosition.y / 20) * 20,
            Mathf.FloorToInt(shipPosition.z / 20) * 20
        );
         

        if (csvReader != null)
        {
            if (ship_blockPosition != currentOcttreeBlockPosition)
            {
                DeactivateStarsInBlock(currentOcttreeBlockPosition, csvReader);
                ActivateStarsInBlock(ship_blockPosition, csvReader);
                currentOcttreeBlockPosition = ship_blockPosition;
            }
        }

                                    /*
                                        // Check if the octree block exists in the dictionary
                                        if (csvReader.octreeBlocks.ContainsKey(blockPosition))
                                        {


                                            Debug.Log("sidlogblock Activate octree block at position: " + block.block_position+"stars present:"+ block.stars_in_block.Count);
                                            // do something compute intensice with the bloack here

                                                // Activate the octree block containing the ship
                                                OctreeBlock block = csvReader.octreeBlocks[blockPosition];
                                                Debug.Log("Perform compute-intensive task for octree block at position: " + block.block_position);
                                                foreach (GameObject star in block.stars_in_block)
                                                {
                                                    // Activate the star or perform any other desired logic
                                                    star.SetActive(true);
                                                }
                    

                                                //activate the neighbouring blocks as well
                                                // Activate neighboring blocks
                                                foreach (Vector3Int neighborOffset in GetNeighborBlockPositions(currentOcttreeBlockPosition))
                                                {
                                                    Vector3Int neighborPosition = blockPosition + neighborOffset;
                                                    if (csvReader.octreeBlocks.ContainsKey(neighborPosition))
                                                    {
                                                        OctreeBlock neighborBlock = csvReader.octreeBlocks[neighborPosition];
                                                        Debug.Log("Activate neighboring octree block at position: " + neighborBlock.block_position + " stars present: " + neighborBlock.stars_in_block.Count);
                                                        // Perform compute-intensive task for neighboring block
                                                        foreach (GameObject star in neighborBlock.stars_in_block)
                                                        {
                                                            // Activate the star or perform any other desired logic
                                                            star.SetActive(true);
                                                        }
                                                    }
                                                }
                                            }

                                    */

        
    }

    void DeactivateStarsInBlock(Vector3Int blockPosition, CSV_reader csvReader)
    {
        
        if (csvReader.octreeBlocks.ContainsKey(blockPosition))
        {
            Debug.Log("sidlogdebug deactivating:" + blockPosition);
            OctreeBlock block = csvReader.octreeBlocks[blockPosition];
            foreach (GameObject star in block.stars_in_block)
            {
                star.SetActive(false);
            }

            // Deactivate stars in neighboring blocks
            foreach (Vector3Int neighborOffset in GetNeighborBlockPositions(blockPosition))
            {
                Vector3Int neighborPosition = neighborOffset;
                
                if (csvReader.octreeBlocks.ContainsKey(neighborPosition))
                {
                    Debug.Log("sidlogdebug NB deactivating:" + neighborPosition);
                    OctreeBlock neighborBlock = csvReader.octreeBlocks[neighborPosition];
                    foreach (GameObject star in neighborBlock.stars_in_block)
                    {
                        star.SetActive(false);
                    }
                }
            }
        }
    }

void ActivateStarsInBlock(Vector3Int blockPosition, CSV_reader csvReader)
    {
        if (csvReader.octreeBlocks.ContainsKey(blockPosition))
        {
            Debug.Log("sidlogdebug activating:" + blockPosition);
            OctreeBlock block = csvReader.octreeBlocks[blockPosition];
            foreach (GameObject star in block.stars_in_block)
            {
                star.SetActive(true);
            }

            // Activate stars in neighboring blocks
            foreach (Vector3Int neighborOffset in GetNeighborBlockPositions(blockPosition))
            {
                Vector3Int neighborPosition =  neighborOffset;
                if (csvReader.octreeBlocks.ContainsKey(neighborPosition))
                {
                    Debug.Log("sidlogdebug NB activating:" + neighborPosition);
                    OctreeBlock neighborBlock = csvReader.octreeBlocks[neighborPosition];
                    foreach (GameObject star in neighborBlock.stars_in_block)
                    {
                        star.SetActive(true);
                    }
                }
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

    private List<Vector3Int> GetNeighborBlockPositions(Vector3Int blockPosition)
    {
        List<Vector3Int> neighborPositions = new List<Vector3Int>();

        // Define the offsets for neighboring blocks in each dimension (x, y, z)
        int offset = 20;
        Vector3Int[] offsets = new Vector3Int[]
        {
        new Vector3Int(-offset, 0, 0), // Left neighbor
        new Vector3Int(offset, 0, 0),  // Right neighbor
        new Vector3Int(0, -offset, 0), // Bottom neighbor
        new Vector3Int(0, offset, 0),  // Top neighbor
        new Vector3Int(0, 0, -offset), // Back neighbor
        new Vector3Int(0, 0, offset),  // Front neighbor
        new Vector3Int(-offset, 0, -offset), // Front-left neighbor
        new Vector3Int(offset, 0, -offset),  // Front-right neighbor
        new Vector3Int(-offset, 0, offset),  // Back-left neighbor
        new Vector3Int(offset, 0, offset)    // Back-right neighbor
        };

        // Calculate the positions of neighboring blocks relative to the current block position
        foreach (Vector3Int neighborOffset in offsets)
        {
            Vector3Int neighborPosition = blockPosition + neighborOffset;
            neighborPositions.Add(neighborPosition);
        }

        return neighborPositions;
    }
}


