using System;
using System.Collections.Generic;
using System.Linq;

namespace IlbmReaderTest
{
    internal class IffFile
    {
        public IList<Ilbm> Ilbms { get; }
        public IList<IffChunk> AllChunks { get; }
        public bool IsAnim { get; internal set; }

        

        public IffFile()
        {
            Ilbms = new List<Ilbm>();
            AllChunks = new List<IffChunk>();
    }

        internal Bmhd GetBmhd()
        {
            var bmhd = Ilbms.Single(x => x.Bmhd != null).Bmhd;
            if (bmhd == null)
            {
                throw new Exception("BMHD chunk not loaded error");
            }

            return bmhd;
        }

        internal Body GetBody()
        {
            var body = Ilbms.Single(x => x.Body != null).Body;
            return body;
        }

        internal Cmap GetCmap()
        {
            var cmap = Ilbms.Single(x => x.Cmap != null).Cmap;
            return cmap;
        }
    }
}