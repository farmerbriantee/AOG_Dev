using AOG.Classes;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOG
{
    public enum TrackMode
    { None = 0, AB = 2, Curve = 4, bndTrackOuter = 8, bndTrackInner = 16, bndCurve = 32, waterPivot = 64 };//, Heading, Circle, Spiral

    public class CTracks
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        public IReadOnlyList<CTrk> gArr => _gArr;

        private List<CTrk> _gArr = new List<CTrk>();

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
        public bool isMakingTrack, isRecordingCurveTrack;

        //to fake the user into thinking they are making a line - but is a curve
        public bool isMakingABLine;

        public CTracks(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public void AddTrack(CTrk track)
        {
            if (track == null) return;

            string name = track.name;

            while (_gArr.Any(t => t.name == name))
                name += " ";

            track.name = name;

            _gArr.Add(track);
        }

        public void SetTracks(List<CTrk> tracks)
        {
            _gArr = tracks;
        }

        public void setTrack(CTrk track)
        {
            int index = _gArr.FindIndex(item => item == track);
            if (index != -1)
            {
                if (track == currTrk)
                    isTrackValid = false;
                _gArr[index] = track;
            }
        }

        public int TrackIndex(CTrk track)
        {
            return _gArr.FindIndex(item => item == track);
        }

        public void MoveTrackUp(CTrk track)
        {
            int index = _gArr.IndexOf(track);
            if (track == null || index == 0)
                return;

            _gArr.Reverse(index - 1, 2);
        }

        public void MoveTrackDn(CTrk track)
        {
            int index = _gArr.IndexOf(track);

            if (track == null || index == (_gArr.Count - 1))
                return;

            _gArr.Reverse(index, 2);
        }

        public void RemoveTrack(CTrk track)
        {
            _gArr.Remove(track);
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

                    int index2 = _gArr.FindIndex(x => x == _currTrk);
                    mf.lblNumCu.Text = (index2 + 1).ToString() + "/" + gArr.Count.ToString();
                    mf.lblNumCu.Visible = !mf.ct.isContourBtnOn;
                    mf.PanelUpdateRightAndBottom();
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
            int index = _gArr.FindIndex(x => x == currTrk);

            if (next)
                currTrk = gArr.Skip(index + 1).Concat(gArr.Take(index)).FirstOrDefault(x => x.isVisible);
            else
                currTrk = gArr.Take(index).Reverse().Concat(gArr.Skip(index + 1).Reverse()).FirstOrDefault(x => x.isVisible);
        }

        public async void GetDistanceFromRefTrack(CTrk track, vec3 pivot)
        {
            double widthMinusOverlap = Settings.Tool.toolWidth - Settings.Tool.overlap;

            if (!isTrackValid || ((mf.secondsSinceStart - lastSecond) > 2 && (!mf.isBtnAutoSteerOn || mf.mc.steerSwitchHigh)))
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
                        isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading 
                            + glm.toRadians(mf.mc.actualSteerAngleDegrees) - track.curvePts[rA].heading) 
                            - Math.PI) < glm.PIBy2;
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

            bool loops = track.mode > TrackMode.Curve;

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

                    newCurList.CalculateHeadings(loops);
                }
                else
                {
                    double step = (Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.4;
                    if (step > 2) step = 2;
                    if (step < 0.5) step = 0.5;

                    newCurList = track.curvePts.OffsetLine(distAway, step, loops);

                    if (track.mode != TrackMode.AB)
                    {
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

                        }
                    }

                    newCurList.CalculateHeadings(loops);

                    if (!loops)
                    {
                        vec3 pt1 = new vec3(newCurList[0]);
                        pt1.easting -= (Math.Sin(pt1.heading) * 10000);
                        pt1.northing -= (Math.Cos(pt1.heading) * 10000);

                        newCurList.Insert(0, pt1);

                        vec3 pt2 = new vec3(newCurList[newCurList.Count - 1]);
                        pt2.easting += (Math.Sin(pt2.heading) * 10000);
                        pt2.northing += (Math.Cos(pt2.heading) * 10000);

                        newCurList.Add(pt2);
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

                    double step = (Settings.Tool.toolWidth - Settings.Tool.overlap) * 0.48;
                    if (step > 4) step = 4;
                    if (step < 1) step = 1;

                    newGuideList = track.curvePts.OffsetLine(nextGuideDist, step, track.mode > TrackMode.Curve);

                    if (mf.bnd.bndList.Count > 0)
                    {
                        for (int i = newGuideList.Count - 1; i >= 0; i--)
                        {
                            if (!mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(newGuideList[i]))
                            {
                                newGuideList.RemoveAt(i);
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

        public void DrawTrack()
        {
            if (guideArr.Count > 0)
            {
                GL.LineWidth(Settings.User.setDisplay_lineWidth * 3);
                GL.Color3(0, 0, 0);

                for (int i = 0; i < guideArr.Count; i++)
                {
                    guideArr[i].DrawPolygon(currTrk.mode != TrackMode.bndCurve ? PrimitiveType.LineStrip : PrimitiveType.LineLoop);
                }

                GL.LineWidth(Settings.User.setDisplay_lineWidth);
                GL.Color4(0.2, 0.75, 0.2, 0.6);

                for (int i = 0; i < guideArr.Count; i++)
                {
                    guideArr[i].DrawPolygon(currTrk.mode != TrackMode.bndCurve ? PrimitiveType.LineStrip : PrimitiveType.LineLoop);
                }
            }

            //draw reference line
            if (currTrk.mode != TrackMode.waterPivot)
            {
                if (currTrk.curvePts == null || currTrk.curvePts.Count == 0) return;

                GL.LineWidth(Settings.User.setDisplay_lineWidth * 2);
                GL.Color3(0.96, 0.2f, 0.2f);
                currTrk.curvePts.DrawPolygon(PrimitiveType.Lines);

                if (mf.font.isFontOn)
                {
                    GL.Color3(0.40f, 0.90f, 0.95f);
                    mf.font.DrawText3D(currTrk.ptA.easting, currTrk.ptA.northing, "&A", true);
                    mf.font.DrawText3D(currTrk.ptB.easting, currTrk.ptB.northing, "&B", true);
                }

                //just draw ref and smoothed line if smoothing window is open
                if (isSmoothWindowOpen)
                {
                    if (smooList.Count == 0) return;

                    GL.LineWidth(Settings.User.setDisplay_lineWidth);
                    GL.Color3(0.930f, 0.92f, 0.260f);
                    smooList.DrawPolygon(PrimitiveType.LineStrip);
                }
            }

            //Draw Tracks
            if (currentGuidanceTrack.Count > 0 && !isSmoothWindowOpen) //normal. Smoothing window is not open.
            {
                GL.LineWidth(Settings.User.setDisplay_lineWidth * 4);
                GL.Color3(0, 0, 0);

                //ablines and curves are a line - the rest a loop
                currentGuidanceTrack.DrawPolygon(currTrk.mode <= TrackMode.Curve ? PrimitiveType.LineStrip : PrimitiveType.LineLoop);


                GL.LineWidth(Settings.User.setDisplay_lineWidth);
                GL.Color3(0.95f, 0.2f, 0.95f);

                if (currTrk.mode == TrackMode.waterPivot)
                {
                    GL.PointSize(15.0f);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex3(currTrk.ptA.easting, currTrk.ptA.northing, 0);
                    GL.End();
                }

                currentGuidanceTrack.DrawPolygon(currTrk.mode <= TrackMode.Curve ? PrimitiveType.LineStrip : PrimitiveType.LineLoop);

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
            mf.font.DrawText3D(designPtA.easting, designPtA.northing, "&A", true);
            mf.font.DrawText3D(designPtB.easting, designPtB.northing, "&B", true);
        }

        public void DrawNewTrack()
        {
            DrawABLineNew();

            if (designPtsList.Count > 0)
            {
                GL.Color3(0.95f, 0.42f, 0.750f);
                GL.LineWidth(4.0f);
                designPtsList.DrawPolygon(PrimitiveType.LineStrip);

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
                smooList.CalculateHeadings(track.mode > TrackMode.Curve);
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

        public CTrk CreateDesignedABTrack(bool isRefRightSide)
        {
            var track = new CTrk(TrackMode.AB);

            //fill in the dots between A and B
            double len = glm.Distance(designPtA, designPtB);
            if (len < 20)
            {
                designPtB.easting = designPtA.easting + (Math.Sin(designHeading) * 30);
                designPtB.northing = designPtA.northing + (Math.Cos(designHeading) * 30);
            }

            track.curvePts.Add(new vec3(designPtA, designHeading));
            track.curvePts.Add(new vec3(designPtB, designHeading));
            track.heading = designHeading;

            double dist = (Settings.Tool.toolWidth - Settings.Tool.overlap) * (isRefRightSide ? 0.5 : -0.5) + Settings.Tool.offset;
            NudgeRefTrack(track, dist);

            track.ptA = new vec2(track.curvePts[0]);
            track.ptB = new vec2(track.curvePts[track.curvePts.Count - 1]);

            //build the tail extensions
            AddFirstLastPoints(ref track.curvePts, 200);

            AddTrack(track);
            return track;
        }

        public void AddFirstLastPoints(ref List<vec3> xList, int metersToAdd)
        {
            int ptCnt = xList.Count - 1;
            vec3 start;

            for (int i = 2; i < metersToAdd; i += 2)
            {
                vec3 pt = new vec3(xList[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                xList.Add(pt);
            }

            //and the beginning
            start = new vec3(xList[0]);

            for (int i = 2; i < metersToAdd; i += 2)
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

            var curList = track.curvePts.OffsetLine(distAway, 1, track.mode > TrackMode.Curve);

            if (track.mode != TrackMode.AB)
            {
                //curList.CalculateHeadings(track.mode > TrackMode.Curve);

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
                }
            }
            else
            {
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


                track.ptA.easting = (track.curvePts[aClose].easting);
                track.ptA.northing = (track.curvePts[aClose].northing);
                track.ptB.easting = (track.curvePts[bClose].easting);
                track.ptB.northing = (track.curvePts[bClose].northing);
            }

            curList.CalculateHeadings(track.mode > TrackMode.Curve);
            track.curvePts = curList;
        }

        public void ResetTrack()
        {
            currentGuidanceTrack?.Clear();
            currTrk = null;
            _gArr.Clear();
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