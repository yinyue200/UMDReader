

using System;
using System.IO;
using System.Text;

namespace UMDReader
{
    public class CUMDBook
    {
        private byte[] _Book_Type = new byte[2]
        {
      (byte) 1,
      (byte) 1
        };
        private string _Book_Path = string.Empty;
        private string _Book_Title = string.Empty;
        private string _Book_Author = string.Empty;
        private DateTime _Book_PublishDate = DateTime.Now;
        private string _Book_Kind = string.Empty;
        private string _Book_Publisher = string.Empty;
        private string _Book_Vendor = string.Empty;
        private Windows.UI.Xaml.Media.Imaging.BitmapImage _Book_Cover = null;
        private CChapterList _Book_Chapters = new CChapterList();
        private Encoding _Book_Encoding = Encoding.Unicode;


        public byte[] BookType
        {
            get
            {
                return this._Book_Type;
            }
            set
            {
                this._Book_Type = value;
            }
        }

        public string BookTitle
        {
            get
            {
                return this._Book_Title;
            }
            set
            {
                this._Book_Title = value;
            }
        }

        public string Author
        {
            get
            {
                return this._Book_Author;
            }
            set
            {
                this._Book_Author = value;
            }
        }

        public string BookKind
        {
            get
            {
                return this._Book_Kind;
            }
            set
            {
                this._Book_Kind = value;
            }
        }

        public DateTime PublishDate
        {
            get
            {
                return this._Book_PublishDate;
            }
            set
            {
                this._Book_PublishDate = value;
            }
        }

        public string Publisher
        {
            get
            {
                return this._Book_Publisher;
            }
            set
            {
                this._Book_Publisher = value;
            }
        }

        public string Vendor
        {
            get
            {
                return this._Book_Vendor;
            }
            set
            {
                this._Book_Vendor = value;
            }
        }

        public Windows.UI.Xaml.Media.Imaging.BitmapImage Cover
        {
            get
            {
                return this._Book_Cover;
            }
            set
            {
                this._Book_Cover = value;
            }
        }

        public CChapterList Chapters
        {
            get
            {
                return this._Book_Chapters;
            }
        }

        public Encoding BookEncoding
        {
            get
            {
                return this._Book_Encoding;
            }
        }


        public void AppendChapter(CChapter chapter)
        {
            this._Book_Chapters.Add(chapter);
        }
    }
}
