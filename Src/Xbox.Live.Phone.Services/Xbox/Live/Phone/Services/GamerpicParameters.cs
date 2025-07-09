// *********************************************************
// Type: Xbox.Live.Phone.Services.GamerpicParameters
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Xna.Framework;
using System;


namespace Xbox.Live.Phone.Services
{
  public class GamerpicParameters
  {
    public Guid AnimationId { get; set; }

    public float AnimationFrame { get; set; }

    public bool UseProp { get; set; }

    public Vector3 Offset { get; set; }

    public Vector3 Rotation { get; set; }

    public int Background { get; set; }

    public float FieldOfView { get; set; }

    public int Joint { get; set; }
  }
}
