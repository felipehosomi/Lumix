using MessagingToolkit.QRCode.Codec;
using Nordware.AddOn.Lumix.Core.Model;
using Nordware.AddOn.Lumix.Core.Utils;
using SBO.Hub;
using SBO.Hub.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Nordware.AddOn.Lumix.Core.BLL
{
    public class OLDQrCodeBLL
    {
        // Print QrCode label using EPL language
        public string GenerateEPL(List<LabelModel> labelList, string printer)
        {
            try
            {
                if (!Directory.Exists("c:\\temp\\"))
                {
                    Directory.CreateDirectory("c:\\temp\\");
                }

                StreamReader sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Label", "EPL6.prn"));

                string baseFile = sr.ReadToEnd();
                sr.Close();
                string str = baseFile;

                int i = 0;

                foreach (var label in labelList)
                {
                    QRCodeEncoder qrCodecEncoder = new QRCodeEncoder();
                    qrCodecEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
                    qrCodecEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
                    qrCodecEncoder.CharacterSet = "UTF-8";
                    qrCodecEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    qrCodecEncoder.QRCodeScale = 4;
                    qrCodecEncoder.QRCodeVersion = 0;
                    qrCodecEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;

                    Bitmap image = new Bitmap((Image)qrCodecEncoder.Encode(label.ItemCode + "|" + label.DistNumber), new Size(101, 101));

                    image.Save($"c:\\temp\\QRCode.png");

                    System.Drawing.Bitmap rotateImage = RotateImg(image, 180);
                    rotateImage.Save("c:\\temp\\QRCodeNovo.png");

                    str = str.Replace($"%QRCode{i + 1}%", SendImageToPrinter(10 + (140 * i), 16, rotateImage));

                    if (label.ItemCode.Length < 13)
                    {
                        str = str.Replace($"%Item{i + 1}%", label.ItemCode);
                    }
                    else
                    {
                        str = str.Replace($"%Item{i + 1}%", label.ItemCode.Substring(0, 13));
                    }

                    if (label.DistNumber.Length < 13)
                    {
                        str = str.Replace($"%Lote{i + 1}%", label.DistNumber);
                    }
                    else
                    {
                        str = str.Replace($"%Lote{i + 1}%", label.DistNumber.Substring(0, 13));
                    }

                    File.Delete($"c:\\temp\\QRCode.png");

                    i++;
                    if (i == 6)
                    {
                        str = str.Replace($"%QRCode2%", String.Empty);
                        str = str.Replace($"%QRCode3%", String.Empty);
                        str = str.Replace($"%QRCode4%", String.Empty);
                        str = str.Replace($"%QRCode5%", String.Empty);
                        str = str.Replace($"%QRCode6%", String.Empty);

                        str = str.Replace($"%Item2%", String.Empty);
                        str = str.Replace($"%Item3%", String.Empty);
                        str = str.Replace($"%Item4%", String.Empty);
                        str = str.Replace($"%Item5%", String.Empty);
                        str = str.Replace($"%Item6%", String.Empty);

                        str = str.Replace($"%Lote2%", String.Empty);
                        str = str.Replace($"%Lote3%", String.Empty);
                        str = str.Replace($"%Lote4%", String.Empty);
                        str = str.Replace($"%Lote5%", String.Empty);
                        str = str.Replace($"%Lote6%", String.Empty);

                        i = 0;

                        File.WriteAllText($"c:\\temp\\WriteLines{i.ToString()+"-"+DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString()}.txt", str);
                        RawPrinter.SendStringToPrinter(printer, str);
                        str = baseFile;
                    }
                }

                if (i > 0)
                {
                    str = str.Replace($"%QRCode2%", String.Empty);
                    str = str.Replace($"%QRCode3%", String.Empty);
                    str = str.Replace($"%QRCode4%", String.Empty);
                    str = str.Replace($"%QRCode5%", String.Empty);
                    str = str.Replace($"%QRCode6%", String.Empty);

                    str = str.Replace($"%Item2%", String.Empty);
                    str = str.Replace($"%Item3%", String.Empty);
                    str = str.Replace($"%Item4%", String.Empty);
                    str = str.Replace($"%Item5%", String.Empty);
                    str = str.Replace($"%Item6%", String.Empty);

                    str = str.Replace($"%Lote2%", String.Empty);
                    str = str.Replace($"%Lote3%", String.Empty);
                    str = str.Replace($"%Lote4%", String.Empty);
                    str = str.Replace($"%Lote5%", String.Empty);
                    str = str.Replace($"%Lote6%", String.Empty);

                    File.WriteAllText($"c:\\temp\\WriteLines{i.ToString() + "-" + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString()}.txt", str);
                    RawPrinter.SendStringToPrinter(printer, str);
                }

                //File.Delete("c:\\temp\\QRCode.png");
                return String.Empty;
            }
            catch (Exception ex)
            {
                LogHelper.LogFileException(ex);
                return ex.Message;
            }
        }

        private static System.Drawing.Bitmap RotateImg(System.Drawing.Bitmap bmp, float angle)
        {
            angle = angle % 360;
            if (angle > 180) angle -= 360;
            float sin = (float)System.Math.Abs(System.Math.Sin(angle * System.Math.PI / 180.0)); // this function takes radians
            float cos = (float)System.Math.Abs(System.Math.Cos(angle * System.Math.PI / 180.0)); // this one too
            float newImgWidth = sin * bmp.Height + cos * bmp.Width;
            float newImgHeight = sin * bmp.Width + cos * bmp.Height;
            float originX = 0f;
            float originY = 0f;
            if (angle > 0)
            {
                if (angle <= 90)
                    originX = sin * bmp.Height;
                else
                {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * bmp.Width;
                }
            }
            else
            {
                if (angle >= -90)
                    originY = sin * bmp.Width;
                else
                {
                    originX = newImgWidth - sin * bmp.Height;
                    originY = newImgHeight;
                }
            }
            System.Drawing.Bitmap newImg =
            new System.Drawing.Bitmap((int)newImgWidth, (int)newImgHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newImg);
            g.Clear(System.Drawing.Color.White);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
            g.Dispose();
            return newImg;
        }

        public static string SendImageToPrinter(int top, int left, string source, float angle)
        {
            System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(source);
            System.Drawing.Bitmap newbitmap = RotateImg(bitmap, angle);
            return SendImageToPrinter(top, left, bitmap);
        }
        public static string SendImageToPrinter(int top, int left, string source)
        {
            System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(source);
            return SendImageToPrinter(top, left, bitmap);
        }


        private static string SendImageToPrinter(int top, int left, Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms, Encoding.ASCII);


            //we set p3 parameter, remember it is Width of Graphic in bytes,
            //so we divive the width of image and round up of it
            int P3 = (int)System.Math.Ceiling((double)bitmap.Width / 8);
            bw.Write(Encoding.ASCII.GetBytes(string.Format
            ("GW{0},{1},{2},{3},", top, left, P3, bitmap.Height)));
            //the width of matrix is rounded up multi of 8
            int canvasWidth = P3 * 8;
            //Now we convert image into 2 dimension binary matrix by 2 for loops below,
            //in the range of image, we get colour of pixel of image,
            //calculate the luminance in order to set value of 1 or 0
            //otherwise we set value to 1
            //Because P3 is set to byte (8 bits), so we gather 8 dots of this matrix,
            //convert into a byte then write it to memory by using shift left operator <<
            //e,g 1 << 7  ---> 10000000
            //    1 << 6  ---> 01000000
            //    1 << 3  ---> 00001000
            for (int y = 0; y < bitmap.Height; ++y)     //loop from top to bottom
            {
                for (int x = 0; x < canvasWidth;)       //from left to right
                {
                    byte abyte = 0;
                    for (int b = 0; b < 8; ++b, ++x)     //get 8 bits together and write to memory
                    {
                        int dot = 1;                     //set 1 for white,0 for black
                                                         //pixel still in width of bitmap,
                                                         //check luminance for white or black, out of bitmap set to white
                        if (x < bitmap.Width)
                        {
                            System.Drawing.Color color = bitmap.GetPixel(x, y);
                            int luminance = (int)((color.R * 0.3) + (color.G * 0.59) + (color.B * 0.11));
                            dot = luminance > 127 ? 1 : 0;
                        }
                        abyte |= (byte)(dot << (7 - b)); //shift left,
                                                         //then OR together to get 8 bits into a byte
                    }
                    bw.Write(abyte);
                }
            }
            bw.Write("\n");
            bw.Flush();
            //reset memory
            ms.Position = 0;
            //get encoding, I have no idea why encode page of 1252 works and fails for others
            string text = Encoding.GetEncoding(1252).GetString(ms.ToArray());
            ms.Close();
            bw.Close();
            return text;
        }

        private static string SendImageToPrinter(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms, Encoding.ASCII);

            //we set p3 parameter, remember it is Width of Graphic in bytes,
            //so we divive the width of image and round up of it
            int P3 = (int)System.Math.Ceiling((double)bitmap.Width / 8);

            //the width of matrix is rounded up multi of 8
            int canvasWidth = P3 * 8;
            //Now we convert image into 2 dimension binary matrix by 2 for loops below,
            //in the range of image, we get colour of pixel of image,
            //calculate the luminance in order to set value of 1 or 0
            //otherwise we set value to 1
            //Because P3 is set to byte (8 bits), so we gather 8 dots of this matrix,
            //convert into a byte then write it to memory by using shift left operator <<
            //e,g 1 << 7  ---> 10000000
            //    1 << 6  ---> 01000000
            //    1 << 3  ---> 00001000
            for (int y = 0; y < bitmap.Height; ++y)     //loop from top to bottom
            {
                for (int x = 0; x < canvasWidth;)       //from left to right
                {
                    byte abyte = 0;
                    for (int b = 0; b < 8; ++b, ++x)     //get 8 bits together and write to memory
                    {
                        int dot = 1;                     //set 1 for white,0 for black
                                                         //pixel still in width of bitmap,
                                                         //check luminance for white or black, out of bitmap set to white
                        if (x < bitmap.Width)
                        {
                            System.Drawing.Color color = bitmap.GetPixel(x, y);
                            int luminance = (int)((color.R * 0.3) + (color.G * 0.59) + (color.B * 0.11));
                            dot = luminance > 127 ? 1 : 0;
                        }
                        abyte |= (byte)(dot << (7 - b)); //shift left,
                                                         //then OR together to get 8 bits into a byte
                    }
                    bw.Write(abyte);
                }
            }
            bw.Write("\n");
            bw.Flush();
            //reset memory
            ms.Position = 0;
            //get encoding, I have no idea why encode page of 1252 works and fails for others
            string text = Encoding.GetEncoding(1252).GetString(ms.ToArray());
            ms.Close();
            bw.Close();
            return text;
        }
    }
}
