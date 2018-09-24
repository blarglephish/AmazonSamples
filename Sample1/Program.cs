using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample1
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<List<int>> forwardShipRouteList = new List<List<int>>()
            //    { new List<int>() {1, 2000 }, new List<int>() {2, 4000 }, new List<int>() {3, 6000 }, new List<int> {4, 5000 } };
            //List<List<int>> returnShipRouteList = new List<List<int>>()
            //    { new List<int>() {1, 2000 }, new List<int>(){2, 1000 } , new List<int> {3, 2000 }};

            //List<List<int>> list = optimalUtilization(7000, forwardShipRouteList, returnShipRouteList);

            //foreach (List<int> item in list)
            //{
            //    Console.WriteLine("[" + item[0] + ", " + item[1] + "]");
            //}
            int[] heights = { 2, 5, 4, 2, 3, 6 };
            Console.WriteLine("Input Heights: " + heights);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights));
            Console.ReadLine();

            int[] heights2 = { 3, 3, 3, 3, 3};
            Console.WriteLine("Input Heights: " + heights2);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights2));
            Console.ReadLine();

            int[] heights3 = { 1, 2, 3, 4, 5 };
            Console.WriteLine("Input Heights: " + heights3);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights3));
            Console.ReadLine();

            int[] heights4 = { 5, 4, 3, 2, 1};
            Console.WriteLine("Input Heights: " + heights4);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights4));
            Console.ReadLine();

            int[] heights5 = { 2, 1, 1, 1, 2 };
            Console.WriteLine("Input Heights: " + heights5);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights5));
            Console.ReadLine();

            int[] heights6 = { 2, 0, 2 };
            Console.WriteLine("Input Heights: " + heights6);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights6));
            Console.ReadLine();

            int[] heights7 = { 3, 0, 0, 2, 0, 4 };
            Console.WriteLine("Input Heights: " + heights7);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights7));
            Console.ReadLine();

            int[] heights8 = { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };
            Console.WriteLine("Input Heights: " + heights8);
            Console.WriteLine("Trapped Rainfall: " + getTrappedRainfallAmount(heights8));
            Console.ReadLine();
        }

        // METHOD SIGNATURE BEGINS, THIS METHOD IS REQUIRED
        public static List<List<int>> nearestXsteakHouses(int totalSteakhouses,
                                                   int[,] allLocations,
                                                   int numSteakhouses)
        {
            List<List<int>> returnList = new List<List<int>>(numSteakhouses);   // the returned object

            // Helper Objects
            List<KeyValuePair<double, List<int>>> keyValuePairs = new List<KeyValuePair<double, List<int>>>(numSteakhouses);
            for(int i = 0; i < allLocations.GetLength(0); i++)
            {
                int xLocation = allLocations[i, 0];
                int yLocation = allLocations[i, 1];
                double distance = getDistance(xLocation, yLocation);

                keyValuePairs.Add(new KeyValuePair<double, List<int>>(distance, new List<int> { xLocation, yLocation }));
            }

            // Sort the list
            keyValuePairs.Sort((x, y) => x.Key.CompareTo(y.Key));

            for (int i =0; i < numSteakhouses; i++)
            {
                List<int> listEntry = keyValuePairs.ElementAt(i).Value;
                returnList.Add(listEntry);
            }

            return returnList;
        }
        // METHOD SIGNATURE ENDS

        private static double getDistance(int x, int y)
        {
            return Math.Sqrt((x*x) + (y*y));
        }

        // METHOD SIGNATURE BEGINS, THIS METHOD IS REQUIRED
        public static List<List<int>> optimalUtilization(
                                    int maximumOperatingTravelDistance,
                                    List<List<int>> forwardShippingRouteList,
                                    List<List<int>> returnShippingRouteList)
        {
            /**
             * General approach:
             * 1. Creat all pairwise combinations of forward-return travel routes, and add them to a collection
             * 2. Find the optimal distance that is bounded by maximumOperatingTravelDistance
             * 3. Prune the collection of all items whose combined travel distance is less than the optimal
             * 4. Return the remaining collection.
             * */
            List<List<int>> returnList = new List<List<int>>();

            int optimal = 0;
            foreach (List<int> forwardShippingRoute in forwardShippingRouteList)
            {
                foreach (List<int> returnShippingRoute in returnShippingRouteList)
                {
                    int combined = forwardShippingRoute[1] + returnShippingRoute[1];

                    if (combined <= maximumOperatingTravelDistance)
                    {
                        if (combined >= optimal)
                        {
                            optimal = combined;
                            returnList.Add(new List<int> { forwardShippingRoute[0], returnShippingRoute[0] });
                        }
                    }
                }
            }

            /**
             * NOTE: we cannot remove items from returnList as we iterate over it, so we
             * add the items to remove to a seperate collection object. Once done iterating,
             * we remove each item from this 'to be remvoed' collection object
             * */
            List<List<int>> itemsToRemove = new List<List<int>>();
            foreach (List<int> item in returnList)
            {
                int forwardIndex = item[0];
                List<int> forwardRoute = forwardShippingRouteList[forwardIndex - 1];
                int forwardDistance = forwardRoute[1];

                int returnIndex = item[1];
                List<int> returnRoute = returnShippingRouteList[returnIndex - 1];
                int returnDistance = returnRoute[1];

                int combined = forwardDistance + returnDistance;
                if (combined < optimal)
                {
                    itemsToRemove.Add(item);
                }
            }
            foreach (List<int> item in itemsToRemove)
            {
                returnList.Remove(item);
            }

            return returnList;
        }
        // METHOD SIGNATURE ENDS

        public static int getTrappedRainfallAmount(int[] buildingHeights)
        {
            // Initialize return value
            int retVal = 0;

            // index pointers: one at the start of the array, one at the end
            int low = 0;
            int high = buildingHeights.Length - 1;

            // Stores left and right maximum height
            int LMax = 0;
            int RMax = 0;

            while (low < high)
            {
                if (buildingHeights[low] < buildingHeights[high])
                {
                    if (buildingHeights[low] > LMax)
                    {
                        LMax = buildingHeights[low];
                    }
                    else
                    {
                        retVal += LMax - buildingHeights[low];
                    }

                    low++;
                }
                else
                {
                    if (buildingHeights[high] > RMax)
                    {
                        RMax = buildingHeights[high];
                    }
                    else
                    {
                        retVal += RMax - buildingHeights[high];
                    }

                    high--;
                }
            }

            return retVal;
        }
    }
}
