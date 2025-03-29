using System;
using System.Collections.Generic;

namespace AOG
{
    public class CBoundaryList
    {
        public List<vec3> fenceLine = new List<vec3>(128);
        public List<vec2> fenceLineEar = new List<vec2>(128);
        public List<Triangle> fenceTriangleList = new List<Triangle>(128);

        public List<vec3> hdLine = new List<vec3>(128);
        public List<Triangle> hdLineTriangleList = new List<Triangle>(128);

        public List<vec3> turnLine = new List<vec3>(128);

        //public CPolygon bndPolygon = new CPolygon();
        //public CPolygon hdLinePolygon = new CPolygon();

        //boundary variables
        public double area;

        public bool isDriveThru;

        private int idx = 0;
        //constructor
        public CBoundaryList()
        {
            area = 0;
            isDriveThru = false;
        }

        public void FixFenceLine(int bndNum)
        {
            idx = bndNum;

            CalculateFenceArea(bndNum);

            double spacing;
            //close if less then 20 ha, 40ha, more
            if (area < 200000) spacing = 1.1;
            else if (area < 400000) spacing = 2.2;
            else spacing = 3.3;

            if (bndNum > 0) spacing *= 0.5;

            int bndCount = fenceLine.Count;
            double distance;

            //make sure distance isn't too big between points on boundary
            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;

                if (j == bndCount) j = 0;
                distance = glm.Distance(fenceLine[i], fenceLine[j]);
                if (distance > spacing * 1.5)
                {
                    vec3 pointB = new vec3((fenceLine[i].easting + fenceLine[j].easting) / 2.0,
                        (fenceLine[i].northing + fenceLine[j].northing) / 2.0, fenceLine[i].heading);

                    fenceLine.Insert(j, pointB);
                    bndCount = fenceLine.Count;
                    i--;
                }
            }

            //make sure distance isn't too big between points on boundary
            bndCount = fenceLine.Count;

            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;

                if (j == bndCount) j = 0;
                distance = glm.Distance(fenceLine[i], fenceLine[j]);
                if (distance > spacing * 1.6)
                {
                    vec3 pointB = new vec3((fenceLine[i].easting + fenceLine[j].easting) / 2.0,
                        (fenceLine[i].northing + fenceLine[j].northing) / 2.0, fenceLine[i].heading);

                    fenceLine.Insert(j, pointB);
                    bndCount = fenceLine.Count;
                    i--;
                }
            }

            //make sure distance isn't too small between points on headland
            spacing *= 0.9;
            bndCount = fenceLine.Count;
            for (int i = 0; i < bndCount - 1; i++)
            {
                distance = glm.Distance(fenceLine[i], fenceLine[i + 1]);
                if (distance < spacing)
                {
                    fenceLine.RemoveAt(i + 1);
                    bndCount = fenceLine.Count;
                    i--;
                }
            }

            //make sure headings are correct for calculated points
            fenceLine.CalculateHeadings(true);

            double delta = 0;
            fenceLineEar?.Clear();

            for (int i = 0; i < fenceLine.Count; i++)
            {
                if (i == 0)
                {
                    fenceLineEar.Add(new vec2(fenceLine[i]));
                    continue;
                }
                delta += (fenceLine[i - 1].heading - fenceLine[i].heading);
                if (Math.Abs(delta) > 0.005)
                {
                    fenceLineEar.Add(new vec2(fenceLine[i]));
                    delta = 0;
                }
            }

            //Triangulate the bundary polygon
            CPolygon bndPolygon = new CPolygon(fenceLineEar.ToArray());
            fenceTriangleList = bndPolygon.Triangulate();


            BuildTurnLine();
        }

        public void BuildTurnLine()
        {
            //determine how wide a headland space
            double totalHeadWidth = Settings.Vehicle.set_youTurnDistanceFromBoundary;

            turnLine = fenceLine.OffsetLine(totalHeadWidth, 4, true);

            for (int i = turnLine.Count - 1; i >= 0; i--)
            {
                if ((idx == 0) != fenceLineEar.IsPointInPolygon(turnLine[i]))
                {
                    turnLine.RemoveAt(i);
                }
            }

            //make sure headings are correct for calculated points
            turnLine.CalculateHeadings(true);

            //countExit the reference list of original curve
            int cnt = turnLine.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            for (int s = 0; s < cnt; s++)
            {
                arr[s] = turnLine[s];
            }

            double delta = 0;
            turnLine?.Clear();

            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    turnLine.Add(arr[i]);
                    continue;
                }
                delta += (arr[i - 1].heading - arr[i].heading);
                if (Math.Abs(delta) > 0.005)
                {
                    turnLine.Add(arr[i]);
                    delta = 0;
                }
            }
        }

        private void ReverseWinding()
        {
            //reverse the boundary
            int cnt = fenceLine.Count;
            vec3[] arr = new vec3[cnt];
            cnt--;
            fenceLine.CopyTo(arr);
            fenceLine.Clear();
            for (int i = cnt; i >= 0; i--)
            {
                arr[i].heading -= Math.PI;
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                fenceLine.Add(arr[i]);
            }
        }

        private bool CalculateFenceArea(int idx)
        {
            int ptCount = fenceLine.Count;
            if (ptCount < 1) return false;

            area = 0;         // Accumulates area in the loop
            int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

            for (int i = 0; i < ptCount; j = i++)
            {
                area += (fenceLine[j].easting + fenceLine[i].easting) * (fenceLine[j].northing - fenceLine[i].northing);
            }

            bool isClockwise = area >= 0;

            area = Math.Abs(area / 2);

            //make sure is clockwise for outer counter clockwise for inner
            if ((idx == 0) != isClockwise)
            {
                ReverseWinding();
            }

            return isClockwise;
        }
    }
}