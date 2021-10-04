using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public GameObject sectionPrefab;
	public GameObject subSectionPrefab;
	
	[Header("Props")]
	public GameObject elevatorPrefab;
	public GameObject elevatorSpawnPrefab;
	public List<GameObject> propPrefabList;
	[Tooltip("Mapped by index to propPrefabList")]
	public List<int> propCountList;
	
	protected List<HashSet<int[]>> mapGrid; // {grid #, local x, local y}
	protected List<int[]> elevatorLocationPool;
	protected List<int[]> elevatorSpawnLocationPool; // Location for spawn
	protected int[] elevatorLocation;
	
	protected Dictionary<GameObject, int> propPool;
	
	protected GameObject elevatorSpawnObject;
	protected GameObject elevatorObject;
	protected Elevator elevator;
	
	protected GameObject playerObject;
	
    protected virtual void Awake()
	{
		mapGrid = new List<HashSet<int[]>>();
		elevatorLocationPool = new List<int[]>();
		elevatorLocation = new int[3];
		elevatorSpawnLocationPool = new List<int[]>();
		
		propPool = new Dictionary<GameObject, int>();
		for (int i=0; i<propPrefabList.Count; i++) {
			propPool.Add(propPrefabList[i], 
				propCountList[i]*(DataManager.floorLevel+1));
		}
		
		GenerateSections();
		GenerateProps();
		
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null && elevatorSpawnObject != null) {
			playerObject.transform.position = 
				elevatorSpawnObject.transform.position + new Vector3(0,0.31f,0);
		}
	}
	
	protected virtual void GenerateProps()
	{
		List<int[]> totalCells = new List<int[]>();
		
		for (int i=0; i<mapGrid.Count; i++) {
			foreach (int[] cell in mapGrid[i]) {
				totalCells.Add(cell);
			}
		}
		
		foreach (KeyValuePair<GameObject, int> prop in propPool) {
			for (int i=0; i<prop.Value; i++) {
				int cellIndex = Random.Range(0,totalCells.Count);
				int[] cell = totalCells[cellIndex];
				
				GameObject newProp = Instantiate(prop.Key, 
					transform.Find("Section"+cell[0].ToString()).Find("Grid"));
				newProp.name = prop.Key.name + 
					"(" + cell[1].ToString() + "," + cell[2].ToString() + ")";
					
				newProp.transform.localPosition = 
					new Vector3(cell[1],0,cell[2]);
					
				totalCells.RemoveAt(cellIndex); // Not optimized, still has to shift
			}
		}
		
		if (elevatorPrefab != null && elevator == null) {
			Vector3 spawnPosition = 
				new Vector3(elevatorLocation[1], 0, elevatorLocation[2]);
			elevatorObject = Instantiate(elevatorPrefab, 
				transform.Find("Section"+elevatorLocation[0].ToString()).Find("Grid"));
			elevatorObject.name = 
				elevatorPrefab.name + "(" + elevatorLocation[1].ToString() 
				+ "," + elevatorLocation[2].ToString() + ")";
			elevatorObject.transform.localPosition = spawnPosition;
				
			elevator = elevatorObject.transform.Find("Model").GetComponent<Elevator>();
			if (elevator != null) {
				elevator.SetStartPosition(elevatorObject.transform.position);
			}
		}
		
		if (elevatorSpawnPrefab != null) {
			int index = Random.Range(0,elevatorSpawnLocationPool.Count);
			int[] elevatorSpawnLocation = elevatorSpawnLocationPool[index];
			elevatorSpawnObject = Instantiate(elevatorSpawnPrefab, 
				transform.Find("Section"+elevatorSpawnLocation[0].ToString()).Find("Grid"));
			Vector3 spawnPosition = 
				new Vector3(elevatorSpawnLocation[1], 0, 
					elevatorSpawnLocation[2] - 2);
			elevatorSpawnObject.transform.localPosition = spawnPosition;
		}
	}
	
	protected virtual void GenerateSections()
	{
		// gridSize = 40;
		int subSectionCount = 14;
		int sectionCount = Mathf.Min((int)Mathf.Floor(DataManager.floorLevel/2) + 1, 6);
		
		if (sectionPrefab != null && subSectionPrefab != null) {
			
			int elevatorSectionIndex = Random.Range(0, sectionCount);
			
			for (int i=0; i<sectionCount; i++) {
				
				mapGrid.Add(new HashSet<int[]>());
				
				GameObject section = Instantiate(sectionPrefab, 
					new Vector3(40*i,0,0), Quaternion.identity, transform);
				section.name = "Section"+i.ToString();
				
				// Generate subSections
				HashSet<int> guaranteedZones = new HashSet<int>();
				guaranteedZones.Add(elevatorSectionIndex);
				guaranteedZones.Add(Random.Range(13,14));
				
				List<int> subSectionIndexList = new List<int>();
				for (int o=0; o<16; o++) {
					if (!guaranteedZones.Contains(o)) {
						subSectionIndexList.Add(o);
					}
				}
				int removeCount = 16 - subSectionCount;
				for (int o=0; o<removeCount; o++) {
					subSectionIndexList.RemoveAt(
						Random.Range(0, subSectionIndexList.Count));
				}
				// Add subSectionIndexList indices into guaranteedZones
				for (int o=0; o<subSectionIndexList.Count; o++) {
					guaranteedZones.Add(subSectionIndexList[o]);
				}
				
				for (int o=0; o<16; o++) {
					if (guaranteedZones.Contains(o)) {
						GameObject subSection = 
							Instantiate(subSectionPrefab, section.transform);
						subSection.name = "SubSection" + o.ToString();
						subSection.transform.localPosition = new Vector3(
							10*(o%4), 0, -10*Mathf.Floor(o/4));
						// Add all available cells to mapGrid
						int[] startCell = 
							new int[] {(int)(10*(o%4)), (int)(-10*Mathf.Floor(o/4) )};
						for (int x=0; x<10; x++) {
							for (int y=0; y<10; y++) {
								int[] newCell = 
									new int[] {i, startCell[0] + x, 
										startCell[1] - y};
								if (newCell[2] < -4 && newCell[2] >= -37) { 
									// Exclude first and last few rows
									mapGrid[i].Add(newCell);
								}
								else if (newCell[2] == -39 
									&& newCell[1] >= 10 && newCell[1] <= 30) {
									elevatorSpawnLocationPool.Add(newCell);
								}
								else if (newCell[2] == -1) {
									elevatorLocationPool.Add(newCell);
								}
							}
						}
					}	
				}
				
				if (i == elevatorSectionIndex) {
					int index = Random.Range(0,elevatorLocationPool.Count);
					elevatorLocation = elevatorLocationPool[index];
					// mapGrid[i].Remove(elevatorLocation);
				}
				
			}
		}
	}
	
	
}
