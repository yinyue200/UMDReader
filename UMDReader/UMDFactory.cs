

using System.IO;

namespace UMDReader
{
  public static class UMDFactory
  {

    public static CUMDBook CreateNewUMDBook()
    {
      return new CUMDBook();
    }

    public static CUMDBook ReadUMDBook(Stream stream)
    {
      return new UMDBookReader().Read(stream);
    }

  }
}
