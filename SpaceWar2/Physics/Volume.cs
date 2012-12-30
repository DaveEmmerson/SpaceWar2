using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Physics
{
    public class Volume
    {
        //BoundingBox is a handy struct provided by XNA that contains the properties we need
        private BoundingBox _boundingBox;

        public Volume(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            var minimumBound = new Vector3(minX, minY, minZ);
            var maximumBound = new Vector3(maxX, maxY, maxZ);
            _boundingBox = new BoundingBox(minimumBound, maximumBound);
        }

        public float MinX { get { return _boundingBox.Min.X; } }
        public float MaxX { get { return _boundingBox.Max.X; } }
        public float MinY { get { return _boundingBox.Min.Y; } }
        public float MaxY { get { return _boundingBox.Max.Y; } }
        public float MinZ { get { return _boundingBox.Min.Z; } }
        public float MaxZ { get { return _boundingBox.Max.Z; } }

        public float Width
        {
            get { return _boundingBox.Max.X - _boundingBox.Min.X; }
        }

        public float Height
        {
            get { return _boundingBox.Max.Y - _boundingBox.Min.Y; }
        }

        public void Expand(float verticalAmount)
        {
            var horizontalAmount = verticalAmount * Width / Height;
            var delta = new Vector3(horizontalAmount, verticalAmount, 0);
            _boundingBox.Min -= delta;
            _boundingBox.Max += delta;
        }

        public void Contract(float verticalAmount)
        {
            Expand(-verticalAmount);
        }

        public Volume Clone()
        {
            return new Volume(MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
        }
    }
}
