// https://softwarebydefault.com/2013/04/12/bitmap-color-tint/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Revlex.RadarOverlay
{
    public static class ImageEffect
    {

        public static Bitmap ColorTint(this Bitmap sourceBitmap, int redTint, int greenTint, int blueTint)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float blue = 0;
            float green = 0;
            float red = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length+1; k += 4)
            {
                blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * ((float)blueTint / (float)255);
                green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * ((float)greenTint / (float)255);
                red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * ((float)redTint / (float)255);
                if (blue > 255)
                { blue = 255; }

                if (green > 255)
                { green = 255; }

                if (red > 255)
                { red = 255; }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }

        public static Image RotateImage(Image img, float rotationAngle)
        {
            // When drawing the returned image to a form, modify your points by 
            // (-(img.Width / 2) - 1, -(img.Height / 2) - 1) to draw for actual co-ordinates.

            //create an empty Bitmap image 
            Bitmap bmp = new Bitmap((img.Width * 2), (img.Height * 2));

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //set the point system origin to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            //move the point system origin back to 0,0
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size

            //draw our new image onto the graphics object with its center on the center of rotation
            gfx.DrawImage(img, new PointF((img.Width / 2), (img.Height / 2)));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
        public static Bitmap rotateImageUsi(Bitmap bitmap, float angle)
        {
            Bitmap returnBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics graphics = Graphics.FromImage(returnBitmap);

            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //graphics.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
            //graphics.RotateTransform(angle);
            //graphics.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
            graphics.DrawImage(bitmap, new Point(0, 0));
            return returnBitmap;
        }
    }
}
