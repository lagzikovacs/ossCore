using ossServer.Models;
using System;

namespace ossServer.Utils
{
  public class Register
  {
    public static void Creation(ossContext model, object entity)
    {
      var piParticiokod = entity.GetType().GetProperty("Particiokod");
      if (piParticiokod != null)
      {
        if ((int) piParticiokod.GetValue(entity) == 0)
          piParticiokod.SetValue(entity, model.CurrentSession.Particiokod);
      }

      var piLetrehozta = entity.GetType().GetProperty("Letrehozta");
      if (piLetrehozta != null)
      {
        if (piLetrehozta.GetValue(entity) == null)
          piLetrehozta.SetValue(entity, model.CurrentSession.Felhasznalo);
      }

      var piLetrehozva = entity.GetType().GetProperty("Letrehozva");
      if (piLetrehozva != null)
      {
        if (((DateTime) piLetrehozva.GetValue(entity)) == DateTime.MinValue)
          piLetrehozva.SetValue(entity, DateTime.Now);
      }

      Modification(model, entity);
    }

    public static void Modification(ossContext model, object entity)
    {
      var piModositotta = entity.GetType().GetProperty("Modositotta");
      if (piModositotta != null)
        piModositotta.SetValue(entity, model.CurrentSession.Felhasznalo);

      var piModositva = entity.GetType().GetProperty("Modositva");
      if (piModositva != null)
        piModositva.SetValue(entity, DateTime.Now);
    }
  }
}