using System.Collections.Generic;

namespace AOG
{
    public class CHeadLine
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        public List<CHeadPath> tracksArr = new List<CHeadPath>();

        public int idx;

        public CHeadLine(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        //for calculating for display the averaged new line
    }

    public class CHeadPath
    {
        public List<vec3> trackPts = new List<vec3>();
        public string name = "";
        public double moveDistance = 0;
        public TrackMode mode = 0;
        public int a_point = 0;
    }
}