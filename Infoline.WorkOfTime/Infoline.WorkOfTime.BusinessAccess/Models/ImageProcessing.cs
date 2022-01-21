////using SixLabors.ImageSharp;
////using SixLabors.ImageSharp.Drawing;
////using SixLabors.ImageSharp.Drawing.Processing;
////using SixLabors.ImageSharp.PixelFormats;
////using SixLabors.ImageSharp.Processing;
////using System;
////using System.IO;
////using System.Web;

////namespace Infoline.WorkOfTime.BusinessAccess
////{
////    public static class ImageProcessing
////    {

////        public static Byte[] MergePictures(string img1Dir, string img2Dir)
////        {

////            //  img1Dir = Alttaki resim.
////            //  img2Dir = Üstüne gelecek ve yuvarlak yapılacak resim.

////            using (Image prof = Image.Load(HttpContext.Current.Server.MapPath(img2Dir)))
////            {
////                using (Image markk = Image.Load(HttpContext.Current.Server.MapPath(img1Dir)))
////                {

                    

////                    prof.Mutate(a => a.AutoOrient().Resize(new ResizeOptions
////                    {
////                        Mode=ResizeMode.Crop,
////                        Position=AnchorPositionMode.Center,
////                        Size= new Size(40,40),
                        
////                    }).BackgroundColor(Color.Transparent));


////                    markk.Mutate(a => a.DrawImage(prof, new SixLabors.ImageSharp.Point(12, 1), 1f));
                    
////                    prof.Save("c:/tt/img1.jpg");
////                    markk.Save("c:/tt/img2.jpg");

////                    using (var ms = new MemoryStream())
////                    {
////                        prof.Save(ms, SixLabors.ImageSharp.Formats.Png.PngFormat.Instance);

////                        return ms.ToArray();

////                    }

////                }

////            }
////        }


////        private static IImageProcessingContext ApplyRoundedCorners(this IImageProcessingContext ctx, float cornerRadius)
////        {
////            var size = ctx.GetCurrentSize();
////            var corners = BuildCorners(size.Width, size.Height, cornerRadius);

////            ctx.SetGraphicsOptions(new GraphicsOptions()
////            {
////                Antialias = true,
////                AlphaCompositionMode = PixelAlphaCompositionMode.DestOut
////            });

////            foreach (var c in corners)
////            {
////                ctx = ctx.Fill(SixLabors.ImageSharp.Color.Red, c);
////            }
////            return ctx;
////        }

////        private static IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
////        {
////            // first create a square
////            var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

////            // then cut out of the square a circle so we are left with a corner
////            IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

////            // corner is now a corner shape positions top left
////            //lets make 3 more positioned correctly, we can do that by translating the original around the center of the image

////            float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
////            float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

////            // move it across the width of the image - the width of the shape
////            IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
////            IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
////            IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

////            return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
////        }

////        private static IImageProcessingContext ConvertToAvatar(this IImageProcessingContext processingContext, Size size, float cornerRadius)
////        {
////            return processingContext.Resize(new ResizeOptions
////            {
////                Size = size,
////                Mode = ResizeMode.BoxPad
////            }).ApplyRoundedCorners(cornerRadius);
////        }

////    }
////}
