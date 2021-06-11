// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.CardImage
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class CardImage
  {
    private byte[] _image;
    private string _mimeType;

    public static CardImage CreateFromByteArray(byte[] image)
    {
      if (image == null || image.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (image), Microsoft.IdentityModel.SR.GetString("ID0015"));
      CardImage cardImage = new CardImage();
      using (MemoryStream memoryStream = new MemoryStream(image))
      {
        using (Image bitmapImage = Image.FromStream((Stream) memoryStream))
          cardImage.InitializeFromImage(bitmapImage);
      }
      return cardImage;
    }

    public static CardImage CreateFromImage(Image image)
    {
      if (image == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (image));
      CardImage cardImage = new CardImage();
      cardImage.InitializeFromImage(image);
      return cardImage;
    }

    private CardImage()
    {
    }

    public CardImage(byte[] image, string mimeType)
    {
      if (string.IsNullOrEmpty(mimeType))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (mimeType));
      if (image == null || image.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (image), Microsoft.IdentityModel.SR.GetString("ID0015"));
      this._mimeType = CardImage.IsValidMimeType(mimeType) ? mimeType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (mimeType), Microsoft.IdentityModel.SR.GetString("ID2042"));
      this._image = image;
    }

    public CardImage(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (fileName));
      try
      {
        this.InitializeFromImage(Image.FromFile(fileName));
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case FileNotFoundException _:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (fileName), Microsoft.IdentityModel.SR.GetString("ID2017", (object) fileName));
          case OutOfMemoryException _:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (fileName), Microsoft.IdentityModel.SR.GetString("ID2018", (object) fileName));
          default:
            throw;
        }
      }
    }

    public Bitmap BitmapImage
    {
      get
      {
        using (MemoryStream memoryStream = new MemoryStream(this._image))
          return new Bitmap(Image.FromStream((Stream) memoryStream));
      }
    }

    private static string GetMimeType(ImageFormat format)
    {
      if (format.Equals((object) ImageFormat.Bmp))
        return "image/bmp";
      if (format.Equals((object) ImageFormat.Gif))
        return "image/gif";
      if (format.Equals((object) ImageFormat.Jpeg))
        return "image/jpeg";
      if (format.Equals((object) ImageFormat.Png))
        return "image/png";
      return format.Equals((object) ImageFormat.Tiff) ? "image/tiff" : (string) null;
    }

    public byte[] GetImage() => this._image;

    private void InitializeFromImage(Image bitmapImage)
    {
      if (bitmapImage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (bitmapImage));
      ImageFormat format = CardImage.IsSupportedImageFormat(bitmapImage.RawFormat) ? bitmapImage.RawFormat : ImageFormat.Jpeg;
      this._mimeType = CardImage.GetMimeType(format);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        bitmapImage.Save((Stream) memoryStream, format);
        byte[] buffer = new byte[memoryStream.Length];
        memoryStream.Position = 0L;
        memoryStream.Read(buffer, 0, (int) memoryStream.Length);
        this._image = buffer;
      }
    }

    internal static bool IsValidMimeType(string mimeType)
    {
      if (string.IsNullOrEmpty(mimeType))
        return false;
      return StringComparer.Ordinal.Equals(mimeType, "image/bmp") || StringComparer.Ordinal.Equals(mimeType, "image/gif") || (StringComparer.Ordinal.Equals(mimeType, "image/jpeg") || StringComparer.Ordinal.Equals(mimeType, "image/png")) || StringComparer.Ordinal.Equals(mimeType, "image/tiff");
    }

    private static bool IsSupportedImageFormat(ImageFormat format) => CardImage.GetMimeType(format) != null;

    public string MimeType => this._mimeType;
  }
}
