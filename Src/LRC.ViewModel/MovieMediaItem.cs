// *********************************************************
// Type: LRC.ViewModel.MovieMediaItem
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class MovieMediaItem : MediaItem
  {
    private bool isCastAndCrewNotFound;
    private string actorsTitle;
    private string directorsTitle;
    private ObservableCollection<string> actors;
    private ObservableCollection<string> directors;

    public MovieMediaItem() => this.MediaType = MediaType.movie;

    public MovieMediaItem(string id)
      : this()
    {
      this.Id = id;
    }

    public bool IsCastAndCrewNotFound
    {
      get => this.isCastAndCrewNotFound;
      set
      {
        this.SetPropertyValue<bool>(ref this.isCastAndCrewNotFound, value, nameof (IsCastAndCrewNotFound));
      }
    }

    public string ActorsTitle
    {
      get => this.actorsTitle;
      set => this.SetPropertyValue<string>(ref this.actorsTitle, value, nameof (ActorsTitle));
    }

    [DataMember]
    public ObservableCollection<string> Actors
    {
      get => this.actors;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.actors, value, nameof (Actors));
        this.ActorsTitle = this.Actors == null || this.Actors.Count <= 1 ? Resource.Actor_Header : Resource.Actors_Header;
      }
    }

    public string DirectorsTitle
    {
      get => this.directorsTitle;
      set => this.SetPropertyValue<string>(ref this.directorsTitle, value, nameof (DirectorsTitle));
    }

    public ObservableCollection<string> Directors
    {
      get => this.directors;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.directors, value, nameof (Directors));
        this.DirectorsTitle = this.Directors == null || this.Directors.Count <= 1 ? Resource.Director_Header : Resource.Directors_Header;
      }
    }
  }
}
