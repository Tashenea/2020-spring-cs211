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
        var endingMapLocation = map.GetTile(end);
        //And this converts the Unity tile into an object model that tracks the
        //cost to visit the tile.
        var startingTile = tileFactory.GetTile(startingMapLocation.name);
        startingTile.Position = start;
        var endingTile = tileFactory.GetTile(endingMapLocation.name);
        endingTile.Position = end;
        //Any discovered path must start at the origin!
        discoveredPath.AddTileToPath(startingTile);

        //This adds the starting tile to the PQ and we start off from there...
        pathQueue.Enqueue(discoveredPath);
        bool found = false;
        while (found == false && pathQueue.IsEmpty() == false)
        {
            //TODO: Implement Dijkstra's algorithm!
            TilePath currentPath = new TilePath(pathQueue.Dequeue());

            // Step 1, we found the spot
            Vector3Int Location = currentPath.GetMostRecentTile().Position; // TO-DO: Get most recent tile's location, assign to variable

            if (Location == endingTile.Position) // If currentLocation == End location
            {
                discoveredPath = currentPath;
                break;
            }

            // Step 2, we didn't find the spot, Dijksta's algorithm part 2.

            // Do this 4 times, once for each of these locations:
            // where O is our current path, and X is the next path we go to
            //  _X_ ___ ___ ___
            //  _O_ _O_ _OX XO_
            //  ___ _X_ ___ ___

            // Get location of X

            Vector3Int down = Location;
            Vector3Int up = Location;
            Vector3Int right = Location;
            Vector3Int left = Location;

            down.y -= 1;
            up.y += 1;
            right.x += 1;
            left.x -= 1;

            Vector3Int[] next_steps = new Vector3Int[4];
            next_steps[0] = down;
            next_steps[1] = up;
            next_steps[2] = right;
            next_steps[3] = left;

            for (int i = 0; i < 4; i++)
            {

                // Get the tile and set to position of X
                var xTileLocation = map.GetTile(next_steps[i]);
                // Get the tile
                var xTile = tileFactory.GetTile(xTileLocation.name);
                xTile.Position = next_steps[i];

                // Now we make a brand new path based on our currento ne.
                TilePath copyForX = new TilePath(currentPath); // Done

                // Add xTile to copyForX
                copyForX.AddTileToPath(xTile); // todo

                if(next_steps[i] == endingTile.Position)
                {
                    found = true;
                    break;
                }
                else
                {
                    pathQueue.Enqueue(copyForX);
                }
                // Enqueue to pathQueue
                pathQueue.Enqueue(copyForX); // Todo
            }
        
        // Repeat for every X value (4 total) // todo





        //pathQueue.Dequeue();
        //This line ensures that we don't get an infinite loop in Unity.
        //You will need to remove it in order for your pathfinding algorithm to work.
        found = true;
    }
        return discoveredPath;
    }
}
            

        
    

