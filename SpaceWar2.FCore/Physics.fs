namespace DEMW.SpaceWar2.FCore.Physics

module Physics =

    open Microsoft.Xna.Framework

    type Force = {
        vector : Vector2;
        displacement : Vector2
    }
    with
        member this.zero = { vector = Vector2.Zero; displacement = Vector2.Zero }

    let rotate force angle =
        let rotation = Matrix.CreateRotationZ(angle)
        
        { vector = Vector2.Transform(force.vector, rotation)
          displacement = Vector2.Transform(force.displacement, rotation) }



        