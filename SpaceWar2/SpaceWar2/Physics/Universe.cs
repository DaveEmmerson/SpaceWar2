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

        internal Universe(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {

            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;
        }
    }
}
