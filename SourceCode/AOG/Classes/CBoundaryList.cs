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

        //fence functions
        private void CalculateFenceLineHeadings()
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = fenceLine.Count;
            vec3[] arr = new vec3[cnt];
            cnt--;
            fenceLine.CopyTo(arr);
            fenceLine.Clear();

            //first point needs last, first, second points
            vec3 pt3 = arr[0];
            pt3.heading = Math.Atan2(arr[1].easting - arr[cnt].easting, arr[1].northing - arr[cnt].northing);
            if (pt3.heading < 0) pt3.heading += glm.twoPI;
            fenceLine.Add(pt3);

            //middle points
            for (int i = 1; i < cnt; i++)
            {
                pt3 = arr[i];
                pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                fenceLine.Add(pt3);
            }

            //last and first point
            pt3 = arr[cnt];
            pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 1].easting, arr[0].northing - arr[cnt - 1].northing);
            if (pt3.heading < 0) pt3.heading += glm.twoPI;
            fenceLine.Add(pt3);
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
            CalculateFenceLineHeadings();

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
            //to fill the list of line points
            vec3 point = new vec3();

            //determine how wide a headland space
            double totalHeadWidth = Settings.Vehicle.set_youTurnDistanceFromBoundary;

            //inside boundaries
            turnLine.Clear();

            int ptCount = fenceLine.Count;

            for (int i = ptCount - 1; i >= 0; i--)
            {
                //calculate the point outside the boundary
                point.easting = fenceLine[i].easting + (-Math.Sin(glm.PIBy2 + fenceLine[i].heading) * totalHeadWidth);
                point.northing = fenceLine[i].northing + (-Math.Cos(glm.PIBy2 + fenceLine[i].heading) * totalHeadWidth);
                point.heading = fenceLine[i].heading;
                if (point.heading < -glm.twoPI) point.heading += glm.twoPI;

                //only add if outside actual field boundary
                if (idx == 0 == fenceLineEar.IsPointInPolygon(point))
                {
                    vec3 tPnt = new vec3(point.easting, point.northing, point.heading);
                    turnLine.Add(tPnt);
                }
            }
            FixTurnLine(totalHeadWidth, 2);

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

            if (turnLine.Count > 0)
            {
                vec3 end = new vec3(turnLine[0].easting,
                    turnLine[0].northing, turnLine[0].heading);
                turnLine.Add(end);
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
            if ((idx == 0 && isClockwise) || (idx > 0 && !isClockwise))
            {
                ReverseWinding();
            }

            return isClockwise;
        }

        //Turn Functions
        private void CalculateTurnHeadings()
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = turnLine.Count;
            vec3[] arr = new vec3[cnt];
            cnt--;
            turnLine.CopyTo(arr);
            turnLine.Clear();

            //first point needs last, first, second points
            vec3 pt3 = arr[0];
            pt3.heading = Math.Atan2(arr[1].easting - arr[cnt].easting, arr[1].northing - arr[cnt].northing);
            if (pt3.heading < 0) pt3.heading += glm.twoPI;
            turnLine.Add(pt3);

            //middle points
            for (int i = 1; i < cnt; i++)
            {
                pt3 = arr[i];
                pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                turnLine.Add(pt3);
            }

            //first point
            vec3 pt2 = arr[0];
            pt2.heading = Math.Atan2(arr[1].easting - arr[0].easting, arr[1].northing - arr[0].northing);
            if (pt2.heading < 0) pt2.heading += glm.twoPI;
            turnLine.Insert(0, new vec3(pt2));

            //last point
            pt2 = arr[arr.Length - 1];
            pt2.heading = Math.Atan2(arr[arr.Length - 1].easting - arr[arr.Length - 2].easting,
                arr[arr.Length - 1].northing - arr[arr.Length - 2].northing);
            if (pt2.heading < 0) pt2.heading += glm.twoPI;
            turnLine.Add(new vec3(pt2));
        }

        private void FixTurnLine(double totalHeadWidth, double spacing)
        {
            //countExit the points from the boundary
            int lineCount = turnLine.Count;

            totalHeadWidth *= totalHeadWidth;
            spacing *= spacing;

            //int headCount = mf.bndArr[inTurnNum].bndLine.Count;
            double distance;

            //remove the points too close to boundary
            for (int i = 0; i < fenceLine.Count; i++)
            {
                for (int j = 0; j < lineCount; j++)
                {
                    //make sure distance between headland and boundary is not less then width
                    distance = glm.DistanceSquared(fenceLine[i], turnLine[j]);
                    if (distance < (totalHeadWidth * 0.99))
                    {
                        turnLine.RemoveAt(j);
                        lineCount = turnLine.Count;
                        j = -1;
                    }
                }
            }

            //make sure distance isn't too big between points on Turn
            int bndCount = turnLine.Count;
            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;
                if (j == bndCount) j = 0;
                distance = glm.DistanceSquared(turnLine[i], turnLine[j]);
                if (distance > (spacing * 1.8))
                {
                    vec3 pointB = new vec3((turnLine[i].easting + turnLine[j].easting) / 2.0, (turnLine[i].northing + turnLine[j].northing) / 2.0, turnLine[i].heading);

                    turnLine.Insert(j, pointB);
                    bndCount = turnLine.Count;
                    i--;
                }
            }

            //make sure distance isn't too small between points on turnLine
            bndCount = turnLine.Count;
            for (int i = 0; i < bndCount - 1; i++)
            {
                distance = glm.DistanceSquared(turnLine[i], turnLine[i + 1]);
                if (distance < spacing)
                {
                    turnLine.RemoveAt(i + 1);
                    bndCount = turnLine.Count;
                    i--;
                }
            }

            //make sure headings are correct for calculated points
            if (turnLine.Count > 0)
            {
                CalculateTurnHeadings();
            }
        }
    }
}