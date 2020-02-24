using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.Annotation;
using Android.Widget;

namespace Android.Material.Chips
{
    public class ChipUtils
    {
        [IdRes]
        public const int IMAGE_ID = 0x00910518;
        [IdRes]
        public const int TEXT_ID = 0x00059118;

        public static readonly Color[] colors =
        {
            new Color(0xd32f2f),
            new Color(0xC2185B),
            new Color(0x7B1FA2),
            new Color(0x512DA8),
            new Color(0x303F9F),
            new Color(0x1976D2),
            new Color(0x0288D1),
            new Color(0x0097A7),
            new Color(0x00796B),
            new Color(0x388E3C),
            new Color(0x689F38),
            new Color(0xAFB42B),
            new Color(0xFBC02D),
            new Color(0xFFA000),
            new Color(0xF57C00),
            new Color(0xE64A19),
            new Color(0x5D4037),
            new Color(0x616161),
            new Color(0x455A64),
        };

        public static Bitmap GetScaledBitmap(Context context, Bitmap bitmap)
        {
            int width = (int)context.Resources.GetDimension(Resource.Dimension.chip_height);
            return Bitmap.CreateScaledBitmap(bitmap, width, width, false);
        }

        public static Bitmap GetSquareBitmap(Bitmap bitmap)
        {
            Bitmap outPut;
            if (bitmap.Width >= bitmap.Height)
            {
                outPut = Bitmap.CreateBitmap(bitmap, bitmap.Width / 2 - bitmap.Height / 2,
                    0,
                    bitmap.Height,
                    bitmap.Height);
            }
            else
            {
                outPut = Bitmap.CreateBitmap(bitmap, 0, bitmap.Height / 2 - bitmap.Width / 2,
                    bitmap.Width,
                    bitmap.Width);
            }
            return outPut;
        }

        public static Bitmap GetCircleBitmap(Context context, Bitmap bitmap)
        {
            int width = (int)context.Resources.GetDimension(Resource.Dimension.chip_height);
            Bitmap output = Bitmap.CreateBitmap(width, width, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);
            Color color = Color.Red;
            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, width, width);
            RectF rectF = new RectF(rect);
            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            paint.Color = color;
            canvas.DrawOval(rectF, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, rect, rect, paint);
            return output;
        }

        public static Bitmap GetCircleBitmapWithText(Context context, string text, Color bgColor, Color textColor)
        {
            int width = (int)context.Resources.GetDimension(Resource.Dimension.chip_height);
            Bitmap output = Bitmap.CreateBitmap(width, width, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);
            Paint paint = new Paint();
            Paint textPaint = new Paint();
            Rect rect = new Rect(0, 0, width, width);
            RectF rectF = new RectF(rect);

            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            paint.Color = bgColor;
            canvas.DrawOval(rectF, paint);
            textPaint.Color = textColor;
            textPaint.StrokeWidth = 30;
            textPaint.TextSize = 50;
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcOver));
            textPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcAtop));

            int xPos = 0, yPos = 0;
            if (text.Length == 1)
            {
                xPos = (int)((canvas.Width / 1.9) + ((textPaint.Descent() + textPaint.Ascent()) / 2));
                yPos = (int)((canvas.Height / 2) - ((textPaint.Descent() + textPaint.Ascent()) / 2));
            }
            else
            {
                xPos = (int)((canvas.Width / 2.7) + ((textPaint.Descent() + textPaint.Ascent()) / 2));
                yPos = (int)((canvas.Height / 2) - ((textPaint.Descent() + textPaint.Ascent()) / 2));
            }
            canvas.DrawBitmap(output, rect, rect, paint);
            canvas.DrawText(text, xPos, yPos, textPaint);
            return output;
        }

        public static string GenerateText(string iconText)
        {
            if (string.IsNullOrEmpty(iconText))
            {
                throw new Java.Lang.IllegalArgumentException($"Icon text must have at least one symbol");
            }
            if(iconText.Length == 1 || iconText.Length == 2)
            {
                return iconText;
            }
            string[] parts = iconText.Split(' ');
            if(parts.Length == 1)
            {
                string text = parts[0];
                text = text.Substring(0, 2);
                string f = text.Substring(0, 1);
                string s = text.Substring(1, 1);
                f = f.ToUpper();
                s = s.ToLower();
                text = f + s;
                return text;
            }
            string first = parts[0];
            string second = parts[1];
            first = first.Substring(0, 1);
            first = first.ToUpper();
            second = second.Substring(0, 1);
            second = second.ToUpper();
            return first + second;
        }

        public static void SetIconColor(ImageView image, Color color)
        {
            Drawable drawable = image.Drawable;
            image.SetColorFilter(new PorterDuffColorFilter(color, PorterDuff.Mode.SrcAtop));
            image.SetImageDrawable(drawable);
        }
    }
}