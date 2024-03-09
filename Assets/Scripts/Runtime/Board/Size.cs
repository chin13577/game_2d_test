namespace FS
{
    public struct Size
    {
        public int width;
        public int height;

        public int TotalSize { get => width * height; }

        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}