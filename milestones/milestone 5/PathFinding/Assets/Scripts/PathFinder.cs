using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder
{
    public static TilePath DiscoverPath(Tilemap map, Vector3Int start, Vector3Int end)
    {
        //you will return this path to the user.  It should be the shortest path between
        //the start and end vertices 
        TilePath discoveredPath = new TilePath();

        //TileFactory is how you get information on tiles that exist at a particular vector's
        //coordinates
        TileFactory tileFactory = TileFactory.GetInstance();

        //This is the priority queue of paths that you will use in your implementation of
        //Dijkstra's algorithm
        PriortyQueue<TilePath> pathQueue = new PriortyQueue<TilePath>();

        //You can slightly speed up your algorithm by remembering previously visited tiles.
        //This isn't strictly necessary.
        Dictionary<Vector3Int, int> discoveredTiles = new Dictionary<Vector3Int, int>();

        //quick sanity check
        if (map == null || start == null || end == null)
        {
            return discoveredPath;
        }

        //This is how you get tile information for a particular map location
        //This gets the Unity tile, which contains a coordinate (.Position)
        var startingMapLocation = map.GetTile(start);

        //And this converts the Unity tile into an object model that tracks the
        //cost to visit the tile.
        var startingTile = tileFactory.GetTile(startingMapLocation.name);
        startingTile.Position = start;

        //Any discovered path must start at the origin!
        discoveredPath.AddTileToPath(startingTile);

        //This adds the starting tile to the PQ and we start off from there...
        pathQueue.Enqueue(discoveredPath);
        bool found = false;
        while (found == false && pathQueue.IsEmpty() == false)
        {
            //TODO: Implement Dijkstra's algorithm!
            int v;                                 // The current vertex
            int[] D;
            PriortyQueue[] Edge = new PriortyQueue[pathQueue.GetSize()];        // Heap for edges
            Edge[0] = new PriortyQueue(0, v);               // Initial vertex
            PriortyQueue.MinHeap Heap = new MinHeap(Edge, 1, pathQueue.GetSize());
            for (int i = 0; i < PriortyQueue.Count; i++)            // Initialize distance
                D[i] = null;
            D[v] = 0;
            for (int i = 0; i < PriortyQueue.Count; i++)
            {          // For each vertex
                do
                {
                    PriortyQueue temp = (PriortyQueue)(Heap.Dequeue());
                    if (temp == null) return;       // Unreachable nodes exist
                    v = (Integer)temp.value();
                } // Get position
                while (PriortyQueue.GetFirst(v) == true);
                PriortyQueue.setValue(v, true);
                if (D[v] == false) return;        // Unreachable
                int[] nList = PriortyQueue.adjustHeap(v);
                for (int j = 0; j < nList.length; j++)
                {
                    {
                        int w = nList[j];
                        if (D[w] > (D[v] + AddTileToPath(v)))
                        { // Update D
                            D[w] = D[v] + AddTileToPath(v);
                            Heap.insert(new pathQueue(D[w], w));
                        }

                        //pathQueue.Dequeue();
                        //This line ensures that we don't get an infinite loop in Unity.
                        //You will need to remove it in order for your pathfinding algorithm to work.
                        found = true;
                    }
                    return discoveredPath;
                }
            }
        }
    }
}
