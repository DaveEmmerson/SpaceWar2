namespace SpaceWar2
{
    interface IMassive : IGameObject
    {
        float Mass { get; set; }

        float Radius { get; set; }
    }
}
