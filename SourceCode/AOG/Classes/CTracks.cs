using AgOpenGPS.Classes;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgOpenGPS
{
    public enum TrackMode
    { None = 0, AB = 2, Curve = 4, bndTrackOuter = 8, bndTrackInner = 16, bndCurve = 32, waterPivot = 64 };//, Heading, Circle, Spiral

    public class CTracks
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        public List<CTrk> gArr = new List<CTrk>();
        private CTrk _currTrk;

        public bool isHeadingSameWay = true, lastIsHeadingSameWay = true;

        public double howManyPathsAway, lastHowManyPathsAway;

        public bool isSmoothWindowOpen;

        public List<vec3> smooList = new List<vec3>();

        //the list of points of curve to drive on
        public List<vec3> currentGuidanceTrack = new List<vec3>();

        //guidelines
        public List<List<vec3>> guideArr = new List<List<vec3>>();

        private bool isBusyWorking = false;

        public bool isTrackValid;

        public double lastSecond = 0;

        //design a new track
        public List<vec3> designPtsList = new List<vec3>();

        public vec2 designPtA = new vec2(0.2, 0.15);
        public vec2 designPtB = new vec2(0.3, 0.3);
        public vec2 designLineEndA = new vec2(0.2, 0.15);
        public vec2 designLineEndB = new vec2(0.3, 0.3);

        public double designHeading = 0;

        //flag for starting stop adding points for curve
        public bool isMakingCurveTrack, isRecordingCurveTrack;

        //to fake the user into thinking they are making a line - but is a curve
        public bool isMakingABLine;

        public CTracks(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public CTrk currTrk
        {
            get => _currTrk;
            set
            {
                if (_currTrk != value)
                {
                    _currTrk = value;

                    isTrackValid = false;

                    mf.SetAutoSteerButton(false, _currTrk == null ? gStr.Get(gs.gsNoABLineActive) : "Track Changed");

                    //mf.SetYouTurnButton(false);
                    //ss Log.EventWriter("Autosteer Stop, No Tracks Available");

                    int index2 = gArr.FindIndex(x => x == _currTrk);
                    mf.lblNumCu.Text = (index2 + 1).ToString() + "/" + gArr.Count.ToString();
                    mf.lblNumCu.Visible = !mf.ct.isContourBtnOn;
                }
            }
        }

        public int GetVisibleTracks()
        {
            int tracksVisible = 0;
            foreach (var track in gArr)
            {
                if (track.isVisible) tracksVisible++;
            }
            return tracksVisible;
        }
        public CTrk GetNextTrack(CTrk track, List<CTrk> gTemp, bool next = true, bool invisible = false)
        {
            int index = gTemp.FindIndex(x => x == track);

            if (next)
                return gTemp.Skip(index + 1).Concat(gTemp.Take(index)).FirstOrDefault(x => x.isVisible || invisible);
            else
                return gTemp.Take(index).Reverse().Concat(gTemp.Skip(index + 1).Reverse()).FirstOrDefault(x => x.isVisible || invisible);
        }

        public void GetNextTrack(bool next = true)
        {
            int index = gArr.FindIndex(x => x == currTrk);

            if (next)
                currTrk = gArr.Skip(index + 1).Concat(gArr.Take(index)).FirstOrDefault(x => x.isVisible);
            else
                currTrk = gArr.Take(index).Reverse().Concat(gArr.Skip(index + 1).Reverse()).FirstOrDefault(x => x.isVisible);
        }

        public async void GetDistanceFromRefTrack(CTrk track, vec3 pivot)
        {
            double widthMinusOverlap = Settings.Tool.toolWidth - Settings.Tool.overlap;

            if (!isTrackValid || ((mf.secondsSinceStart - lastSecond) > 3 && (!mf.isBtnAutoSteerOn || mf.mc.steerSwitchHigh)))
            {
                double distanceFromRefLine = 0;
                lastSecond = mf.secondsSinceStart;
                if (track.mode != TrackMode.waterPivot)
                {
                    int refCount = track.curvePts.Count;
                    if (refCount < 2)
                    {
                        currentGuidanceTrack?.Clear();
                        return;
                    }

                    //int cc = mf.gyd.FindGlobalRoughNearest(mf.guidanceLookPos, track.curvePts, 10, !isTrackValid);

                    if (mf.gyd.FindClosestSegment(track.curvePts, false, mf.guidanceLookPos, out int rA, out int rB))//, cc - 10, cc + 10))
                    {
                        distanceFromRefLine = mf.gyd.FindDistanceToSegment(mf.guidanceLookPos, track.curvePts[rA], track.curvePts[rB], out vec3 point, out _, true, false, false);

                        //same way as line creation or not
                        isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - track.curvePts[rA].heading) - Math.PI) < glm.PIBy2;
                    }
                }
                else //pivot guide list
                {
                    //cross product
                    isHeadingSameWay = ((mf.pivotAxlePos.easting - track.ptA.easting) * (mf.steerAxlePos.northing - track.ptA.northing)
                        - (mf.pivotAxlePos.northing - track.ptA.northing) * (mf.steerAxlePos.easting - track.ptA.easting)) < 0;

                    //pivot circle center
                    distanceFromRefLine = -glm.Distance(mf.guidanceLookPos, track.ptA);
                }

                distanceFromRefLine -= (0.5 * widthMinusOverlap);

                double RefDist = (distanceFromRefLine + (isHeadingSameWay ? Settings.Tool.offset : -Settings.Tool.offset) - track.nudgeDistance) / widthMinusOverlap;

                if (RefDist < 0) howManyPathsAway = (int)(RefDist - 0.5);
                else howManyPathsAway = (int)(RefDist + 0.5);
            }

            if (!isTrackValid || howManyPathsAway != lastHowManyPathsAway || (isHeadingSameWay != lastIsHeadingSameWay && Settings.Tool.offset != 0))
            {
                if (!isBusyWorking)
                {
                    try
                    {
                        //is boundary curve - use task
                        isBusyWorking = true;
                        isTrackValid = true;
                        lastHowManyPathsAway = howManyPathsAway;
                        lastIsHeadingSameWay = isHeadingSameWay;
                        double distAway = widthMinusOverlap * howManyPathsAway + (isHeadingSameWay ? -Settings.Tool.offset : Settings.Tool.offset) + track.nudgeDistance;

                        distAway += (0.5 * widthMinusOverlap);

                        currentGuidanceTrack = await Task.Run(() => BuildCurrentGuidanceTrack(distAway, track));

                        if (!mf.yt.isYouTurnTriggered)
                        {
                            mf.yt.ResetCreatedYouTurn();
                        }

                        isBusyWorking = false;
                        mf.gyd.isFindGlobalNearestTrackPoint = true;

                        guideArr?.Clear();
                        if (Settings.User.isSideGuideLines && mf.camera.camSetDistance > Settings.Tool.toolWidth * -400)
                        {
                            //build the list list of guide lines
                            guideArr = await Task.Run(() => BuildTrackGuidelines(distAway, Settings.Vehicle.setAS_numGuideLines, track));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.EventWriter("BuildGuidanceCatch: " + ex.ToString());
                    }
                }
            }
        }

        public List<vec3> BuildCurrentGuidanceTrack(double distAway, CTrk track)
        {
            //the list of points of curve new list from async
            List<vec3> newCurList = new List<vec3>();

            try
            {
                if (track.mode == TrackMode.waterPivot)
                {
                    //max 2 cm offset from correct circle or limit to 500 points
                    double Angle = glm.twoPI / Math.Min(Math.Max(Math.Ceiling(glm.twoPI / (2 * Math.Acos(1 - (0.02 / Math.Abs(distAway))))), 100), 1000);//limit between 50 and 500 points

                    vec3 centerPos = new vec3(track.ptA.easting, track.ptA.northing, 0);
                    double rotation = 0;

                    while (rotation < glm.twoPI)
                    {
                        //Update the heading
                        rotation += Angle;
                        //Add the new coordinate to the path
                        newCurList.Add(new vec3(centerPos.easting + distAway * Math.Sin(rotation), centerPos.northing + distAway * Math.Cos(rotation), 0));
                    }

                    if (newCurList.Count > 1)
                    {
                        vec3[] arr = new vec3[newCurList.Count];
                        newCurList.CopyTo(arr);
                        newCurList.Clear();

                        for (int i = 1; i < (arr.Length - 1); i++)
                        {
                            arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i-1].easting, arr[i + 1].northing - arr[i-1].northing);
                            if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                            if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                        }

                        arr[arr.Length - 1].heading = Math.Atan2(arr[0].easting - arr[arr.Length - 1].easting, arr[0].northing - arr[arr.Length - 1].northing);
                        if (arr[arr.Length - 1].heading < 0) arr[arr.Length - 1].heading += glm.twoPI;
                        if (arr[arr.Length - 1].heading >= glm.twoPI) arr[arr.Length - 1].heading -= glm.twoPI;

                        arr[0].heading = Math.Atan2(arr[ 1].easting - arr[arr.Length - 1].easting, arr[1].northing - arr[arr.Length - 1].northing);
                        if (arr[0].heading < 0) arr[0].heading += glm.twoPI;
                        if (arr[0].heading >= glm.twoPI) arr[0].heading -= glm.twoPI;

                        newCurList.AddRange(arr);
                    }
                }
                else
                {
                    vec3 point;

                    double step = (Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.4;
                    if (step > 2) step = 2;
                    if (step < 0.5) step = 0.5;

                    if (track.mode == TrackMode.AB)
                    {
                        for (int i = 0; i < track.curvePts.Count; i++)
                        {
                            point = new vec3(
                                track.curvePts[i].easting + (Math.Sin(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                                track.curvePts[i].northing + (Math.Cos(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                                track.curvePts[i].heading);

                            newCurList.Add(point);
                        }
                    }
                    else
                    {
                        double distSqAway = (distAway * distAway) - 0.01;

                        int refCount = track.curvePts.Count;
                        for (int i = 0; i < refCount; i++)
                        {
                            point = new vec3(
                            track.curvePts[i].easting + (Math.Sin(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                            track.curvePts[i].northing + (Math.Cos(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                            track.curvePts[i].heading);
                            bool Add = true;

                            for (int t = 0; t < refCount; t++)
                            {
                                double dist = ((point.easting - track.curvePts[t].easting) * (point.easting - track.curvePts[t].easting))
                                    + ((point.northing - track.curvePts[t].northing) * (point.northing - track.curvePts[t].northing));
                                if (dist < distSqAway)
                                {
                                    Add = false;
                                    break;
                                }
                            }

                            if (Add)
                            {
                                if (newCurList.Count > 0)
                                {
                                    double dist = ((point.easting - newCurList[newCurList.Count - 1].easting) * (point.easting - newCurList[newCurList.Count - 1].easting))
                                        + ((point.northing - newCurList[newCurList.Count - 1].northing) * (point.northing - newCurList[newCurList.Count - 1].northing));
                                    if (dist > step)
                                        newCurList.Add(point);
                                }
                                else newCurList.Add(point);
                            }
                        }

                        int cnt = newCurList.Count;
                        if (cnt > 6)
                        {
                            vec3[] arr = new vec3[cnt];
                            newCurList.CopyTo(arr);

                            newCurList.Clear();

                            for (int i = 0; i < (arr.Length - 1); i++)
                            {
                                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                                if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                            }

                            arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;

                            cnt = arr.Length;
                            double distance;

                            //add the first point of loop - it will be p1
                            newCurList.Add(arr[0]);

                            for (int i = 0; i < cnt - 3; i++)
                            {
                                // add p1
                                newCurList.Add(arr[i + 1]);

                                distance = glm.Distance(arr[i + 1], arr[i + 2]);

                                if (distance > step)
                                {
                                    int loopTimes = (int)(distance / step + 1);
                                    for (int j = 1; j < loopTimes; j++)
                                    {
                                        vec3 pos = new vec3(glm.Catmull(j / (double)(loopTimes), arr[i], arr[i + 1], arr[i + 2], arr[i + 3]));
                                        newCurList.Add(pos);
                                    }
                                }
                            }

                            newCurList.Add(arr[cnt - 2]);
                            newCurList.Add(arr[cnt - 1]);

                            //to calc heading based on next and previous points to give an average heading.
                            cnt = newCurList.Count;
                            arr = new vec3[cnt];
                            cnt--;
                            newCurList.CopyTo(arr);
                            newCurList.Clear();

                            newCurList.Add(new vec3(arr[0]));

                            //middle points
                            for (int i = 1; i < cnt; i++)
                            {
                                vec3 pt3 = new vec3(arr[i])
                                {
                                    heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing)
                                };
                                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                                newCurList.Add(pt3);
                            }

                            int k = arr.Length - 1;
                            vec3 pt33 = new vec3(arr[k])
                            {
                                heading = Math.Atan2(arr[k].easting - arr[k - 1].easting, arr[k].northing - arr[k - 1].northing)
                            };
                            if (pt33.heading < 0) pt33.heading += glm.twoPI;
                            newCurList.Add(pt33);
                        }
                    }
                    if (mf.bnd.bndList.Count > 0 && (track.mode == TrackMode.AB || track.mode == TrackMode.Curve))
                    {
                        int ptCnt = newCurList.Count - 1;
                        int iStep = (int)step;

                        bool isAdding = false;
                        //end
                        while (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(newCurList[newCurList.Count - 1]))
                        {
                            isAdding = true;
                            for (int i = 1; i < 10; i++)
                            {
                                vec3 pt = new vec3(newCurList[ptCnt]);
                                pt.easting += (Math.Sin(pt.heading) * i * iStep);
                                pt.northing += (Math.Cos(pt.heading) * i * iStep);
                                newCurList.Add(pt);
                            }
                            ptCnt = newCurList.Count - 1;
                        }

                        if (isAdding)
                        {
                            vec3 pt = new vec3(newCurList[newCurList.Count - 1]);
                            for (int i = 1; i < 5; i++)
                            {
                                pt.easting += (Math.Sin(pt.heading) * iStep);
                                pt.northing += (Math.Cos(pt.heading) * iStep);
                                newCurList.Add(pt);
                            }
                        }

                        isAdding = false;

                        //and the beginning
                        vec3 pt3 = new vec3(newCurList[0]);

                        while (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(newCurList[0]))
                        {
                            isAdding = true;
                            pt3 = new vec3(newCurList[0]);

                            for (int i = 1; i < 10; i++)
                            {
                                vec3 pt = new vec3(pt3);
                                pt.easting -= (Math.Sin(pt.heading) * i * iStep);
                                pt.northing -= (Math.Cos(pt.heading) * i * iStep);
                                newCurList.Insert(0, pt);
                            }
                        }

                        if (isAdding)
                        {
                            vec3 pt = new vec3(newCurList[0]);
                            for (int i = 1; i < 5; i++)
                            {
                                pt.easting -= (Math.Sin(pt.heading) * iStep);
                                pt.northing -= (Math.Cos(pt.heading) * iStep);
                                newCurList.Insert(0, pt);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.EventWriter("Exception Build new offset curve" + e.ToString());
            }

            return newCurList;
        }

        private List<List<vec3>> BuildTrackGuidelines(double distAway, int _passes, CTrk track)
        {
            // the listlist of all the guidelines
            List<List<vec3>> newGuideLL = new List<List<vec3>>();

            try
            {
                for (int numGuides = -_passes; numGuides <= _passes; numGuides++)
                {
                    if (numGuides == 0) continue;

                    //the list of points of curve new list from async
                    List<vec3> newGuideList = new List<vec3>
                    {
                        Capacity = 128
                    };

                    newGuideLL.Add(newGuideList);

                    double nextGuideDist = (Settings.Tool.toolWidth - Settings.Tool.overlap) * numGuides +
                        (isHeadingSameWay ? -Settings.Tool.offset : Settings.Tool.offset) ;

                    //nextGuideDist += (0.5 * (Settings.Tool.toolWidth - Settings.Tool.overlap));

                    nextGuideDist += distAway;

                    vec3 point;

                    double step = (Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.48;
                    if (step > 4) step = 4;
                    if (step < 1) step = 1;

                    double distSqAway = (nextGuideDist * nextGuideDist) - 0.01;

                    int refCount = track.curvePts.Count;
                    for (int i = 0; i < refCount; i++)
                    {
                        point = new vec3(
                        track.curvePts[i].easting + (Math.Sin(glm.PIBy2 + track.curvePts[i].heading) * nextGuideDist),
                        track.curvePts[i].northing + (Math.Cos(glm.PIBy2 + track.curvePts[i].heading) * nextGuideDist),
                        track.curvePts[i].heading);
                        bool Add = true;

                        for (int t = 0; t < refCount; t++)
                        {
                            double dist = ((point.easting - track.curvePts[t].easting) * (point.easting - track.curvePts[t].easting))
                                + ((point.northing - track.curvePts[t].northing) * (point.northing - track.curvePts[t].northing));
                            if (dist < distSqAway)
                            {
                                Add = false;
                                break;
                            }
                        }

                        if (Add)
                        {
                            if (newGuideList.Count > 0)
                            {
                                double dist = ((point.easting - newGuideList[newGuideList.Count - 1].easting) * (point.easting - newGuideList[newGuideList.Count - 1].easting))
                                    + ((point.northing - newGuideList[newGuideList.Count - 1].northing) * (point.northing - newGuideList[newGuideList.Count - 1].northing));
                                if (dist > step)
                                {
                                    if (mf.bnd.bndList.Count > 0)
                                    {
                                        if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                                        {
                                            newGuideList.Add(point);
                                        }
                                    }
                                    else
                                    {
                                        newGuideList.Add(point);
                                    }
                                }
                            }
                            else
                            {
                                if (mf.bnd.bndList.Count > 0)
                                {
                                    if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                                    {
                                        newGuideList.Add(point);
                                    }
                                }
                                else
                                {
                                    newGuideList.Add(point);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.EventWriter("Exception Build new offset curve" + e.ToString());
            }

            return newGuideLL;
        }

        public void DrawNewTrack()
        {
            if (designPtsList.Count > 0)
            {
                GL.Color3(0.95f, 0.42f, 0.750f);
                GL.LineWidth(4.0f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < designPtsList.Count; h++) GL.Vertex3(designPtsList[h].easting, designPtsList[h].northing, 0);
                GL.End();

                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0F00);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(0.99f, 0.99f, 0.0);
                GL.Vertex3(designPtsList[designPtsList.Count - 1].easting, designPtsList[designPtsList.Count - 1].northing, 0);
                GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
                GL.End();

                GL.Disable(EnableCap.LineStipple);
            }
        }

        public void DrawTrack()
        {
            if (guideArr.Count > 0)
            {
                GL.LineWidth(Settings.User.setDisplay_lineWidth * 3);
                GL.Color3(0, 0, 0);

                for (int i = 0; i < guideArr.Count; i++)
                {
                    GL.Begin(currTrk.mode != TrackMode.bndCurve ? PrimitiveType.LineStrip : PrimitiveType.LineLoop);

                    for (int h = 0; h < guideArr[i].Count; h++)
                        GL.Vertex3(guideArr[i][h].easting, guideArr[i][h].northing, 0);
                    GL.End();
                }

                GL.LineWidth(Settings.User.setDisplay_lineWidth);
                GL.Color4(0.2, 0.75, 0.2, 0.6);

                for (int i = 0; i < guideArr.Count; i++)
                {
                    GL.Begin(currTrk.mode != TrackMode.bndCurve ? PrimitiveType.LineStrip : PrimitiveType.LineLoop);
                    for (int h = 0; h < guideArr[i].Count; h++)
                        GL.Vertex3(guideArr[i][h].easting, guideArr[i][h].northing, 0);
                    GL.End();
                }
            }

            //draw reference line
            if (currTrk.mode != TrackMode.waterPivot)
            {
                if (currTrk.curvePts == null || currTrk.curvePts.Count == 0) return;

                GL.LineWidth(Settings.User.setDisplay_lineWidth * 2);
                GL.Color3(0.96, 0.2f, 0.2f);
                GL.Begin(PrimitiveType.Lines);

                for (int h = 0; h < currTrk.curvePts.Count; h++) GL.Vertex3(
                    currTrk.curvePts[h].easting,
                    currTrk.curvePts[h].northing,
                    0);

                GL.End();

                if (mf.font.isFontOn)
                {
                    GL.Color3(0.40f, 0.90f, 0.95f);
                    mf.font.DrawText3D(currTrk.ptA.easting, currTrk.ptA.northing, "&A");
                    mf.font.DrawText3D(currTrk.ptB.easting, currTrk.ptB.northing, "&B");
                }

                //just draw ref and smoothed line if smoothing window is open
                if (isSmoothWindowOpen)
                {
                    if (smooList.Count == 0) return;

                    GL.LineWidth(Settings.User.setDisplay_lineWidth);
                    GL.Color3(0.930f, 0.92f, 0.260f);
                    GL.Begin(PrimitiveType.Lines);
                    for (int h = 0; h < smooList.Count; h++) GL.Vertex3(smooList[h].easting, smooList[h].northing, 0);
                    GL.End();
                }
            }

            //Draw Tracks
            if (currentGuidanceTrack.Count > 0 && !isSmoothWindowOpen) //normal. Smoothing window is not open.
            {
                GL.LineWidth(Settings.User.setDisplay_lineWidth * 4);
                GL.Color3(0, 0, 0);

                //ablines and curves are a line - the rest a loop
                if (currTrk.mode <= TrackMode.Curve)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                }
                else
                {
                    if (currTrk.mode == TrackMode.waterPivot)
                    {
                        GL.PointSize(15.0f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Vertex3(currTrk.ptA.easting, currTrk.ptA.northing, 0);
                        GL.End();
                    }

                    GL.Begin(PrimitiveType.LineLoop);
                }

                for (int h = 0; h < currentGuidanceTrack.Count; h++) GL.Vertex3(currentGuidanceTrack[h].easting, currentGuidanceTrack[h].northing, 0);
                GL.End();

                GL.LineWidth(Settings.User.setDisplay_lineWidth);
                GL.Color3(0.95f, 0.2f, 0.95f);

                //ablines and curves are a track - the rest a loop
                if (currTrk.mode <= TrackMode.Curve)
                {
                    GL.Begin(PrimitiveType.Lines);
                }
                else
                {
                    if (currTrk.mode == TrackMode.waterPivot)
                    {
                        GL.PointSize(15.0f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Vertex3(currTrk.ptA.easting, currTrk.ptA.northing, 0);
                        GL.End();
                    }

                    GL.Begin(PrimitiveType.LineLoop);
                }

                for (int h = 0; h < currentGuidanceTrack.Count; h++) GL.Vertex3(currentGuidanceTrack[h].easting, currentGuidanceTrack[h].northing, 0);
                GL.End();

                mf.yt.DrawYouTurn();

                /*

                //GL.Disable(EnableCap.LineSmooth);

                //GL.PointSize(12.0f);
                //GL.Begin(PrimitiveType.Points);
                //GL.Color3(0.920f, 0.6f, 0.30f);
                ////for (int h = 0; h < currentGuidanceTrack.Count; h++) GL.Vertex3(currentGuidanceTrack[h].easting, currentGuidanceTrack[h].northing, 0);
                //GL.Vertex3(currentGuidanceTrack[mf.gyd.A].easting, currentGuidanceTrack[mf.gyd.A].northing, 0);
                //GL.End();

                //GL.Begin(PrimitiveType.Points);
                //GL.Color3(0.20f, 0.4f, 0.930f);
                ////for (int h = 0; h < currentGuidanceTrack.Count; h++) GL.Vertex3(currentGuidanceTrack[h].easting, currentGuidanceTrack[h].northing, 0);
                //GL.Vertex3(currentGuidanceTrack[mf.gyd.B].easting, currentGuidanceTrack[mf.gyd.B].northing, 0);
                //GL.End();
                */
            }
        }

        public void DrawABLineNew()
        {
            //AB Line currently being designed
            GL.LineWidth(Settings.User.setDisplay_lineWidth);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(0.95f, 0.70f, 0.50f);
            GL.Vertex3(designLineEndA.easting, designLineEndA.northing, 0.0);
            GL.Vertex3(designLineEndB.easting, designLineEndB.northing, 0.0);
            GL.End();

            GL.Color3(0.2f, 0.950f, 0.20f);
            mf.font.DrawText3D(designPtA.easting, designPtA.northing, "&A");
            mf.font.DrawText3D(designPtB.easting, designPtB.northing, "&B");
        }

        //for calculating for display the averaged new line
        public void CalculateHeadings(ref List<vec3> xList)
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = xList.Count;
            if (cnt > 3)
            {
                vec3[] arr = new vec3[cnt];
                cnt--;
                xList.CopyTo(arr);
                xList.Clear();

                vec3 pt3 = arr[0];
                pt3.heading = Math.Atan2(arr[1].easting - arr[0].easting, arr[1].northing - arr[0].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                xList.Add(pt3);

                //middle points
                for (int i = 1; i < cnt; i++)
                {
                    pt3 = arr[i];
                    pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                    if (pt3.heading < 0) pt3.heading += glm.twoPI;
                    xList.Add(pt3);
                }

                pt3 = arr[arr.Length - 1];
                pt3.heading = Math.Atan2(arr[arr.Length - 1].easting - arr[arr.Length - 2].easting,
                    arr[arr.Length - 1].northing - arr[arr.Length - 2].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                xList.Add(pt3);
            }
        }

        public void MakePointMinimumSpacing(ref List<vec3> xList, double minDistance)
        {
            int cnt = xList.Count;
            if (cnt > 3)
            {
                //make sure point distance isn't too big
                for (int i = 0; i < cnt - 1; i++)
                {
                    int j = i + 1;
                    //if (j == cnt) j = 0;
                    double distance = glm.Distance(xList[i], xList[j]);
                    if (distance > minDistance)
                    {
                        vec3 pointB = new vec3((xList[i].easting + xList[j].easting) / 2.0,
                            (xList[i].northing + xList[j].northing) / 2.0,
                            xList[i].heading);

                        xList.Insert(j, pointB);
                        cnt = xList.Count;
                        i = -1;
                    }
                }
            }
        }

        //turning the visual line into the real reference line to use
        public void SaveSmoothList(CTrk track)
        {
            if (smooList.Count > 3)
            {
                CalculateHeadings(ref smooList);
                track.curvePts = smooList;
                smooList = new List<vec3>();
            }
        }

        public void SmoothAB(ref List<vec3> points, int smPts, bool setSmoothList = true)
        {
            //countExit the reference list of original curve
            int cnt = points.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2 && s < cnt; s++)
            {
                arr[s].easting = points[s].easting;
                arr[s].northing = points[s].northing;
                arr[s].heading = points[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = points[s].easting;
                arr[s].northing = points[s].northing;
                arr[s].heading = points[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += points[j + i].easting;
                    arr[i].northing += points[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = points[i].heading;
            }

            if (setSmoothList)
                smooList = arr.ToList();
            else
                points = arr.ToList();
        }

        public void CreateDesignedABTrack(bool isRefRightSide)
        {
            var track = new CTrk(TrackMode.AB);

            double hsin = Math.Sin(designHeading);
            double hcos = Math.Cos(designHeading);

            //fill in the dots between A and B
            double len = glm.Distance(designPtA, designPtB);
            if (len < 20)
            {
                designPtB.easting = designPtA.easting + (Math.Sin(designHeading) * 30);
                designPtB.northing = designPtA.northing + (Math.Cos(designHeading) * 30);
            }
            len = glm.Distance(designPtA, designPtB);

            vec3 P1 = new vec3();
            for (int i = 0; i < (int)len; i += 1)
            {
                P1.easting = (hsin * i) + designPtA.easting;
                P1.northing = (hcos * i) + designPtA.northing;
                P1.heading = designHeading;
                track.curvePts.Add(P1);
            }

            track.heading = designHeading;

            double dist = (Settings.Tool.toolWidth - Settings.Tool.overlap) * (isRefRightSide ? 0.5 : -0.5) + Settings.Tool.offset;
            NudgeRefTrack(track, dist);

            track.ptA = new vec2(track.curvePts[0]);
            track.ptB = new vec2(track.curvePts[track.curvePts.Count - 1]);

            //build the tail extensions
            AddFirstLastPoints(ref track.curvePts, 100);

            gArr.Add(track);
            currTrk = track;
        }

        public void AddFirstLastPoints(ref List<vec3> xList, int ptsToAdd)
        {
            int ptCnt = xList.Count - 1;
            vec3 start;
            ptsToAdd *= 2;

            for (int i = 1; i < ptsToAdd; i += 2)
            {
                vec3 pt = new vec3(xList[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                xList.Add(pt);
            }

            //and the beginning
            start = new vec3(xList[0]);

            for (int i = 1; i < ptsToAdd; i += 2)
            {
                vec3 pt = new vec3(start);
                pt.easting -= (Math.Sin(pt.heading) * i);
                pt.northing -= (Math.Cos(pt.heading) * i);
                xList.Insert(0, pt);
            }
        }

        public void AddStartPoints(ref List<vec3> xList, int ptsToAdd)
        {
            vec3 start;
            ptsToAdd *= 2;

            start = new vec3(xList[0]);

            for (int i = 1; i < ptsToAdd; i += 2)
            {
                vec3 pt = new vec3(start);
                pt.easting -= (Math.Sin(pt.heading) * i);
                pt.northing -= (Math.Cos(pt.heading) * i);
                xList.Insert(0, pt);
            }
        }

        public void AddEndPoints(ref List<vec3> xList, int ptsToAdd)
        {
            int ptCnt = xList.Count - 1;
            ptsToAdd *= 2;

            for (int i = 1; i < ptsToAdd; i += 2)
            {
                vec3 pt = new vec3(xList[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                xList.Add(pt);
            }
        }

        public void NudgeTrack(CTrk track, double dist)
        {
            isTrackValid = false;
            if (track != null)
                track.nudgeDistance += isHeadingSameWay ? dist : -dist;
        }

        public void NudgeDistanceReset(CTrk track)
        {
            isTrackValid = false;
            track.nudgeDistance = 0;
        }

        public void SnapToPivot(CTrk track)
        {
            NudgeTrack(track, mf.gyd.distanceFromCurrentLine);
        }

        public void NudgeRefTrack(CTrk track, double distAway)
        {
            isTrackValid = false;

            List<vec3> curList = new List<vec3>();

            if (track.mode != TrackMode.AB)
            {
                double distSqAway = (distAway * distAway) - 0.01;
                vec3 point;

                for (int i = 0; i < track.curvePts.Count; i++)
                {
                    point = new vec3(
                    track.curvePts[i].easting + (Math.Sin(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                    track.curvePts[i].northing + (Math.Cos(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                    track.curvePts[i].heading);
                    bool Add = true;

                    for (int t = 0; t < track.curvePts.Count; t++)
                    {
                        double dist = ((point.easting - track.curvePts[t].easting) * (point.easting - track.curvePts[t].easting))
                            + ((point.northing - track.curvePts[t].northing) * (point.northing - track.curvePts[t].northing));
                        if (dist < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }

                    if (Add)
                    {
                        if (curList.Count > 0)
                        {
                            double dist = ((point.easting - curList[curList.Count - 1].easting) * (point.easting - curList[curList.Count - 1].easting))
                                + ((point.northing - curList[curList.Count - 1].northing) * (point.northing - curList[curList.Count - 1].northing));
                            if (dist > 1.0)
                                curList.Add(point);
                        }
                        else curList.Add(point);
                    }
                }

                int cnt = curList.Count;
                if (cnt > 6)
                {
                    vec3[] arr = new vec3[cnt];
                    curList.CopyTo(arr);

                    curList.Clear();

                    for (int i = 0; i < (arr.Length - 1); i++)
                    {
                        arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                        if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                        if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                    }

                    arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;

                    //replace the array
                    cnt = arr.Length;
                    double distance;
                    double spacing = 2;

                    //add the first point of loop - it will be p1
                    curList.Add(arr[0]);

                    for (int i = 0; i < cnt - 3; i++)
                    {
                        // add p2
                        curList.Add(arr[i + 1]);

                        distance = glm.Distance(arr[i + 1], arr[i + 2]);

                        if (distance > spacing)
                        {
                            int loopTimes = (int)(distance / spacing + 1);
                            for (int j = 1; j < loopTimes; j++)
                            {
                                vec3 pos = new vec3(glm.Catmull(j / (double)(loopTimes), arr[i], arr[i + 1], arr[i + 2], arr[i + 3]));
                                curList.Add(pos);
                            }
                        }
                    }

                    curList.Add(arr[cnt - 2]);
                    curList.Add(arr[cnt - 1]);

                    CalculateHeadings(ref curList);

                    track.curvePts = curList;
                }
            }
            else
            {
                vec3 point;

                //find the A and B points in the ref

                int aClose = 0, bClose = 0;
                double minDist = double.MaxValue;

                for (int i = 0; i < track.curvePts.Count; i++)
                {
                    double dist = glm.DistanceSquared(track.curvePts[i], track.ptA);
                    if (dist < minDist)
                    {
                        aClose = i;
                        minDist = dist;
                    }
                }
                minDist = double.MaxValue;
                for (int i = 0; i < track.curvePts.Count; i++)
                {
                    double dist = glm.DistanceSquared(track.curvePts[i], track.ptB);
                    if (dist < minDist)
                    {
                        bClose = i;
                        minDist = dist;
                    }
                }

                for (int i = 0; i < track.curvePts.Count; i++)
                {
                    point = new vec3(
                        track.curvePts[i].easting + (Math.Sin(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                        track.curvePts[i].northing + (Math.Cos(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                        track.curvePts[i].heading);

                    curList.Add(point);
                }

                track.curvePts.Clear();

                track.curvePts = curList;

                track.ptA.easting = (track.curvePts[aClose].easting);
                track.ptA.northing = (track.curvePts[aClose].northing);
                track.ptB.easting = (track.curvePts[bClose].easting);
                track.ptB.northing = (track.curvePts[bClose].northing);
            }
        }

        public void ResetTrack()
        {
            currentGuidanceTrack?.Clear();
            currTrk = null;
            gArr.Clear();
        }

        public bool PointOnLine(vec3 pt1, vec3 pt2, vec3 pt)
        {
            vec2 r = new vec2(0, 0);
            if (pt1.northing == pt2.northing && pt1.easting == pt2.easting) { pt1.northing -= 0.00001; }

            double U = ((pt.northing - pt1.northing) * (pt2.northing - pt1.northing)) + ((pt.easting - pt1.easting) * (pt2.easting - pt1.easting));

            double Udenom = Math.Pow(pt2.northing - pt1.northing, 2) + Math.Pow(pt2.easting - pt1.easting, 2);

            U /= Udenom;

            r.northing = pt1.northing + (U * (pt2.northing - pt1.northing));
            r.easting = pt1.easting + (U * (pt2.easting - pt1.easting));

            double minx, maxx, miny, maxy;

            minx = Math.Min(pt1.northing, pt2.northing);
            maxx = Math.Max(pt1.northing, pt2.northing);

            miny = Math.Min(pt1.easting, pt2.easting);
            maxy = Math.Max(pt1.easting, pt2.easting);
            return _ = r.northing >= minx && r.northing <= maxx && (r.easting >= miny && r.easting <= maxy);
        }
    }

    public class CTrk
    {
        public List<vec3> curvePts = new List<vec3>();
        public double heading;
        public string name;
        public bool isVisible;
        public vec2 ptA;
        public vec2 ptB;
        public vec2 endPtA;
        public vec2 endPtB;
        public TrackMode mode;
        public double nudgeDistance;

        public CTrk(TrackMode _mode = TrackMode.None)
        {
            curvePts = new List<vec3>();
            heading = 3;
            name = "New Track";
            isVisible = true;
            ptA = new vec2();
            ptB = new vec2();
            endPtA = new vec2();
            endPtB = new vec2();
            mode = _mode;
            nudgeDistance = 0;
        }

        public CTrk(CTrk _trk)
        {
            curvePts = new List<vec3>(_trk.curvePts);
            heading = _trk.heading;
            name = _trk.name;
            isVisible = _trk.isVisible;
            ptA = _trk.ptA;
            ptB = _trk.ptB;
            endPtA = new vec2();
            endPtB = new vec2();
            mode = _trk.mode;
            nudgeDistance = _trk.nudgeDistance;
        }

        public static bool operator ==(CTrk a, CTrk b)
        {
            if (a is null && b is null) return true;
            if (a is null) return false;
            if (b is null) return false;
            return a.name == b.name;
            //if (ReferenceEquals(a, b)) return true;
        }

        public static bool operator !=(CTrk a, CTrk b) => !(a == b);

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            return this == (CTrk)obj;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}