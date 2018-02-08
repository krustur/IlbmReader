using System;
using System.Collections.Generic;
using System.Drawing;

namespace IlbmReaderTest
{
    internal class Cmap
    {
        public Cmap(IffChunk innerIlbmChunk, Ilbm ilbm)
        {
            if (ilbm.Bmhd == null)
            {
                throw new Exception("BMHD chunk not loaded error");
            }

            Colors = new List<Color>();
            for (int i = 0; i < innerIlbmChunk.ContentLength / 3; i++)
            {
                var color = Color.FromArgb(
                    innerIlbmChunk.Content[i * 3 + 0],
                    innerIlbmChunk.Content[i * 3 + 1],
                    innerIlbmChunk.Content[i * 3 + 2]
                    );
                Colors.Add(color);
            }
        }

        public IList<Color> Colors { get; }
    }

}