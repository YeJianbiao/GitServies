using System;
using SharpSvn;

namespace Appeon.SnapDevelop.SvnServices
{
    public class Revision
    {
        SvnRevision revision;

        public static readonly Revision Base = SvnRevision.Base;
        public static readonly Revision Committed = SvnRevision.Committed;
        public static readonly Revision Head = SvnRevision.Head;
        public static readonly Revision Working = SvnRevision.Working;
        public static readonly Revision Unspecified = SvnRevision.None;

        public static Revision FromNumber(long number)
        {
            return new SvnRevision(number);
        }

        public static implicit operator SvnRevision(Revision r)
        {
            return r.revision;
        }

        public static implicit operator Revision(SvnRevision r)
        {
            return new Revision() { revision = r };
        }

        public override string ToString()
        {
            switch (revision.RevisionType)
            {
                case SvnRevisionType.Base:
                    return "base";
                case SvnRevisionType.Committed:
                    return "committed";
                case SvnRevisionType.Time:
                    return revision.Time.ToString();
                case SvnRevisionType.Head:
                    return "head";
                case SvnRevisionType.Number:
                    return revision.Revision.ToString();
                case SvnRevisionType.Previous:
                    return "previous";
                case SvnRevisionType.None:
                    return "unspecified";
                case SvnRevisionType.Working:
                    return "working";
                default:
                    return "unknown";
            }
        }
    }
}
