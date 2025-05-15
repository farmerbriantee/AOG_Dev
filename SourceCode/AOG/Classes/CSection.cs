//Please, if you use this, share the improvements

namespace AOG
{
    //each section is composed of a patchlist and triangle list
    //the triangle list makes up the individual triangles that make the block or patch of applied (green spot)
    //the patch list is a list of the list of triangles

    public class CSection
    {
        //is this section on or off
        public bool isSectionOn = false;

        public bool sectionOnRequest = false;

        public int sectionOnTimer = 0;
        public int sectionOffTimer = 0;

        //mapping
        public bool isMappingOn = false;

        public int mappingOnTimer = 0;
        public int mappingOffTimer = 0;

        public double speedPixels = 0;

        //the left side is always negative, right side is positive
        //so a section on the left side only would be -8, -4
        //in the center -4,4  on the right side only 4,8
        //reads from left to right
        //   ------========---------||----------========---------
        //        -8      -4      -1  1         4      8
        // in (meters)

        public double positionLeft = -4;
        public double positionRight = 4;
        public double sectionWidth = 0;

        public double foreDistance = 0;

        //used by readpixel to determine color in pixel array
        public int rpSectionWidth = 0;

        public int rpSectionPosition = 0;

        //points in world space that start and end of section are in
        public vec2 leftPoint;

        public vec2 rightPoint;

        //used to determine left and right speed of section
        public vec2 lastLeftPoint;

        public vec2 lastRightPoint;

        //whether or not this section is in boundary, headland
        public bool isHydLiftInWorkArea = true;

        public int numTriangles = 0;

        //used to determine state of Manual section button - Off Auto On
        public btnStates sectionBtnState = btnStates.Off;

        //simple constructor, position is set in GPSWinForm_Load in FormGPS when creating new object
        public CSection()
        {
        }
    }
}