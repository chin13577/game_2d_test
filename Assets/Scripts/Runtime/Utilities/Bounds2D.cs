using System;
using UnityEngine;

namespace FS.Util
{
    [Serializable]
    public struct Bounds2D
    {
        public float x;
        public float y;
        public float w;
        public float h;

        public Bounds2D(float x, float y, float w, float h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public Bounds2D(float w, float h)
        {
            x = 0;
            y = 0;
            this.w = w;
            this.h = h;
        }

        public AnchorBounds2D Anchor
        {
            get
            {
                return new AnchorBounds2D(this);
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(x, y);
            }
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        public Vector2 Extents
        {
            get
            {
                return new Vector2(w, h);
            }
            set
            {
                w = value.x;
                h = value.y;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(w, h) / 2.0f;
            }
            set
            {
                w = value.x / 2.0f;
                h = value.y / 2.0f;
            }
        }

        public Vector2 Min
        {
            get
            {
                return Center - Extents;
            }
            set
            {
                SetMinMax(value, Max);
            }
        }

        public Vector2 Max
        {
            get
            {
                return Center + Extents;
            }
            set
            {
                SetMinMax(Min, value);
            }
        }

        public void SetMinMax(Vector2 min, Vector2 max)
        {
            Extents = (max - min) * 0.5f;
            Center = min + Extents;
        }

        public struct AnchorBounds2D
        {
            public Vector2 topLeft;
            public Vector2 topRight;
            public Vector2 bottomLeft;
            public Vector2 bottomRight;

            public Vector2 centerLeft;
            public Vector2 centerRight;
            public Vector2 centerTop;
            public Vector2 centerBottom;

            public AnchorBounds2D(Bounds2D bound)
            {
                topLeft = new Vector2((bound.x - bound.w / 2), (bound.y + bound.h / 2));
                bottomLeft = new Vector2((bound.x - bound.w / 2), (bound.y - bound.h / 2));
                topRight = new Vector2((bound.x + bound.w / 2), (bound.y + bound.h / 2));
                bottomRight = new Vector2((bound.x + bound.w / 2), (bound.y - bound.h / 2));

                centerLeft = new Vector2((bound.x - bound.w / 2), bound.y);
                centerRight = new Vector2((bound.x + bound.w / 2), bound.y);
                centerTop = new Vector2(bound.x, (bound.y + bound.h / 2));
                centerBottom = new Vector2(bound.x, (bound.y - bound.h / 2));
            }
            public override string ToString()
            {
                return " centerUp : " + centerTop.y + " centerDown : " + centerBottom.y;
            }
        }

        public override string ToString()
        {
            return " center : " + Center + " width : " + w + " height : " + h + Anchor.ToString();
        }
    }
}