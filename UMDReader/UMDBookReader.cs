
using ICSharpCode.SharpZipLib.Zip.Compression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;

namespace UMDReader
{
    public class UMDBookReader
    {
        private CUMDBook _Book = new CUMDBook();
        private string _Publish_Year = string.Empty;
        private string _Publish_Month = string.Empty;
        private string _Publish_Day = string.Empty;
        private int[] _ChaptersOff = (int[])null;
        private int _TotalContentLen = 0;
        private List<byte[]> _ZippedContentList = new List<byte[]>();
        private List<BitmapImage> _TotalImageList = new List<BitmapImage>();
        private uint _AdditionalCheckNumber;

        public CUMDBook Read(Stream file)
        {
            using (file)
            {
                using (BinaryReader reader = new BinaryReader(file))
                {
                    try
                    {
                        this.Read(reader);
                    }
                    catch
                    {
                        this._Book = (CUMDBook)null;
                    }

                }
            }
            try
            {
                this._Book.PublishDate = new DateTime(int.Parse(this._Publish_Year), int.Parse(this._Publish_Month), int.Parse(this._Publish_Day));
            }
            catch
            {
                this._Book.PublishDate = DateTime.Now;
            }
            if ((int)this._Book.BookType[1] == 1)
                this.ParseChapterTxtContents();
            else if ((int)this._Book.BookType[1] == 2)
                this.ParseChapterImages();
            return this._Book;
        }

        private void Read(BinaryReader reader)
        {
            if ((int)reader.ReadUInt32() != -560292983)
                throw new Exception("Wrong header");
            short num1 = -1;
            char ch = (char)reader.PeekChar();
            while ((int)ch == 35)
            {
                int num2 = (int)reader.ReadChar();
                short segType = reader.ReadInt16();
                byte segFlag = reader.ReadByte();
                byte length1 = (byte)((uint)reader.ReadByte() - 5U);
                this.ReadSection(segType, segFlag, length1, reader);
                if ((int)segType == 241 || (int)segType == 10)
                    segType = num1;
                num1 = segType;
                for (ch = (char)reader.PeekChar(); (int)ch == 36; ch = (char)reader.PeekChar())
                {
                    int num3 = (int)reader.ReadChar();
                    uint additionalCheckNumber = reader.ReadUInt32();
                    uint length2 = reader.ReadUInt32() - 9U;
                    this.ReadAdditionalSection(segType, additionalCheckNumber, length2, reader);
                }
                /*Console.WriteLine("BEGIN");
                Console.WriteLine((int) segType);
                Console.WriteLine(ch);
                Console.WriteLine("END");*/
            }
        }

        protected void ReadSection(short segType, byte segFlag, byte length, BinaryReader reader)
        {
            switch (segType)
            {
                case 1:
                    this._Book.BookType[0] = reader.ReadByte();
                    this._Book.BookType[1] = this._Book.BookType[0];
                    int num1 = (int)reader.ReadInt16();
                    break;
                case 2:
                    this._Book.BookTitle = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 3:
                    this._Book.Author = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 4:
                    this._Publish_Year = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 5:
                    this._Publish_Month = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 6:
                    this._Publish_Day = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 7:
                    this._Book.BookKind = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 8:
                    this._Book.Publisher = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 9:
                    this._Book.Vendor = this._Book.BookEncoding.GetString(reader.ReadBytes((int)length));
                    break;
                case 10:
                    reader.ReadInt32();
                    break;
                case 11:
                    this._TotalContentLen = reader.ReadInt32();
                    break;
                case 12:
                    int num2 = (int)reader.ReadUInt32();
                    break;
                case 13:
                    //Console.WriteLine("Seq type = " + (object) 13);
                    //Console.WriteLine(reader.ReadUInt32());
                    break;
                case 14:
                    int num3 = (int)reader.ReadByte();
                    break;
                case 15:
                    reader.ReadBytes((int)length);
                    this._Book.BookType[0] = (byte)3;
                    break;
                case 129:
                case 131:
                case 132:
                    this._AdditionalCheckNumber = reader.ReadUInt32();
                    break;
                case 130:
                    int num4 = (int)reader.ReadByte();
                    this._AdditionalCheckNumber = reader.ReadUInt32();
                    break;
                default:
                    byte[] numArray = reader.ReadBytes((int)length);
                    //Console.WriteLine("未知编码");
                    //Console.WriteLine("Seg Type = " + (object) segType);
                    //Console.WriteLine("Seg Flag = " + (object) segFlag);
                    //Console.WriteLine("Seg Len = " + (object) length);
                    //Console.WriteLine("Seg content = " + numArray.ToString());
                    break;
            }
        }

