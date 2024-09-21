namespace Leditor.Leditor
{
    public class TextCursor
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public int lastPaintedX, lastPaintedY;

        public bool isShowing;

        private const int showInterval = 500;
        private int timer = 0;

        public TextCursor()
        {
            x = 0;
            y = 25;
            w = 1;
            h = 16;
            isShowing = true;
        }
        public void Update(int dt)
        {
            timer += dt;
            if(timer >= showInterval)
            {
                isShowing = !isShowing;
                timer = 0;
            }
        }

        public void Painted()
        {
            lastPaintedX = x;
            lastPaintedY = y;
        }

        public bool NewPaintedPosition()
        {
            return lastPaintedX != x || lastPaintedY != y;
        }
    }
}
