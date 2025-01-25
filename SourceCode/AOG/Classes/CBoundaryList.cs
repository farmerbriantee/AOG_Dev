using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class CBoundaryList
    {
        //list of coordinates of boundary line
        public List<vec3> fenceLine = new List<vec3>(128);

        public List<vec2> fenceLineEar = new List<vec2>(128);
        public List<vec3> hdLine = new List<vec3>(128);
        public List<vec3> turnLine = new List<vec3>(128);

        public List<Triangle> bndTriangleList = new List<Triangle>(128);
        public List<Triangle> hdLineTriangleList = new List<Triangle>(128);

        public CPolygon bndPolygon = new CPolygon();
        public CPolygon hdLinePolygon = new CPolygon();

        //constructor
        public CBoundaryList()
        {
            area = 0;
            isDriveThru = false;
        }
    }
}