        protected virtual void ReadAdditionalSection(short segType, uint additionalCheckNumber, uint length, BinaryReader reader)
        {
            switch (segType)
            {
                case 14:
                    //this._TotalImageList.Add((object) Image.FromStream((Stream) new MemoryStream(reader.ReadBytes((int) length))));
                    break;
                case 15:
                    //this._TotalImageList.Add((object) Image.FromStream((Stream) new MemoryStream(reader.ReadBytes((int) length))));
                    break;
                case 129:
                    reader.ReadBytes((int)length);
                    break;
                case 130:
                    //this._Book.Cover = BitmapImage.FromStream((Stream) new MemoryStream(reader.ReadBytes((int) length)));
                    break;
                case 131:
                    this._ChaptersOff = (int[])null;
                    this._ChaptersOff = new int[length / 4U];
                    for (int index = 0; index < this._ChaptersOff.Length; ++index)
                        this._ChaptersOff[index] = reader.ReadInt32();
                    break;
                case 132:
                    if ((int)this._AdditionalCheckNumber != (int)additionalCheckNumber)
                    {
                        this._ZippedContentList.Add(reader.ReadBytes((int)length));
                        break;
                    }
                    int index1 = 0;
                    byte num;
                    int index2;
                    for (byte[] bytes = reader.ReadBytes((int)length); index1 < bytes.Length; index1 = index2 + (int)num)
                    {
                        num = bytes[index1];
                        index2 = index1 + 1;
                        this._Book.Chapters.Add(new CChapter(this._Book.BookEncoding.GetString(bytes, index2, (int)num), string.Empty));
                    }
                    break;
                default:
                    /*Console.WriteLine("未知内容");
                    Console.WriteLine("Seg Type = " + (object) segType);
                    Console.WriteLine("Seg Len = " + (object) length);
                    Console.WriteLine("content = " + (object) reader.ReadBytes((int) length));*/
                    break;
            }
        }

        private void ParseChapterImages()
        {
            int num1 = 0;
            for (int index1 = 0; index1 < this._Book.Chapters.Count; ++index1)
            {
                num1 = index1 >= this._Book.Chapters.Count - 1 ? this._TotalImageList.Count : this._ChaptersOff[index1 + 1];
                int num2 = this._ChaptersOff[index1];
                for (int index2 = this._ChaptersOff[index1]; index2 < num1; ++index2)
                    this._Book.Chapters[index1].AppendImage(this._TotalImageList[index2]);
            }
            if (num1 < this._TotalImageList.Count)
            {
                CChapter chapter = new CChapter("未知", string.Empty);
                for (int index = num1; index < this._TotalImageList.Count; ++index)
                    chapter.AppendImage(this._TotalImageList[index]);
                this._Book.AppendChapter(chapter);
            }
            this._TotalImageList.Clear();
        }

        private void ParseChapterTxtContents()
        {
            int destinationIndex = 0;
            byte[] bytes = new byte[this._TotalContentLen];
            byte[] buf = new byte[32768];
            foreach (byte[] zippedContent in this._ZippedContentList)
            {
                Inflater inflater = new Inflater();
                inflater.SetInput(zippedContent);
                inflater.Inflate(buf);
                if (destinationIndex < bytes.Length)
                {
                    Array.Copy((Array)buf, 0, (Array)bytes, destinationIndex, (int)Math.Min(bytes.Length - destinationIndex, inflater.TotalOut));
                    destinationIndex += (int)inflater.TotalOut;
                }
            }
            for (int index1 = 0; index1 < this._ChaptersOff.Length; ++index1)
            {
                int index2 = this._ChaptersOff[index1];
                int count = index1 >= this._ChaptersOff.Length - 1 ? bytes.Length - index2 : this._ChaptersOff[index1 + 1] - index2;
                string str = this._Book.BookEncoding.GetString(bytes, index2, count);
                this._Book.Chapters[index1].Content = str.Replace("\x2029", "\r\n");
            }
            this._ZippedContentList.Clear();
        }

        private CChapter GetChapter(int index)
        {
            if (index != this._Book.Chapters.Count)
                throw new Exception("堆栈溢出！");
            this._Book.Chapters.Add(new CChapter());
            return this._Book.Chapters[index];
        }
    }
}
