using System;
using System.Text;

namespace AOG
{
    public class CFieldData
    {
        private readonly FormGPS mf;

        //all the section area added up;
        public double workedAreaTotal;

        //just a cumulative tally based on distance and eq width.
        public double workedAreaTotalUser;

        //accumulated user distance
        public double distanceUser;

        public double barPercent = 0;

        public double overlapPercent = 0;

        //Outside area minus inner boundaries areas (m)
        public double areaBoundaryOuterLessInner;

        //used for overlap calcs - total done minus overlap
        public double actualAreaCovered;

        //Inner area of outer boundary(m)
        public double areaOuterBoundary;

        //not really used - but if needed
        public double userSquareMetersAlarm;

        //Area inside Boundary less inside boundary areas
        public string AreaBoundaryLessInners => (areaBoundaryOuterLessInner * glm.m22HaOrAc).ToString("N2");

        //User tally string
        public string WorkedUserArea => (workedAreaTotalUser * glm.m22HaOrAc).ToString("N2");

        //String of Area worked
        public string WorkedArea => (workedAreaTotal * glm.m22HaOrAc).ToString("N2");

        //User Distance strings
        public string DistanceUser => Convert.ToString(Math.Round((distanceUser * glm.m2FtOrM), 1));

        public string WorkedAreaRemain => ((areaBoundaryOuterLessInner - workedAreaTotal) * glm.m22HaOrAc).ToString("N2");

        public string WorkedAreaRemainPercentage
        {
            get
            {
                if (areaBoundaryOuterLessInner > 10)
                {
                    barPercent = ((areaBoundaryOuterLessInner - workedAreaTotal) * 100 / areaBoundaryOuterLessInner);
                    return barPercent.ToString("N1") + "%";
                }
                else
                {
                    barPercent = 0;
                    return "0%";
                }
            }
        }

        //overlap strings
        public string ActualAreaWorked => (actualAreaCovered * glm.m22HaOrAc).ToString("N2");

        public string ActualRemain => ((areaBoundaryOuterLessInner - actualAreaCovered) * glm.m22HaOrAc).ToString("N2");

        public string ActualOverlapPercent => overlapPercent.ToString("N1") + "% ";

        public string TimeTillFinished
        {
            get
            {
                if (mf.avgSpeed > 2)
                {
                    TimeSpan timeSpan = TimeSpan.FromHours(((areaBoundaryOuterLessInner - workedAreaTotal) * glm.m2ha
                        / (Settings.Tool.toolWidth * mf.avgSpeed * 0.1)));
                    return timeSpan.Hours.ToString("00:") + timeSpan.Minutes.ToString("00") + '"';
                }
                else return "\u221E Hrs";
            }
        }

        public string WorkRateHour => (Settings.Tool.toolWidth * mf.avgSpeed * glm.m22HaOrAc * 1000).ToString("N1") + glm.unitsHaOrAcHr;

        //constructor
        public CFieldData(FormGPS _f)
        {
            mf = _f;
            workedAreaTotal = 0;
            workedAreaTotalUser = 0;
            userSquareMetersAlarm = 0;
        }

        public void UpdateFieldBoundaryGUIAreas()
        {
            mf.btnABDraw.Visible = mf.bnd.bndList.Count > 0;

            if (mf.bnd.bndList.Count > 0)
            {
                areaOuterBoundary = mf.bnd.bndList[0].area;
                areaBoundaryOuterLessInner = areaOuterBoundary;

                for (int i = 1; i < mf.bnd.bndList.Count; i++)
                {
                    areaBoundaryOuterLessInner -= mf.bnd.bndList[i].area;
                }
            }
            else
            {
                areaOuterBoundary = 0;
                areaBoundaryOuterLessInner = 0;
            }
        }

        public String GetDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Field: {0}", mf.displayFieldName);
            sb.AppendLine();
            sb.AppendFormat("Total Hectares: {0}", (areaBoundaryOuterLessInner * glm.m2ha).ToString("N2"));
            sb.AppendLine();
            sb.AppendFormat("Worked Hectares: {0}", (workedAreaTotal * glm.m2ha).ToString("N2"));
            sb.AppendLine();
            sb.AppendFormat("Missing Hectares: {0}", ((areaBoundaryOuterLessInner - workedAreaTotal) * glm.m2ha).ToString("N2"));
            sb.AppendLine();
            sb.AppendFormat("Total Acres: {0}", (areaBoundaryOuterLessInner * glm.m2ac).ToString("N2"));
            sb.AppendLine();
            sb.AppendFormat("Worked Acres: {0}", (workedAreaTotal * glm.m2ac).ToString("N2"));
            sb.AppendLine();
            sb.AppendFormat("Missing Acres: {0}", ((areaBoundaryOuterLessInner - workedAreaTotal) * glm.m2ac).ToString("N2"));
            sb.AppendLine();
            sb.AppendFormat("Tool Width: {0}", Settings.Tool.toolWidth);
            sb.AppendLine();
            sb.AppendFormat("Sections: {0}", mf.section.Count);
            sb.AppendLine();
            sb.AppendFormat("Section Overlap: {0}", Settings.Tool.overlap);
            sb.AppendLine();
            return sb.ToString();
        }
    }
}