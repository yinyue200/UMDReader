

using System.Collections;
using System.Collections.Generic;

namespace UMDReader
{
  public class CChapterList : List<CChapter>
  {

    public int IndexOf(string title)
    {
      int num = -1;
      for (int index = 0; index < Count; ++index)
      {
        if (this[index].Title == title)
          return index;
      }
      return num;
    }


    public void Remove(string title)
    {
      int index = this.IndexOf(title);
      if (index == -1)
        return;
      this.RemoveAt(index);
    }
  }
}
