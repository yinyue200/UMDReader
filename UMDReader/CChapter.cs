
using System.Collections;
using System.Collections.Generic;

namespace UMDReader
{
  public class CChapter
  {
    private string content = string.Empty;
    private string title = string.Empty;
        private List<Windows.UI.Xaml.Media.Imaging.BitmapImage> imagelist = new List<Windows.UI.Xaml.Media.Imaging.BitmapImage>();

    public string Content
    {
      get
      {
        return this.content;
      }
      set
      {
        this.content = value;
      }
    }

    public string Title
    {
      get
      {
        return this.title;
      }
      set
      {
        this.title = value;
      }
    }

    public List<Windows.UI.Xaml.Media.Imaging.BitmapImage> ImageList
    {
      get
      {
        return this.imagelist;
      }
      set
      {
        this.imagelist = value;
      }
    }

    public CChapter()
    {
    }

    public CChapter(string title, string content)
    {
      this.title = title;
      this.content = content;
    }

    public void AppendImage(Windows.UI.Xaml.Media.Imaging.BitmapImage picture)
    {
      this.imagelist.Add(picture);
    }

    public void RemoveImage(int index)
    {
      if (index < 0 || index >= this.imagelist.Count)
        return;
      this.imagelist.RemoveAt(index);
    }

    public override string ToString()
    {
      return this.title;
    }
  }
}
