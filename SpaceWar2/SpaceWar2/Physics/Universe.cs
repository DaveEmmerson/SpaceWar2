namespace DEMW.SpaceWar2.Physics
{
    internal struct Universe
    {
        internal float MinX;
        internal float MaxX;
        internal float MinY;
        internal float MaxY;
        internal float MinZ;
        internal float MaxZ;

        public float Width
        {
            get
            {
                return MaxX - MinX;

            }

        }

        public float Height
        {
            get
            {
                return MaxY - MinY;

            }

        }

        internal Universe(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {

            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;
        }

        internal Universe Expand(float amount)
        {
            return new Universe(
                MinX - amount, MaxX + amount,
                MinY - amount, MaxY + amount,
                MinZ - amount, MaxZ + amount
            );

        }

        internal static Universe GetDefault()
        {
            return new Universe(-400, 400, -240, 240, -1000, 1000);
        }
    }
}
