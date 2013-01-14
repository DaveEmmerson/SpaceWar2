using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Core.Physics
{
    public class Volume
    {
        //BoundingBox is a handy struct provided by XNA that contains the properties we need
        private BoundingBox _boundingBox;

        internal Volume(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            var minimumBound = new Vector3(minX, minY, minZ);
            var maximumBound = new Vector3(maxX, maxY, maxZ);
            _boundingBox = new BoundingBox(minimumBound, maximumBound);
        }

        internal float MinX { get { return _boundingBox.Min.X; } }
        internal float MaxX { get { return _boundingBox.Max.X; } }
        internal float MinY { get { return _boundingBox.Min.Y; } }
        internal float MaxY { get { return _boundingBox.Max.Y; } }
        internal float MinZ { get { return _boundingBox.Min.Z; } }
        internal float MaxZ { get { return _boundingBox.Max.Z; } }

        internal float Width
        {
            get { return _boundingBox.Max.X - _boundingBox.Min.X; }
        }

        internal float Height
        {
            get { return _boundingBox.Max.Y - _boundingBox.Min.Y; }
        }

        internal void Expand(float verticalAmount)
        {
            var horizontalAmount = verticalAmount * Width / Height;
            var delta = new Vector3(horizontalAmount, verticalAmount, 0);
            _boundingBox.Min -= delta;
            _boundingBox.Max += delta;
        }

        internal void Contract(float verticalAmount)
        {
            Expand(-verticalAmount);
        }

        internal Volume Clone()
        {
            return new Volume(MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
        }
    }
}
